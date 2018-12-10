using System;
using System.Collections.Generic;
using System.Linq;

namespace Ksandr.Books.Import
{
    public class InpRecord
    {
        public int Index { get; set; }
        public string Folder { get; set; }

        public AuthorRecord[] Authors { get; set; }
        public string[] Genres { get; set; }

        public string Title { get; set; }
        public string Series { get; set; }
        public string SeqNumber { get; set; }
        public string LibId { get; set; }
        public int BookSize { get; set; }
        public string FileName { get; set; }
        public bool IsDeleted { get; set; }
        public string Ext { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Lang { get; set; }
        public int? LibRate { get; set; }
        public string KeyWords { get; set; }

        public static InpRecord Parse(string name, int index, string line)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentNullException(nameof(line));

            string[] data = line.Split('\u0004');

            if (data.Length != 15)
                throw new ArgumentException($"Cannot parse line #{index} in {name}: '{line}'", nameof(line));

            InpRecord result = new InpRecord();
            result.Index = index;
            result.Folder = name;

            result.Authors = ParseAuthors(data[0]);
            result.Genres = ParseGenres(data[1]);

            result.Title = data[2];

            if (!string.IsNullOrWhiteSpace(data[3]))
            {
                result.Series = data[3];
                result.SeqNumber = data[4] != "" ? data[4] : null;
            }
            result.LibId = data[5];
            result.BookSize = int.Parse(data[6]);
            result.FileName = data[7];
            result.IsDeleted = data[8] == "1";
            result.Ext = data[9];
            result.UpdateDate = DateTime.Parse(data[10]);
            result.Lang = data[11];
            result.LibRate = data[12] != "" ? int.Parse(data[12]) : (int?)null;
            result.KeyWords = data[13] != "" ? data[13] : null;

            return result;
        }

        private static AuthorRecord[] ParseAuthors(string authors)
        {
            if (string.IsNullOrWhiteSpace(authors))
                return new AuthorRecord[] { };

            return authors.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => AuthorRecord.Parse(x))
                .Distinct()
                .ToArray();
        }

        private static string[] ParseGenres(string genres)
        {

            if (string.IsNullOrWhiteSpace(genres))
                return new string[] { };

            return genres.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
        }
    }
}
