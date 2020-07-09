using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class BookAuthor
    {
        public int BookId { get; set; }

        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
    }

    public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
    {
        public void Configure(EntityTypeBuilder<BookAuthor> builder)
        {
            builder.ToTable("v_Book_Authors").HasKey(x => new { x.BookId, x.Id });
            builder.Property(x => x.BookId).HasColumnName("BookID");
            builder.Property(x => x.Id).HasColumnName("AuthorID").IsRequired();
            builder.Property(x => x.LastName).HasColumnName("LastName");
            builder.Property(x => x.FirstName).HasColumnName("FirstName");
            builder.Property(x => x.MiddleName).HasColumnName("MiddleName");
        }
    }
}
