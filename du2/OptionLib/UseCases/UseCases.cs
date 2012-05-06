using OptionLib;
using OptionLib.Other;

namespace UseCases
{
    class TimeOptions : ProgramOptionsBase
    {
        [Option("(Used together with -o.) Do not overwrite but append.")]
        [ShortName("a")]
        [LongName("append")]
        public bool append = false;

        [Option("Use the portable output format.")]
        [ShortName("p")]
        [LongName("portability")]
        public bool portability = false;

        [OptionWithParameter("Specify output format, possibly overriding the format specified in the environment variable TIME.", "FORMAT", true)]
        [ShortName("f")]
        [LongName("format")]
        public string format = null;

        [OptionWithParameter("Do not send the results to stderr, but overwrite the specified file.", "FILE", false)]
        [ShortName("o")]
        [LongName("output")]
        public string outputFile = null;

        [Option("Give very verbose output about all the program knows about.")]
        [ShortName("v")]
        [LongName("verbose")]
        public bool verbose = false;

        //example of multiple names for option
        //[Option(new string[] { "c", "m", "s" }, new List<string> { "copy", "move", "send" }, "Blabla description")]
        [ShortName("c")]
        [LongName("copy")]
        public bool copy = false;
    }

    class UseCases
    {
        static void Main()
        {
            time();
        }

        private static void time()
        {
            TimeOptions options = new TimeOptions();
            string[] timeArgs = new string[] { "-v", "-o", "/path/to/file", "-a", "--", "--some--", "useless", "noise" };
            options.Initialize(timeArgs);
            /* TimeOptions processing to be added */
        }
    }
}
