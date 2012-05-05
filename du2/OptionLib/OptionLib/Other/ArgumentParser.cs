using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace OptionLib.Other
{
    class ArgumentParser
    {
        private enum ParserExpectArgumentType
        {
            ExpectArgType_ANY,
            ExpectArgType_OPTION,
            ExpectArgType_PARAMETER,
            ExpectArgType_OPTIONAL_PARAMETER,
            ExpectArgType_ARGUMENT
        }
        private ParserExpectArgumentType nextArg = ParserExpectArgumentType.ExpectArgType_ANY;
        private SortedDictionary<string, FieldInfo> optionsDictionary = new SortedDictionary<string, FieldInfo>();

        public ArgumentParser()
        {
        }

        internal List<string> ProcessCommandLine(ProgramOptionsBase options, string[] args)
        {
            ProcessOptions(options);
            
            List<string> arguments = new List<string>();
            for (int argPosition = 0; argPosition < args.Length; argPosition++)
            {
                /* POZOR POZOR POZOR:
                 * field values k danym klicum mohou byt promenne majici zatim null hodnotu, takze je treba kontrolovat pomoci
                 * field.IsDefined()!!!
                 */
                switch (nextArg)
                {
                    case ParserExpectArgumentType.ExpectArgType_ANY:
                    case ParserExpectArgumentType.ExpectArgType_OPTION:
                    case ParserExpectArgumentType.ExpectArgType_PARAMETER:
                    case ParserExpectArgumentType.ExpectArgType_OPTIONAL_PARAMETER:
                        //field.SetValue(options, parsedvalue);
                        break;
                    case ParserExpectArgumentType.ExpectArgType_ARGUMENT:
                        arguments.AddRange(args.Skip(argPosition + 1));
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return arguments;
        }

        /* reset the options dictionary and refill it with new field information from programoptions options */
        private void ProcessOptions(ProgramOptionsBase options)
        {
            optionsDictionary.Clear();

            FieldInfo[] optionsFields = options.GetType().GetFields();
            /* Process attributes of every field from users "ProgramOptions" (or whatever class derived from ProgramOptionsBase)
             * and store settings in dictionary. Skip parser and arguments fields". */
            foreach(FieldInfo field in optionsFields)
            {
                if (field.Name == ProgramOptionsBase.parserVariableName || field.Name == ProgramOptionsBase.argumentsVariableName)
                {
                    continue;
                }

                ShortNameAttribute shortName = (ShortNameAttribute)Attribute.GetCustomAttribute(field, typeof(ShortNameAttribute));
                System.Console.WriteLine("Adding " + shortName.ShortName + " to dictionary.");
                optionsDictionary.Add(shortName.ShortName, field);

                LongNameAttribute longName = (LongNameAttribute)Attribute.GetCustomAttribute(field, typeof(LongNameAttribute));
                System.Console.WriteLine("Adding " + longName.LongName + " to dictionary.");
                optionsDictionary.Add(longName.LongName, field);
            }
        }

    }

}