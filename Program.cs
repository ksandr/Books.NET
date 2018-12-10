using System;
using System.Collections.Generic;
using CommandLine;

namespace Ksandr.Books
{
    public partial class Program
    {
        [Verb("web", HelpText = "Starts Books.NET Web server console")]
        class WebOptions
        { }

        [Verb("import", HelpText = "Imports libaray archive .inpx file")]
        class ImportOptions
        { }

        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<WebOptions, ImportOptions>(args).MapResult(
                (WebOptions opts) => RunWebConsole(args),
                (ImportOptions opts) => RunImport(opts),
                errs => RunErrors(errs)
            );
        }

        static int RunErrors(IEnumerable<Error> errors)
        {
            Console.WriteLine("Invalid options");
            return 1;
        }
    }
}
