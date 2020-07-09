using System;
using System.IO;

namespace Ksandr.Books.Import.Inpx
{
    public class InpxEntry
    {
        public InpxEntry(string name, Stream entry)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        public string Name { get; private set; }
        public Stream Entry { get; private set; }
    }
}
