using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using Ksandr.Books.Import.Entities;
using Ksandr.Books.Import.Inpx;

namespace Ksandr.Books.Import.Readers
{
    public class BooksReader2 : IDisposable
    {
        private readonly ZipArchive _zip;

        public BooksReader2(string fileName)
        {
            _zip = ZipFile.OpenRead(fileName);

            Languages = Enumerable.Empty<string>();
        }

        public void Dispose()
        {
            _zip.Dispose();
        }

        public bool SkipDeleted { get; set; }
        public IEnumerable<string> Languages { get; set; }

        public IEnumerable<Book> Read()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            return Read(token);
        }

        public IEnumerable<Book> Read(CancellationToken cancellationToken)
        {
            foreach (InpxEntry inpx in LoadInpx(cancellationToken))
            {
                foreach (Book book in LoadBook(inpx, cancellationToken))
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;

                    yield return book;
                }
            }
        }

        private IEnumerable<InpxEntry> LoadInpx(CancellationToken cancellationToken)
        {
            foreach (ZipArchiveEntry entry in _zip.Entries)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                if (!entry.FullName.EndsWith(".inp"))
                    continue;

                string name = Path.ChangeExtension(entry.FullName, ".zip");
                InpxEntry inpx = new InpxEntry(name, entry.Open());

                yield return inpx;
            }
        }

        private IEnumerable<Book> LoadBook(InpxEntry entry, CancellationToken cancellationToken)
        {
            int index = 0;
            using (StreamReader reader = new StreamReader(entry.Entry))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;

                    Book book = Book.Parse(entry.Name, index, line);
                    index++;

                    if (book.IsDeleted && SkipDeleted)
                        continue;

                    if ((Languages.Count() > 0)
                        && (!Languages.Any(x => string.Equals(x, book.Lang, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        continue;
                    }

                    yield return book;
                }
            }
        }
    }
}
