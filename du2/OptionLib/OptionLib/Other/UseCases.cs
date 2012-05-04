using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptionLib.Other;

namespace OptionLib
{
    class ProgramOptions : ProgramOptionsBase
    {
        [Option("a", "append", "(Used together with -o.) Do not overwrite but append.")]
        public bool append = false;

        [Option("p", "portability", "Use the portable output format.")]
        public bool portability = false;

        [OptionWithParameter("f", "format", "Specify output format, possibly overriding the format specified in the environment variable TIME.")]
        public string format = null;

        [OptionWithParameter("o", "output", "Do not send the results to stderr, but overwrite the specified file.", "FILE")]
        public string outputFile = null;

        [Option("v", "verbose", "Give very verbose output about all the program knows about.")]
        public bool verbose = false;


        //example of multiple names for option
        [Option(new string[] { "c", "m", "s" }, new List<string> { "copy", "move", "send" }, "Blabla description")]
        public bool copy = false;

        internal void Initialize(int argc, string[] args)
        {
            throw new NotImplementedException();
        }
    }

    class UseCases
    {
        public void Main(int argc, string[] args)
        {
            ProgramOptions options = new ProgramOptions();
            options.Initialize(argc, args);
        }
    }
}
