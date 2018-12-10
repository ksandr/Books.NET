using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Ksandr.Books.Import
{
    public class GenresReader : IDisposable
    {
        private readonly StreamReader _reader;

        public GenresReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            _reader = new StreamReader(stream);
        }

        public GenresReader(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            _reader = new StreamReader(path);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool Fb2Only { get; set; }

        public IEnumerable<GenreRecord> ReadGenres(CancellationToken cancellationToken)
        {
            foreach (string line in ReadLines(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                GenreRecord record = GenreRecord.Parse(line);

                if (Fb2Only && record.Fb2Code == null)
                    continue;

                yield return record;
            }
        }

        public IEnumerable<GenreRecord> ReadGenres()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            return ReadGenres(token);
        }

        private IEnumerable<string> ReadLines(CancellationToken cancellationToken)
        {
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                if (line.StartsWith("#"))
                    continue;

                yield return line;
            }
        }
    }
}
