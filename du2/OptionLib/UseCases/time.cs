using OptionLib;
using System.Reflection;
using System;

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

        protected override string GetProgramHelpText() {
            return "time [options] command [arguments...]";
        }

        protected override string GetVersionInformation() {
            AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
            Version version = assemblyName.Version;
            return String.Format("AssemblyName: {0}, Version: {1}", assemblyName.Name, version.ToString());
        }
    }

    class time
    {
        static void Main() {
            RunTime();
        }


        private const string PORTABLE_FORMAT = "real %f\nuser %f\nsys %f\n";

        private static void RunTime() {
            TimeOptions options = new TimeOptions();
            string[] timeArgs = new string[] { "-v", "-o", "/path/to/file", "-a", "--", "--some--", "useless", "noise" };
            options.Initialize(timeArgs);
            /* TimeOptions processing to be added */

            string format = options.format;
            if (options.portability) {
                if (options.verbose) {
                    Console.WriteLine("Setting portable format.");
                }
                format = PORTABLE_FORMAT;
            }

            System.IO.StreamWriter writer = null;
            if (options.outputFile != null) {
                var fileStream = new System.IO.FileStream(options.outputFile, options.append ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                writer = new System.IO.StreamWriter(fileStream);
                if (options.verbose) {
                    Console.WriteLine("Output will be redirected to file.");
                }
            }
            else if (options.verbose) {
                Console.WriteLine("Output will be printed to standart output.");
            }

            DoCoreTask(options.Arguments, writer, format);

            if (writer != null) {
                writer.Flush();
                writer.Close();
            }
        }

        private static bool DoCoreTask(System.Collections.Generic.List<string> list, System.IO.StreamWriter outputStream = null, string format = null) {
            if (list == null || list.Count == 0) {
                return false;
            }
            string commandName = list[0];
            list.Remove(commandName);

            return RunAndTimeProgram(commandName, list, outputStream, format);

        }

        private static bool RunAndTimeProgram(string commandName, System.Collections.Generic.List<string> list, System.IO.StreamWriter outputStream = null, string format = null) {

            throw new NotImplementedException();
        }
    }
}
