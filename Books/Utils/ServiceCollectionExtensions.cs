using System;
using System.IO;
using Ksandr.Books.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Hosting;

namespace Ksandr.Books.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseSerilog(this IServiceCollection collection,
            Action<LoggerConfiguration> configureLogger, bool preserveStaticLogger = false)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (configureLogger == null)
                throw new ArgumentNullException(nameof(configureLogger));

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration();
            configureLogger(loggerConfiguration);
            var logger = loggerConfiguration.CreateLogger();

            if (preserveStaticLogger)
            {
                collection.AddLogging(builder =>
                {
                    builder.AddSerilog(logger, true);
                });
            }
            else
            {
                // Passing a `null` logger to `SerilogLoggerFactory` results in disposal via
                // `Log.CloseAndFlush()`, which additionally replaces the static logger with a no-op.
                Log.Logger = logger;
                collection.AddLogging(builder =>
                {
                    builder.AddSerilog(null, true);
                });
            }

            return collection;
        }

        public static IServiceCollection AddBooksContext(this IServiceCollection collection, IConfiguration config)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            string databaseFile = config.GetSection("AppConfig:DatabaseFile").Get<string>();
            string connectionString = $"Data Source=file:{Path.GetFullPath(databaseFile)}?_journal_mode=WAL";
            collection.AddDbContext<BooksContext>(builder => builder.UseSqlite(connectionString));

            return collection;
        }
    }
}
