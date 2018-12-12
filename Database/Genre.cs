using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class Genre
    {
        public int Id { get; set; }
        public string Fb2Code { get; set; }
        public string Name { get; set; }
        public string Search { get; set; }
        public virtual IList<GenreList> GenreList { get; set; }
    }

    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres").HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("GenreID").IsRequired();
            builder.Property(x => x.Fb2Code).HasColumnName("Fb2Code");
            builder.Property(x => x.Name).HasColumnName("GenreAlias");
            builder.Property(x => x.Search).HasColumnName("Search");
            builder.HasMany(x => x.GenreList).WithOne(x => x.Genre).HasForeignKey(x => x.GenreId);
        }
    }
}
