using System;
using Ksandr.Books.Import.Entities.Abstract;

namespace Ksandr.Books.Import.Entities
{
    public class Series : Entity
    {
        public Series(string title)
        {
            Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        }

        public string Title { get; set; }
    }
}
