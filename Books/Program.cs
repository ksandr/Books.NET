using System;
using System.Collections.Generic;
using CommandLine;

namespace Ksandr.Books
{
    public partial class Program
    {
        [Verb("web", HelpText = "Start Books.NET Web server.")]
        class WebOptions
        { }

        [Verb("import", HelpText = "Import libaray archive from .inpx file.")]
        class ImportOptions
        {
            [Option('i', "inpx", Required = true, HelpText = "Path to .inpx file")]
            public string InpxFile { get; set; }

            [Option('f', "force", Required = false, Default = false, HelpText = "Force database recreation without confirmation")]
            public bool Force { get; set; }
        }

        [Verb("import2", HelpText = "New Import libaray archive from .inpx file.")]
        class ImportOptions2
        {
            [Option('i', "inpx", Required = true, HelpText = "Path to .inpx file")]
            public string InpxFile { get; set; }

            [Option('f', "force", Required = false, Default = false, HelpText = "Force database recreation without confirmation")]
            public bool Force { get; set; }
        }

        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<WebOptions, ImportOptions, ImportOptions2>(args).MapResult(
                (WebOptions opts) => RunWebConsole(args),
                (ImportOptions opts) => RunImport(opts),
                (ImportOptions2 opts) => RunImport2(opts),
                errs => RunErrors(errs)
            );
        }

        static int RunErrors(IEnumerable<Error> errors)
        {
            return 1;
        }
    }
}
