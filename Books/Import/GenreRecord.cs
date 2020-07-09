using System;
using System.Text.RegularExpressions;

namespace Ksandr.Books.Import
{
    public class GenreRecord
    {
        private static readonly Regex _regex = new Regex(@"^((\d+\.?)+)\s((.+);)?(.+)$");

        public GenreRecord(string id, string name, string fb2Code = null)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;

            Fb2Code = string.IsNullOrWhiteSpace(fb2Code) ? null : fb2Code;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Fb2Code { get; private set; }

        public string ParentId { get => Id.Substring(0, Id.LastIndexOf('.')); }

        public static GenreRecord Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentNullException(nameof(line));

            Match match = _regex.Match(line);
            if (!match.Success)
                throw new ArgumentException($"Can not parse line: {line}", nameof(line));

            GenreRecord result = new GenreRecord(match.Groups[1].Value, match.Groups[5].Value, match.Groups[4].Value);

            return result;
        }

        public override string ToString()
        {
            string fb2CodeStr = Fb2Code != null ? $"{Fb2Code};" : "";
            return $"{Id} {fb2CodeStr}{Name}";
        }
    }
}
