using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace Ksandr.Books.Import
{
    public class InpxReader : IDisposable
    {
        private readonly ZipArchive _zip;

        public InpxReader(string fileName)
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

        public IEnumerable<InpRecord> ReadInpx()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            return ReadInpx(token);
        }

        public IEnumerable<InpRecord> ReadInpx(CancellationToken cancellationToken)
        {
            foreach (InpxEntry inpx in LoadInpx(cancellationToken))
            {
                foreach (InpRecord record in LoadInp(inpx, cancellationToken))
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;

                    yield return record;
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

        private IEnumerable<InpRecord> LoadInp(InpxEntry entry, CancellationToken cancellationToken)
        {
            int index = 0;
            using (StreamReader reader = new StreamReader(entry.Entry))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;

                    InpRecord record = InpRecord.Parse(entry.Name, index, line);
                    index++;

                    if (record.IsDeleted && SkipDeleted)
                        continue;

                    if ((Languages.Count() > 0)
                        && (!Languages.Any(x => string.Equals(x, record.Lang, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        continue;
                    }

                    yield return record;
                }
            }
        }
    }
}
