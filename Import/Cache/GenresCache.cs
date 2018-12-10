using System.Collections.Generic;
using System.Threading.Tasks;
using Ksandr.Books.Database;
using Microsoft.EntityFrameworkCore;

namespace Ksandr.Books.Import.Cache
{
    internal class GenresCache
    {
        private readonly BooksContext _db;
        private readonly Dictionary<string, Genre> _cache;

        public GenresCache(BooksContext db)
        {
            _db = db;
            _cache = new Dictionary<string, Genre>();
        }

        public async Task<Genre> GetAsync(string fb2Code)
        {
            string searchFb2Code = fb2Code.ToUpper();

            if (_cache.TryGetValue(searchFb2Code, out Genre genre))
                return genre;

            genre = await _db.Genres.FirstOrDefaultAsync(x => x.Fb2Code.ToUpper() == searchFb2Code);

            _cache.Add(searchFb2Code, genre);

            return genre;
        }
    }
}
