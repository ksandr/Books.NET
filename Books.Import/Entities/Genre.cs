using System;
using System.Text.RegularExpressions;
using Ksandr.Books.Import.Entities.Abstract;

namespace Ksandr.Books.Import.Entities
{
    public class Genre : Entity
    {
        //((0).1) (())(Фантастика)
        //((0.)?1.0) ((sf);)?(Научная фантастика)
        private static readonly Regex _regex = new Regex(@"^((\d+\.?)+)\s((.+);)?(.+)$");

        public Genre(string code, string name, string fb2Code = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Code = code;
            Name = name;

            Fb2Code = string.IsNullOrWhiteSpace(fb2Code) ? null : fb2Code;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Fb2Code { get; private set; }

        public string ParentCode { get => Code.Substring(0, Code.LastIndexOf('.')); }
        public bool IsFb2 { get => Fb2Code != null; }

        public static Genre Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentNullException(nameof(line));

            Match match = _regex.Match(line);
            if (!match.Success)
                throw new ArgumentException($"Invalid format: {line}", nameof(line));

            Genre result = new Genre(match.Groups[1].Value, match.Groups[5].Value, match.Groups[4].Value);
            return result;
        }

        public override string ToString()
        {
            string fb2CodeStr = Fb2Code != null ? $"{Fb2Code};" : "";
            return $"{Code} {fb2CodeStr}{Name}";
        }
    }
}
