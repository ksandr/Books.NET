using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class Series
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Search { get; set; }
        public virtual IList<Book> Books { get; set; }
    }

    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            builder.ToTable("Series").HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("SeriesID").IsRequired();
            builder.Property(x => x.Title).HasColumnName("SeriesTitle").IsRequired();
            builder.Property(x => x.Search).HasColumnName("Search").IsRequired();
            builder.HasMany(x => x.Books).WithOne(x => x.Series).HasForeignKey("SeriesID");
        }
    }
}
