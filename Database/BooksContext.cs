using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Books.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ksandr.Books.Database
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new SeriesConfiguration());

            modelBuilder.ApplyConfiguration(new AuthorListConfiguration());
            modelBuilder.ApplyConfiguration(new GenreListConfiguration());

            modelBuilder.ApplyConfiguration(new BookAuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookGenreConfiguration());
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Series> Series { get; set; }

        public void CreateSchema()
        {
            var assembly = typeof(BooksContext).GetTypeInfo().Assembly;
            string[] sqlResourceNames = assembly.GetManifestResourceNames()
                .Where(x => x.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase))
                .ToArray();

            foreach (string sqlResourceName in sqlResourceNames)
            {
                Stream sqlResourceStream = assembly.GetManifestResourceStream(sqlResourceName);
                byte[] sqlBytes = sqlResourceStream.ReadBytes();
                string sql = Encoding.UTF8.GetString(sqlBytes);

                Database.ExecuteSqlCommand(sql);
            }
        }

        public void Vacuum()
        {
            Database.ExecuteSqlCommand("VACUUM");
        }

        public void Clear()
        {
            using (IDbContextTransaction tx = Database.BeginTransaction())
            {
                Database.ExecuteSqlCommand("DELETE FROM Books");
                Database.ExecuteSqlCommand("DELETE FROM Author_List");
                Database.ExecuteSqlCommand("DELETE FROM Genre_List");
                Database.ExecuteSqlCommand("DELETE FROM Authors");
                Database.ExecuteSqlCommand("DELETE FROM Genres");
                Database.ExecuteSqlCommand("DELETE FROM Series");

                Database.ExecuteSqlCommand("DELETE FROM sqlite_sequence");
                tx.Commit();
            }
        }
    }
}
