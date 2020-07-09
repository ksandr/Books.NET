using System;
using System.Collections.Generic;
using Ksandr.Books.Import.Entities;

namespace Ksandr.Books.Import.Cache
{
    public class GenresCache
    {
        private readonly Dictionary<string, Genre> _cache;

        public GenresCache(IEnumerable<Genre> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            _cache = new Dictionary<string, Genre>();

            foreach (Genre genre in values)
            {
                if (Get(genre.Fb2Code) != null)
                    continue;

                Add(genre);
            }
        }

        private void Add(Genre value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _cache.Add(value.Fb2Code.ToUpper(), value);
        }

        public Genre Get(string fb2Code)
        {
            string searchFb2Code = fb2Code.ToUpper();

            if (_cache.TryGetValue(searchFb2Code, out Genre genre))
                return genre;

            return null;
        }
    }
}
