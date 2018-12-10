using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ksandr.Books.Import;
using Ksandr.Books.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ksandr.Books
{
    public partial class Program
    {
        static int RunImport(ImportOptions opts)
        {
            string environmentName = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            ServiceProvider serviceProvider = new ServiceCollection()
                .UseSerilog(configuration => configuration.ReadFrom.Configuration(config))
                .UseBooksContext(config)
                .AddTransient<ImportService>()
                .BuildServiceProvider();

            ImportService importService = serviceProvider.GetRequiredService<ImportService>();

            string genresFile = config.GetSection("AppConfig:GenresFile").Get<string>();
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

            string[] languages = config.GetSection("AppConfig:Languages").Get<string[]>() ?? new string[] { };

            Task task = importService.StartAsync(opts.InpxFile, genresFile, languages, cancelTokenSource.Token);
            task.Wait();

#if DEBUG
            Console.WriteLine("Press <Enter> to exit...");
            Console.ReadLine();
#endif
            return 0;
        }
    }
}
