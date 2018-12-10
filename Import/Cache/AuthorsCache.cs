using System.Collections.Generic;
using System.Threading.Tasks;
using Ksandr.Books.Database;
using Microsoft.EntityFrameworkCore;

namespace Ksandr.Books.Import.Cache
{
    internal class AuthorsCache
    {
        private readonly BooksContext _db;
        private readonly Dictionary<string, Author> _cache;

        public AuthorsCache(BooksContext db)
        {
            _db = db;
            _cache = new Dictionary<string, Author>();
        }

        public async Task<Author> GetAsync(AuthorRecord authorName)
        {
            string searchName = authorName.ToString().ToUpper();

            if (_cache.TryGetValue(searchName, out Author author))
                return author;


            author = await _db.Authors.FirstOrDefaultAsync(x => x.Search == searchName);
            if (author == null)
            {
                author = new Author()
                {
                    LastName = authorName.LastName,
                    FirstName = authorName.FirstName,
                    MiddleName = authorName.MiddleName,
                    Search = searchName
                };

                _db.Add(author);
            }

            _cache.Add(searchName, author);

            return author;
        }
    }
}
