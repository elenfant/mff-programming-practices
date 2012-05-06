using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace OptionLib.Other
{
    class ArgumentParser
    {
        private enum ParserExpectArgumentType
        {
            ExpectArgType_ANY,
            ExpectArgType_PARAMETER,
            ExpectArgType_OPTIONAL_PARAMETER,
        }

        public ArgumentParser(TextWriter output = null)
        {
            this.output = output;
            if (output == null)
            {
                this.output = System.Console.Out;
            }
        }

        private TextWriter output;
        private ParserExpectArgumentType nextArg = ParserExpectArgumentType.ExpectArgType_ANY;
        
        private SortedDictionary<string, FieldInfo> optionsDictionary = new SortedDictionary<string, FieldInfo>();

        // TODO move arguments as local variable inside the ProcessCommandLine method
        private List<string> arguments = new List<string>();

        private ProgramOptionsBase programOptions = null;

        internal List<string> ProcessCommandLine(ProgramOptionsBase options, string[] args)
        {
            if (options == null)
            {
                programOptions = new ProgramOptionsBase();
            }
            else
            {
                programOptions = options;
            }
            ProcessOptions();
            
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
        private void ProcessOptions()
        {
            optionsDictionary.Clear();

            log("POPULATING PARSER DICTIONARY:");

            FieldInfo[] optionsFields = programOptions.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            /* Process attributes of every field from users "ProgramOptions" (or whatever class derived from ProgramOptionsBase)
             * and store settings in dictionary. */
            foreach(FieldInfo field in optionsFields)
            {
                // TODO predelat na seznamy (kratkych i dlouhych) jmen u atributu, pridavat dvojici (string, field), kde field bude sdilene
                // jsou shortname a longname povinne? nebude stacit mit alespon jeden z nich?
                // v tom pripade je treba kontrolovat na shortName != null apod pro longName
                ShortNameAttribute shortName = (ShortNameAttribute)Attribute.GetCustomAttribute(field, typeof(ShortNameAttribute));
                optionsDictionary.Add(shortName.ShortName, field);
                log("Adding option " + shortName.ShortName + " to dictionary.");

                LongNameAttribute longName = (LongNameAttribute)Attribute.GetCustomAttribute(field, typeof(LongNameAttribute));
                optionsDictionary.Add(longName.LongName, field);
                log("Adding option " + longName.LongName + " to dictionary.");

                // TODO upravit podle toho, ve kterem atributu bude required, ale u jinych voleb to nema moc smysl
                OptionWithParameterAttribute option = (OptionWithParameterAttribute)Attribute.GetCustomAttribute(field, typeof(OptionWithParameterAttribute), true);
                if (option != null && option.isRequired())
                {
                    programOptions.AddRequiredOption(option);
                    log("Option " + option.ParameterName + " is required.");
                }
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
            AssemblyTitleAttribute assemblyTitleAttr = (AssemblyTitleAttribute) Attribute.GetCustomAttribute(programOptions.GetType().Assembly, typeof(AssemblyTitleAttribute));
            output.WriteLine(assemblyTitleAttr.Title + ": invalid option - " + option);
        }
    }

}