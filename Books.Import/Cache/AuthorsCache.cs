using System;
using System.Collections.Generic;
using Ksandr.Books.Import.Entities;

namespace Ksandr.Books.Import.Cache
{
    public class AuthorsCache
    {
        private readonly Dictionary<string, Author> _cache;

        public AuthorsCache()
        {
            _cache = new Dictionary<string, Author>();
        }

        public Author GetOrAdd(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            string searchName = value.ToUpper();

            if (_cache.TryGetValue(searchName, out Author author))
                return author;

            author = Author.Parse(value);
            _cache.Add(searchName, author);

            return author;
        }
    }
}
