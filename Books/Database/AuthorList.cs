using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class AuthorList
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }

    public class AuthorListConfiguration : IEntityTypeConfiguration<AuthorList>
    {
        public void Configure(EntityTypeBuilder<AuthorList> builder)
        {
            builder.ToTable("Author_List").HasKey(x => new { x.BookId, x.AuthorId });

            builder.Property(x => x.BookId).HasColumnName("BookID");
            builder.Property(x => x.AuthorId).HasColumnName("AuthorID");

            builder.HasOne(x => x.Book).WithMany(x => x.AuthorList).HasForeignKey(x => x.BookId).IsRequired();
            builder.HasOne(x => x.Author).WithMany(x => x.BookAuthors).HasForeignKey(x => x.AuthorId).IsRequired();
        }
    }
}
