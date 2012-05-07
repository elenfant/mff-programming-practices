using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace OptionLib
{
    /// <summary>
    /// Base class to be derived in program, contains program options definitions. 
    /// </summary>
    public abstract class ProgramOptionsBase
    {
        private List<ProgramOption> optionList;
        private List<string> arguments = new List<string>();

        /// <summary>
        /// List of common arguments, not options.
        /// </summary>
        public List<string> Arguments {
            get {
                return arguments;
            }
        }

        /// <summary>
        /// Prints program help and description of all defined options
        /// </summary>
        public void PrintHelp() {
            Console.WriteLine(GetProgramHelpText());
            foreach (ProgramOption option in optionList) {
                Console.WriteLine(option.PrintHelp());
            }
            Console.WriteLine(TerminateOptionListText);
        }

        /// <summary>
        /// Initializes the program options, proccesses the command line arguments and sets option values.
        /// </summary>
        /// <param name="args">Input from command line.</param>
        /// OptionInvalidException
        /// OptionsClashException
        /// OptionParameterDisalowedException
        /// RequiredParameterMissingException
        public void Initialize(string[] args) {

            optionList = new List<ProgramOption>();

            var fieldInfos = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var fieldInfo in fieldInfos) {
                Attribute optionAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(OptionBase));
                if (optionAttribute == null) {
                    continue;
                }
                ShortNameAttribute shortNameAttribute = (ShortNameAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ShortNameAttribute));
                LongNameAttribute longNameAttribute = (LongNameAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(LongNameAttribute));
                ProgramOption option = new ProgramOption(fieldInfo, (OptionBase)optionAttribute, shortNameAttribute, longNameAttribute);
                optionList.Add(option);
            }

            ArgumentParser argParser = new ArgumentParser(this);
            arguments.AddRange(argParser.ParseCommandLine(optionList, args));
            CheckRequiredOptions();
        }

        [Option(Description = "Print a usage message on standard output and exit successfully.")]
        [LongName("help")]
        public bool help;

        [Option(Description = "Print version information on standard output, then exit successfully.")]
        [ShortName("V"), LongName("version")]
        public bool version;

        private void CheckRequiredOptions() {
            foreach (var option in optionList) {
                if (option.IsRequired() && !option.IsPresent) {
                    throw new RequiredOptionMissingException(option.Name);
                }
            }
        }

        protected abstract string GetProgramHelpText();

        public void PrintVersion() {
            Console.WriteLine(GetVersionInformation());
        }

        protected abstract string GetVersionInformation();

        private const string TerminateOptionListText = Printer.FIRST_LEVEL_INDENT + "--\n" + Printer.SECOND_LEVEL_INDENT + "Terminate option list.";
    }
}
