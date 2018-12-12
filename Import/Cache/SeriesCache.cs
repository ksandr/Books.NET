using System.Collections.Generic;
using System.Threading.Tasks;
using Ksandr.Books.Database;
using Microsoft.EntityFrameworkCore;

namespace Ksandr.Books.Import.Cache
{
    internal class SeriesCache
    {
        private readonly BooksContext _db;
        private readonly Dictionary<string, Series> _cache;

        public SeriesCache(BooksContext db)
        {
            _db = db;
            _cache = new Dictionary<string, Series>();
        }

        public async Task<Series> GetAsync(string title)
        {
            string search = title.ToUpper();

            if (_cache.TryGetValue(search, out Series series))
                return series;


            series = await _db.Series.FirstOrDefaultAsync(x => x.Search == search);
            if (series == null)
            {
                series = new Series()
                {
                    Title = title,
                    Search = search
                };

                _db.Add(series);
            }

            _cache.Add(search, series);

            return series;
        }
    }
}
