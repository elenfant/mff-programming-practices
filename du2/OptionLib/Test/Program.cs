using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptionLib;
using Test;
using System.Reflection;

namespace Test
{
    //[FormatedHelpText("Print a usage message on standard output and exit successfully.")]
    class ProgramOptions : ProgramOptionsBase
    {
        [Option(Description = "(Used together with -o.) Do not overwrite but append.")]
        [ShortName("a", "b", "c")]
        [LongName("append", "blend", "create")]
        public bool append = false;

        [Option(Description = "Use the portable output format.")]
        [ShortName("p"), LongName("portable")]
        public bool portability = false;

        [OptionWithParameter(Description = "Specify output format, possibly overriding the format specified in the environment variable TIME.", ParameterName = "FORMAT")]
        //[ShortName("f")]
        [LongName("format")]
        public string format = null;

        [OptionWithParameter(Description = "Do not send the results to stderr, but overwrite the specified file.", ParameterName = "FILE")]
        [ShortName("o"), LongName("output")]
        public string outputFile = null;

        [Option(Description = "Give very verbose output about all the program knows about.")]
        [ShortName("v")]
        //[LongName("verbose")]
        public bool verbose = false;

        public override string ProgramHelpText {
            get {
                return "time [options] command [arguments...]";
            }
        }

        public override string GetVersionInformation() {
            AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
            Version version = assemblyName.Version;
            return String.Format("AssemblyName: {0}, Version: {1}", assemblyName.Name, version.ToString());
        }
        

        //[FormatedHelpText("Print a usage message on standard output and exit successfully.")]

        //example of multiple names for option
        //[Option(description: "Blabla description", ShortNames = new string[] { "c", "m", "s" }, LongNames = new List<string> {"copy", "move", "send"})]
        //public bool copy = false;
    }


    class Program
    {
        static void Main(string[] args) {
            ProgramOptions options = new ProgramOptions();
            options.Initialize(args);
            options.PrintHelp();
            options.PrintVersion();
        }
    }
}
