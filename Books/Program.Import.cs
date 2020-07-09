﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ksandr.Books.Import;
using Ksandr.Books.Import.Readers;
using Ksandr.Books.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

using SqliteDatabase = Ksandr.Books.Import.Sqlite.Database;

namespace Ksandr.Books
{
    public partial class Program
    {
        static int RunImport2(ImportOptions opts)
        {
            if (!Confirm(opts.Force))
                return -1;

            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            ServiceProvider serviceProvider = new ServiceCollection()
                .UseSerilog(configuration => configuration.ReadFrom.Configuration(config))
                .BuildServiceProvider();

            string genresFile = config.GetSection("AppConfig:GenresFile").Get<string>();
            GenresReader genresReader = new GenresReader(genresFile) { Fb2Only = true };

            string[] languages = config.GetSection("AppConfig:Languages").Get<string[]>() ?? new string[] { };
            BooksReader booksReader = new BooksReader(opts.InpxFile) { Languages = languages, SkipDeleted = true };

            string databaseFile = config.GetSection("AppConfig:DatabaseFile").Get<string>();
            string connectionString = $"Data Source=file:{Path.GetFullPath(databaseFile)}?_journal_mode=WAL";
            IDatabase db = new SqliteDatabase(connectionString);

            ILogger<LibraryConverter> logger = serviceProvider.GetRequiredService<ILogger<LibraryConverter>>();
            LibraryConverter converter = new LibraryConverter(genresReader, booksReader, logger);

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            Task task = converter.ConvertAsync(db, cancelTokenSource.Token);
            task.Wait();

#if DEBUG
            Console.Write("Press <Enter> to exit...");
            Console.ReadLine();
#endif
            return 0;
        }

        static bool Confirm(bool force)
        {
            if (force)
            {
                Print(ConsoleColor.Yellow,
                    "Force database recreation.\nHope you know what you are doing...\n");
                return true;
            }

            Print(ConsoleColor.Red, "Database will be recreated and all data will be deleted.\nContinue? [y/N]: ");
            string answer = Console.ReadLine();

            return string.Equals(answer, "y", StringComparison.InvariantCultureIgnoreCase);
        }

        static void Print(ConsoleColor color, string message)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.Write(message);

            Console.ForegroundColor = oldColor;
        }
    }
}