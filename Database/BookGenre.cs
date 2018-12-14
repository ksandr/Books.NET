using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class BookGenre
    {
        public int BookId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.ToTable("v_Book_Genres").HasKey(x => new { x.BookId, x.Id });
            builder.Property(x => x.BookId).HasColumnName("BookID");
            builder.Property(x => x.Id).HasColumnName("GenreID");
            builder.Property(x => x.Name).HasColumnName("GenreAlias");
        }
    }
}
