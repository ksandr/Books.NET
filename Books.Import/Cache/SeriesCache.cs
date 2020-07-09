using System;
using System.Collections.Generic;
using Ksandr.Books.Import.Entities;

namespace Ksandr.Books.Import.Cache
{
    public class SeriesCache
    {
        private readonly Dictionary<string, Series> _cache;

        public SeriesCache()
        {
            _cache = new Dictionary<string, Series>();
        }

        public Series GetOrAdd(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));

            string search = title.ToUpper();

            if (_cache.TryGetValue(search, out Series series))
                return series;

            series = new Series(title);

            _cache.Add(search, series);

            return series;
        }
    }
}
