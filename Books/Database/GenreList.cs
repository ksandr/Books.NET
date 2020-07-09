using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class GenreList
    {
        public int BookId { get; set; }
        public int GenreId { get; set; }

        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }

    public class GenreListConfiguration : IEntityTypeConfiguration<GenreList>
    {
        public void Configure(EntityTypeBuilder<GenreList> builder)
        {
            builder.ToTable("Genre_List").HasKey(x => new { x.BookId, x.GenreId });

            builder.Property(x => x.BookId).HasColumnName("BookID");
            builder.Property(x => x.GenreId).HasColumnName("GenreID");

            builder.HasOne(x => x.Book).WithMany(x => x.GenreList).HasForeignKey(x => x.BookId).IsRequired();
            builder.HasOne(x => x.Genre).WithMany(x => x.GenreList).HasForeignKey(x => x.GenreId).IsRequired();
        }
    }
}
