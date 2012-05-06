using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace OptionLib
{
    class ArgumentParser
    {
        private enum ParserExpectArgumentType
        {
            ExpectArgType_ANY,
            ExpectArgType_PARAMETER,
            ExpectArgType_OPTIONAL_PARAMETER,
        }

        private TextWriter output;
        public ArgumentParser(TextWriter output = null)
        {
            this.output = output;
            if (output == null)
            {
                this.output = System.Console.Out;
            }
        }

        private ParserExpectArgumentType nextArg = ParserExpectArgumentType.ExpectArgType_ANY;

        private List<ProgramOption> requiredOptionsList = new List<ProgramOption>();
        private SortedDictionary<string, ProgramOption> optionsDictionary = new SortedDictionary<string, ProgramOption>();
        private List<string> arguments = new List<string>();

        internal List<string> ParseCommandLine(List<ProgramOption> optionList, string[] args)
        {
            if (optionList == null)
            {
                optionList = new List<ProgramOption>();
            }
            ProcessOptions(optionList);
            
            for (int argsPosition = 0; argsPosition < args.Length; argsPosition++)
            {
                /* POZOR POZOR POZOR:
                 * field values k danym klicum mohou byt promenne majici zatim null hodnotu, takze je treba kontrolovat pomoci
                 * field.IsDefined()!!!
                 * 
                 * field.SetValue(options, parsedvalue);
                 */
                string arg = args[argsPosition];
                switch (nextArg)
                {
                    case ParserExpectArgumentType.ExpectArgType_ANY:
                        if (string.IsNullOrEmpty(arg))
                        {
                            arguments.Add(arg);
                        }
                        else if (arg[0] != '-')
                        {
                            arguments.Add(arg);
                        }
                        else if (arg == "-")
                        {
                            invalidOption("-");
                        }
                        else if (arg[1] != '-')
                        {
                            ExtractShortOptions(arg);
                        }
                        else
                        {
                            ExtractLongOptions(args, ref argsPosition);
                        }
                        break;
                    case ParserExpectArgumentType.ExpectArgType_PARAMETER:
                        break;
                    case ParserExpectArgumentType.ExpectArgType_OPTIONAL_PARAMETER:
                        if (arg == "--")
                        {
                            arguments.AddRange(args.Skip(argsPosition + 1));
                            argsPosition = args.Length;
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            log("\nPLAIN ARGUMENTS:");
            log(string.Join("\n", arguments));
            return new List<string>(arguments);
        }

        /* reset the options dictionary and refill it with new field information from programoptions options */
        private void ProcessOptions(List<ProgramOption> optionList)
        {
            optionsDictionary.Clear();

            log("POPULATING PARSER DICTIONARY:");

            //FieldInfo[] optionsFields = optionList.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            /* Process attributes of every field from users "ProgramOptions" (or whatever class derived from ProgramOptionsBase)
             * and store settings in dictionary. */
            //foreach(FieldInfo field in optionsFields)
            foreach(ProgramOption programOption in optionList)
            {
                AddNamesToDictionary(programOption);

                if (programOption.IsRequired())
                {
                    requiredOptionsList.Add(programOption);
                    string name;
                    if (programOption.LongNames.Count > 0)
                    {
                        name = programOption.LongNames[0];
                    }
                    /* musi byt alespon jedno kratke jmeno, jinak by AddNamesToDictionary vyhodila vyjimku */
                    else
                    {
                        name = programOption.ShortNames[0];
                    }
                    log("Option " + name + " is required.");
                }
            }
        }

        private void AddNamesToDictionary(ProgramOption programOption)
        {
            if (programOption.ShortNames.Count == 0 && programOption.LongNames.Count == 0)
            {
                throw new NotSupportedException("Option must have at least one name. Add ShortName and/or LongName attribute.");
            }

            foreach (string name in programOption.ShortNames)
            {
                optionsDictionary.Add(name, programOption);
                log("Adding option " + name + " to dictionary.");
            }

            foreach (string name in programOption.LongNames)
            {
                optionsDictionary.Add(name, programOption);
                log("Adding option " + name + " to dictionary.");
            }
        }

        private void ExtractShortOptions(string arg)
        {
        }

        private void ExtractLongOptions(string[] args, ref int argsPosition)
        {
            string arg = args[argsPosition];
            if (arg == "--")
            {
                this.arguments.AddRange(args.Skip(argsPosition + 1));
                argsPosition = args.Length;
            }
        }

        [Conditional("DEBUG")]
        private void log(string msg)
        {
            output.WriteLine(msg);
        }

        private void invalidOption(string option)
        {
            //AssemblyTitleAttribute assemblyTitleAttr = (AssemblyTitleAttribute) Attribute.GetCustomAttribute(programOptions.GetType().Assembly, typeof(AssemblyTitleAttribute));
            //output.WriteLine(assemblyTitleAttr.Title + ": invalid option - " + option);
            output.WriteLine("invalid option - " + option);
        }
    }

}