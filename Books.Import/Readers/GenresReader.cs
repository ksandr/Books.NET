using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Ksandr.Books.Import.Entities;

namespace Ksandr.Books.Import.Readers
{
    public class GenresReader : IDisposable
    {
        private readonly StreamReader _reader;
        private readonly Stream _stream;

        public GenresReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            _stream = stream;
            _reader = new StreamReader(stream);
        }

        public GenresReader(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            _stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            _reader = new StreamReader(path);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool Fb2Only { get; set; }


        public IEnumerable<Genre> Read()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancelTokenSource.Token;

            return Read(cancellationToken);
        }

        public IEnumerable<Genre> Read(CancellationToken cancellationToken)
        {
            foreach (string line in ReadLines(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                Genre record = Genre.Parse(line);

                if (Fb2Only && record.Fb2Code == null)
                    continue;

                yield return record;
            }

            _reader.Close();
        }

        private IEnumerable<string> ReadLines(CancellationToken cancellationToken)
        {
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.StartsWith("#"))
                    continue;



                yield return line.Trim();
            }
        }
    }
}
