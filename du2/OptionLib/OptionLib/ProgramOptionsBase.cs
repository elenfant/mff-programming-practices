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
            foreach (ProgramOption option in optionList) {
                Console.WriteLine(option.PrintHelp());
            }
        }

        /// <summary>
        /// Initializes the program options, proccesses the command line arguments and sets option values.
        /// </summary>
        /// <param name="args">Input from command line.</param>
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
            arguments.Concat(argParser.ParseCommandLine(optionList, args));
        }
    }
}
