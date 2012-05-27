using System;
using System.Collections.Generic;
using CommandLine;

namespace TestUseCases
{

    class Time
    {
        public Time() : this(new string[] {}) { }

        public Time(string[] args)
        {
            CommandLineParser parser = new CommandLineParser();

            var format = new CommandLineStringOption("format", "f");
            format.Help = "Specify output format, possibly overriding the format specified in the environment variable TIME.";
            format.ExpectedValue = "format";
            //parser.AddOption(format);

            var portability = new CommandLineBoolOption("portability", "p");
            portability.Help = "Use the portable output format.";
            //parser.AddOption(portability);

            var output = new CommandLineStringOption("output", "o");
            output.Help = "Do not send the results to stderr, but overwrite the specified file.";
            output.ExpectedValue = "file";
            output.Required = true;
            parser.AddOption(output);

            var append = new CommandLineBoolOption("append", "a");
            append.Help = "(Used together with -o.) Do not overwrite but append.";
            //parser.AddOption(append);

            var verbose = new CommandLineBoolOption("verbose", "v");
            verbose.Help = "Give very verbose output about all the program knows about.";
            verbose.Required = true;
            //verbose.ParameterType = ParameterType.Required;
            parser.AddOption(verbose);

            var help = new CommandLineBoolOption("help");
            help.Help = "Print a usage message on standard output and exit successfully.";
            //parser.AddOption(help);

            var version = new CommandLineBoolOption("version", "V");
            version.Help = "Print version information on standard output, then exit successfully.";
            //parser.AddOption(version);

            List<string> extraParameters;

            try
            {
                extraParameters = parser.Parse(args);
            }
            catch (ParsingException ex)
            {
                if (ex.Option != null)
                {
                    Console.WriteLine("An error occurred in parameter " + ex.Option.Name);
                }
                Console.WriteLine("Message: " + ex.Message);
                return;
            }

            if ((args.Length == 0) || (help.Present))
            {
                parser.PrintHelp();
                return;
            }

            Console.WriteLine("Format: " + format.Value);
            Console.WriteLine("Verbose: " + verbose.Present);
            Console.WriteLine("Output: " + output.Value);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Time timeUseCase = new Time(new string[] {"-o", "path_to_file"});
        }
    }
}
