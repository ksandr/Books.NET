using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ksandr.Books.Database;
using Ksandr.Books.Import.Cache;
using Microsoft.Extensions.Logging;

namespace Ksandr.Books.Import
{
    public class ImportService
    {
        private readonly BooksContext _db;
        private readonly ILogger<ImportService> _logger;

        private readonly AuthorsCache _authorsCache;
        private readonly GenresCache _genresCache;
        private readonly SeriesCache _seriesCache;

        public ImportService(BooksContext db, ILogger<ImportService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _authorsCache = new AuthorsCache(db);
            _genresCache = new GenresCache(db);
            _seriesCache = new SeriesCache(db);
        }

        public async Task StartAsync(string inpxFile, string genresFile, IEnumerable<string> languages, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Import parameters: inpxFile={0}, genresFile={1} ", inpxFile, genresFile);

            if (!File.Exists(inpxFile))
                throw new FileNotFoundException($"INPX file '{inpxFile}' not found");
            if (!File.Exists(genresFile))
                throw new FileNotFoundException($"Genres file '{genresFile}' not found");


            _logger.LogInformation("Clean database...");
            _db.CreateSchema();
            _db.Clear();

            if (cancellationToken.IsCancellationRequested)
                return;

            _logger.LogInformation("Loading genres...");
            await LoadGenresAsync(genresFile, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                return;

            await LoadInpx(inpxFile, languages, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                return;

            _logger.LogInformation("Vacuum database...");
            _db.Vacuum();

            _logger.LogInformation("Done!");
        }

        private async Task LoadGenresAsync(string fileName, CancellationToken cancellationToken)
        {
            using (GenresReader reader = new GenresReader(fileName))
            {
                reader.Fb2Only = true;

                foreach (GenreRecord record in reader.ReadGenres(cancellationToken))
                {
                    Genre genre = new Genre()
                    {
                        GenreCode = record.Id,
                        Fb2Code = record.Fb2Code,
                        Name = record.Name,
                    };

                    _db.Add(genre);
                }
            }

            await _db.SaveChangesAsync();
        }

        private async Task LoadInpx(string fileName, IEnumerable<string> languages, CancellationToken cancellationToken)
        {
            using (InpxReader reader = new InpxReader(fileName))
            {
                reader.SkipDeleted = true;
                reader.Languages = languages;

                string inpName = "";
                foreach (InpRecord inp in reader.ReadInpx(cancellationToken))
                {
                    if (inp.Folder != inpName)
                    {
                        if (inpName != "")
                            await _db.SaveChangesAsync();

                        inpName = inp.Folder;
                        _logger.LogInformation("Loading inp {0}", inp.Folder);
                    }

                    await ParseInp(inp);
                }

                await _db.SaveChangesAsync();
            }
        }

        private async Task ParseInp(InpRecord inp)
        {
            Book book = new Book();
            book.LibId = inp.LibId;
            book.Title = inp.Title;
            if (!string.IsNullOrWhiteSpace(inp.Series))
            {
                book.Series = await _seriesCache.GetAsync(inp.Series);
                book.SeqNumber = inp.SeqNumber;
            }
            book.UpdateDate = inp.UpdateDate;

            book.LibRate = inp.LibRate;


            book.Lang = inp.Lang.ToUpper();
            book.Folder = inp.Folder;
            book.FileName = inp.FileName;
            book.InsideNo = inp.Index;
            book.Ext = "." + inp.Ext;
            book.BookSize = inp.BookSize;
            book.KeyWords = inp.KeyWords;

            book.Search = book.Title.ToUpper();

            book.GenreList = await GetBookGenres(book, inp.Genres);
            book.AuthorList = await GetBookAuthors(book, inp.Authors);

            _db.Add(book);
        }

        private async Task<List<GenreList>> GetBookGenres(Book book, IEnumerable<string> genres)
        {
            if (genres.Count() == 0)
                return null;

            List<GenreList> result = new List<GenreList>();
            foreach (string fb2Code in genres)
            {
                Genre genre = await _genresCache.GetAsync(fb2Code);
                if (genre != null)
                {
                    if (result.Any(x => x.Genre == genre))
                        continue;

                    result.Add(new GenreList()
                    {
                        Book = book,
                        Genre = genre
                    });
                }
            }

            if (result.Count == 0)
                return null;

            return result;
        }

        private async Task<List<AuthorList>> GetBookAuthors(Book book, IEnumerable<AuthorRecord> authors)
        {
            if (authors.Count() == 0)
                return null;

            List<AuthorList> result = new List<AuthorList>();
            foreach (AuthorRecord authorName in authors)
            {

                Author author = await _authorsCache.GetAsync(authorName);
                if (author != null)
                {
                    if (result.Any(x => x.Author == author))
                        continue;

                    result.Add(new AuthorList()
                    {
                        Book = book,
                        Author = author
                    });
                }
            }

            if (result.Count == 0)
                return null;

            return result;
        }
    }
}
