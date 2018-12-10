using System;

namespace Ksandr.Books.Import
{
    public class AuthorRecord
    {
        public AuthorRecord(string lastName, string firstName, string middleName)
        {
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));

            FirstName = string.IsNullOrWhiteSpace(firstName) ? null : firstName;
            MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName;
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public static AuthorRecord Parse(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            string[] names = name.Split(',');

            string lastName = names[0];
            string firstName = names.Length > 1 ? names[1] : null;
            string middleName = names.Length > 2 && names[2] != null ? names[2] : null;

            return new AuthorRecord(lastName, firstName, middleName);
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}".Trim();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AuthorRecord item))
                return false;

            return string.Equals(ToString(), item.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
