using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ksandr.Books.Import.Entities;

namespace Ksandr.Books.Import
{
    public interface IDatabase : IDisposable
    {
        void Close();

        void BeginTransaction();
        void Commit();
        void Rollback();

        Task EnsureSchemaAsync();
        Task ClearAsync();
        void Prepare();

        Task InsertBookAsync(Book book, Series series);
        Task InsertBookAuthorsAsync(Book book, IEnumerable<Author> authors);
        Task InsertBookGenresAsync(Book book, IEnumerable<Genre> genres);
    }
}
