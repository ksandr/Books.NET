using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksandr.Books.Database
{
    public class Book
    {
        public int Id { get; set; }

        public string LibId { get; set; }
        public string Title { get; set; }
        public Series Series { get; set; }
        public string SeqNumber { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? LibRate { get; set; }
        public string Lang { get; set; }
        public string Folder { get; set; }
        public string FileName { get; set; }
        public int InsideNo { get; set; }
        public string Ext { get; set; }
        public int BookSize { get; set; }
        public bool IsLocal { get; set; }
        public bool IsDeleted { get; set; }
        public string KeyWords { get; set; }
        public int Rate { get; set; }
        public int Progress { get; set; }
        public string Annotation { get; set; }
        public string Review { get; set; }
        public string SearchTitle { get; set; }
        public string SearchLang { get; set; }
        public string SearchFolder { get; set; }
        public string SearchFileName { get; set; }
        public string SearchExt { get; set; }
        public string SearchKeyWords { get; set; }
        public string SearchAnnotation { get; set; }

        public virtual IList<AuthorList> AuthorList { get; set; }
        public virtual IList<GenreList> GenreList { get; set; }

        public virtual IList<BookGenre> Genres { get; set; }
        public virtual IList<BookAuthor> Authors { get; set; }
    }

    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books").HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("BookID").IsRequired();

            builder.Property(x => x.LibId).HasColumnName("LibId");
            builder.Property(x => x.Title).HasColumnName("Title");
            builder.Property(x => x.SeqNumber).HasColumnName("SeqNumber");
            builder.Property(x => x.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(x => x.LibRate).HasColumnName("LibRate");
            builder.Property(x => x.Lang).HasColumnName("Lang");
            builder.Property(x => x.Folder).HasColumnName("Folder");
            builder.Property(x => x.FileName).HasColumnName("FileName");
            builder.Property(x => x.InsideNo).HasColumnName("InsideNo");
            builder.Property(x => x.Ext).HasColumnName("Ext");
            builder.Property(x => x.BookSize).HasColumnName("BookSize");
            builder.Property(x => x.IsLocal).HasColumnName("IsLocal");
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(x => x.KeyWords).HasColumnName("KeyWords");
            builder.Property(x => x.Rate).HasColumnName("Rate");
            builder.Property(x => x.Progress).HasColumnName("Progress");
            builder.Property(x => x.Annotation).HasColumnName("Annotation");
            builder.Property(x => x.Review).HasColumnName("Review");
            builder.Property(x => x.SearchTitle).HasColumnName("SearchTitle");
            builder.Property(x => x.SearchLang).HasColumnName("SearchLang");
            builder.Property(x => x.SearchFolder).HasColumnName("SearchFolder");
            builder.Property(x => x.SearchFileName).HasColumnName("SearchFileName");
            builder.Property(x => x.SearchExt).HasColumnName("SearchExt");
            builder.Property(x => x.SearchKeyWords).HasColumnName("SearchKeyWords");
            builder.Property(x => x.SearchAnnotation).HasColumnName("SearchAnnotation");

            builder.HasOne(x => x.Series).WithMany(x => x.Books).HasForeignKey("SeriesId");

            builder.HasMany(x => x.AuthorList).WithOne(x => x.Book).HasForeignKey(x => x.BookId);
            builder.HasMany(x => x.GenreList).WithOne(x => x.Book).HasForeignKey(x => x.BookId);

            builder.HasMany(x => x.Genres).WithOne().HasForeignKey(x => x.BookId);
            builder.HasMany(x => x.Authors).WithOne().HasForeignKey(x => x.BookId);
        }
    }
}
