using OptionLib;

namespace UseCases
{
    class TimeOptions : ProgramOptionsBase
    {
        [Option(Description = "(Used together with -o.) Do not overwrite but append.")]
        [ShortName("a")]
        [LongName("append")]
        public bool append = false;

        [Option(Description = "Use the portable output format.")]
        [ShortName("p")]
        [LongName("portability")]
        public bool portability = false;

        [OptionWithParameter(Description = "Specify output format, possibly overriding the format specified in the environment variable TIME.", ParameterName = "FORMAT")]
        [ShortName("f")]
        [LongName("format")]
        public string format = null;

        [OptionWithParameter(Description = "Do not send the results to stderr, but overwrite the specified file.", ParameterName = "FILE")]
        [ShortName("o")]
        [LongName("output")]
        public string outputFile = null;

        [Option(Description = "Give very verbose output about all the program knows about.")]
        [ShortName("v")]
        [LongName("verbose")]
        public bool verbose = false;
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
