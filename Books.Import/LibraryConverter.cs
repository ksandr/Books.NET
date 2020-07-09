using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ksandr.Books.Import.Cache;
using Ksandr.Books.Import.Entities;
using Ksandr.Books.Import.Readers;
using Microsoft.Extensions.Logging;

namespace Ksandr.Books.Import
{
    public class LibraryConverter
    {
        private readonly GenresReader2 _genresReader;
        private readonly BooksReader2 _booksReader;
        private readonly ILogger<LibraryConverter> _logger;

        public LibraryConverter(GenresReader2 genresReader, BooksReader2 booksReader, ILogger<LibraryConverter> logger)
        {
            _genresReader = genresReader ?? throw new ArgumentNullException(nameof(genresReader));
            _booksReader = booksReader ?? throw new ArgumentNullException(nameof(booksReader));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ConvertAsync(IDatabase db)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancelTokenSource.Token;

            await ConvertAsync(db, cancellationToken);
        }

        public async Task ConvertAsync(IDatabase db, CancellationToken cancellationToken)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            if (cancellationToken == null)
                throw new ArgumentNullException(nameof(cancellationToken));

            if (cancellationToken.IsCancellationRequested)
                return;

            GenresCache2 genresCache = new GenresCache2(_genresReader.Read(cancellationToken));
            AuthorsCache2 authorsCache = new AuthorsCache2();
            SeriesCache2 seriesCache = new SeriesCache2();

            await db.EnsureSchemaAsync();
            await db.ClearAsync();

            db.BeginTransaction();

            db.Prepare();

            int i = 0;
            foreach (Book book in _booksReader.Read(cancellationToken))
            {
                Series series = book.Series != null ? seriesCache.GetOrAdd(book.Series) : null;
                Author[] authors = book.Authors.Select(x => authorsCache.GetOrAdd(x)).ToArray();
                Genre[] genres = book.Genres.Select(x => genresCache.Get(x)).Where(x => x != null).ToArray();

                await db.InsertBookAsync(book, series);
                await db.InsertBookAuthorsAsync(book, authors);
                await db.InsertBookGenresAsync(book, genres);

                i++;
                if (i % 10000 == 0)
                    _logger.LogInformation("{0} books processed...", i);
            }
            _logger.LogInformation("{0} books processed...", i);

            db.Commit();
            db.Close();
            _logger.LogInformation("Done!", i);
        }
    }
}
