using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class Author
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Search { get; set; }
        public virtual IList<AuthorList> BookAuthors { get; set; }
    }

    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors").HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("AuthorID").IsRequired();

            builder.Property(x => x.LastName).HasColumnName("LastName").IsRequired();
            builder.Property(x => x.FirstName).HasColumnName("FirstName");
            builder.Property(x => x.MiddleName).HasColumnName("MiddleName");
            builder.Property(x => x.Search).HasColumnName("Search").IsRequired();
            builder.HasMany(x => x.BookAuthors).WithOne().HasForeignKey(x => x.AuthorId);
        }
    }
}
