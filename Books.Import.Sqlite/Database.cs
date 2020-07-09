using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Ksandr.Books.Import.Entities;
using Microsoft.Data.Sqlite;

namespace Ksandr.Books.Import.Sqlite
{
    public class Database : IDatabase
    {
        private readonly SqliteConnection _connection;

        private SqliteTransaction _transaction;
        private SqliteCommand _insertBooksCommand;

        public Database(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _transaction = null;

            _connection = new SqliteConnection(connectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            Close();
            _connection.Dispose();
        }


        public void Close()
        {
            if (_connection.State != ConnectionState.Open)
                return;

            if (_transaction != null)
                Commit();

            Vacuum();

            _connection.Close();
        }

        private void Vacuum()
        {
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "VACUUM";

            command.ExecuteNonQuery();
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Commit transaction first");

            _transaction =  _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Begin transaction first");

            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Begin transaction first");

            _transaction.Rollback();
            _transaction = null;
        }

        public async Task EnsureSchemaAsync()
        {
            Assembly assembly = GetType().Assembly;
            Stream resourceStream = assembly.GetManifestResourceStream(GetType().Namespace + ".schema.sql");

            StreamReader reader = new StreamReader(resourceStream);
            string schema = await reader.ReadToEndAsync();

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = schema;
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync();
        }

        public async Task ClearAsync()
        {
            try
            {
                BeginTransaction();

                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = @"
DELETE FROM Books;
DELETE FROM Author_List;
DELETE FROM Genre_List;
DELETE FROM Authors;
DELETE FROM Genres;
DELETE FROM Series;

DELETE FROM sqlite_sequence;";

                await command.ExecuteNonQueryAsync();
                Commit();
            }
            catch (Exception e)
            {
                Rollback();
                throw e;
            }
        }

        public void Prepare()
        {
            _insertBooksCommand = _connection.CreateCommand();
            _insertBooksCommand.CommandText = @"
INSERT INTO Books (LibID, Title, SeriesID, SeqNumber, UpdateDate, LibRate, Lang, Folder, FileName, InsideNo, Ext, BookSize, KeyWords, Search)
VALUES($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14)";
            _insertBooksCommand.Parameters.Add("$1", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$2", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$3", SqliteType.Integer);
            _insertBooksCommand.Parameters.Add("$4", SqliteType.Integer);
            _insertBooksCommand.Parameters.Add("$5", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$6", SqliteType.Integer);
            _insertBooksCommand.Parameters.Add("$7", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$8", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$9", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$10", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$11", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$12", SqliteType.Integer);
            _insertBooksCommand.Parameters.Add("$13", SqliteType.Text);
            _insertBooksCommand.Parameters.Add("$14", SqliteType.Text);
            _insertBooksCommand.Prepare();
        }

        public async Task InsertBookAsync(Book book, Series series)
        {
            if (book == null)
                throw new ArgumentException(nameof(book));

            if (book.Id != 0)
                throw new ArgumentException("Сущность уже была сохранена", nameof(book));

            if (series != null && series.Id == 0)
                await InsertSeriesAsync(series);

            _insertBooksCommand.Parameters["$1"].Value = book.LibId;
            _insertBooksCommand.Parameters["$2"].Value = book.Title;
            _insertBooksCommand.Parameters["$3"].Value = series != null ? (object)series.Id : DBNull.Value;
            _insertBooksCommand.Parameters["$4"].Value = (object)book.SeqNumber ?? DBNull.Value;
            _insertBooksCommand.Parameters["$5"].Value = book.UpdateDate;
            _insertBooksCommand.Parameters["$6"].Value = book.LibRate.HasValue ? (object)book.LibRate.Value : DBNull.Value;
            _insertBooksCommand.Parameters["$7"].Value = book.Lang;
            _insertBooksCommand.Parameters["$8"].Value = book.Folder;
            _insertBooksCommand.Parameters["$9"].Value = book.FileName;
            _insertBooksCommand.Parameters["$10"].Value = book.Index;
            _insertBooksCommand.Parameters["$11"].Value = book.Ext;
            _insertBooksCommand.Parameters["$12"].Value = book.BookSize;
            _insertBooksCommand.Parameters["$13"].Value = (object)book.KeyWords ?? DBNull.Value;
            _insertBooksCommand.Parameters["$14"].Value = book.Title.ToUpper();

            await _insertBooksCommand.ExecuteNonQueryAsync();
            book.Id = (int)SQLitePCL.raw.sqlite3_last_insert_rowid(_connection.Handle);
        }

        private async Task InsertSeriesAsync(Series series)
        {
            if (series == null)
                throw new ArgumentException(nameof(series));

            if (series.Id != 0)
                throw new ArgumentException("Сущность уже была сохранена", nameof(series));

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Series(SeriesTitle, Search) VALUES($1, $2)";
            command.CommandType = CommandType.Text;

            SqliteParameter titleParameter = command.CreateParameter();
            titleParameter.ParameterName = "$1";
            titleParameter.DbType = DbType.String;
            titleParameter.Value = series.Title;
            command.Parameters.Add(titleParameter);

            SqliteParameter searchParameter = command.CreateParameter();
            searchParameter.ParameterName = "$2";
            searchParameter.DbType = DbType.String;
            searchParameter.Value = series.Title.ToUpper();
            command.Parameters.Add(searchParameter);

            await command.ExecuteNonQueryAsync();
            series.Id = (int)SQLitePCL.raw.sqlite3_last_insert_rowid(_connection.Handle);
        }

        public async Task InsertBookAuthorsAsync(Book book, IEnumerable<Author> authors)
        {
            if (book == null)
                throw new ArgumentException(nameof(book));

            if (authors == null)
                throw new ArgumentException(nameof(authors));

            foreach (Author author in authors)
            {
                if (author.Id == 0)
                    await InsertAuthorAsync(author);

                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = "INSERT INTO Author_List (BookID, AuthorID) VALUES ($1, $2)";
                command.Parameters.Add("$1", SqliteType.Integer);
                command.Parameters["$1"].Value = book.Id;

                command.Parameters.Add("$2", SqliteType.Integer);
                command.Parameters["$2"].Value = author.Id;

                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task InsertAuthorAsync(Author author)
        {
            if (author == null)
                throw new ArgumentException(nameof(author));

            if (author.Id != 0)
                throw new ArgumentException("Сущность уже была сохранена", nameof(author));

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Authors (LastName, FirstName, MiddleName, Search) VALUES ($1, $2, $3, $4)";
            command.Parameters.Add("$1", SqliteType.Text);
            command.Parameters["$1"].Value = author.LastName;
            command.Parameters.Add("$2", SqliteType.Text);
            command.Parameters["$2"].Value = (object)author.FirstName ?? DBNull.Value;
            command.Parameters.Add("$3", SqliteType.Text);
            command.Parameters["$3"].Value = (object)author.MiddleName ?? DBNull.Value;
            command.Parameters.Add("$4", SqliteType.Text);
            command.Parameters["$4"].Value = author.ToString().ToUpper();

            await command.ExecuteNonQueryAsync();
            author.Id = (int)SQLitePCL.raw.sqlite3_last_insert_rowid(_connection.Handle);
        }

        public async Task InsertBookGenresAsync(Book book, IEnumerable<Genre> genres)
        {
            if (book == null)
                throw new ArgumentException(nameof(book));

            if (genres == null)
                throw new ArgumentException(nameof(genres));

            foreach (Genre genre in genres)
            {
                if (genre.Id == 0)
                    await InsertGenreAsync(genre);

                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = "INSERT INTO Genre_List (BookID, GenreID) VALUES ($1, $2)";
                command.Parameters.Add("$1", SqliteType.Integer);
                command.Parameters["$1"].Value = book.Id;

                command.Parameters.Add("$2", SqliteType.Integer);
                command.Parameters["$2"].Value = genre.Id;

                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task InsertGenreAsync(Genre genre)
        {
            if (genre == null)
                throw new ArgumentException(nameof(genre));

            if (genre.Id != 0)
                throw new ArgumentException("Сущность уже была сохранена", nameof(genre));

            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Genres (Fb2Code, GenreAlias, Search) VALUES ($1, $2, $3)";
            command.Parameters.Add("$1", SqliteType.Text);
            command.Parameters["$1"].Value = genre.Fb2Code;
            command.Parameters.Add("$2", SqliteType.Text);
            command.Parameters["$2"].Value = genre.Name;
            command.Parameters.Add("$3", SqliteType.Text);
            command.Parameters["$3"].Value = genre.Name.ToUpper();

            await command.ExecuteNonQueryAsync();
            genre.Id = (int)SQLitePCL.raw.sqlite3_last_insert_rowid(_connection.Handle);
        }
    }
}
