using System;
using System.IO;

namespace Ksandr.Books.Import
{
    public class InpxEntry
    {
        public InpxEntry(string name, Stream entry)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            Name = name;
            Entry = entry;
        }

        public string Name { get; private set; }
        public Stream Entry { get; private set; }
    }
}
