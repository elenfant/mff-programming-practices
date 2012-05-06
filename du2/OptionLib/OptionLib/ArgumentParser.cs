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
        private enum ArgumentType
        {
            ANY,

            OPTION,
            OPTION_SHORT,
            OPTION_LONG,

            ARGUMENT,
            ARGUMENT_PARAMETER,
            ARGUMENT_PARAMETER_OPTIONAL,
            ARGUMENT_DELIMITER
        }

        private TextWriter output;
        private ProgramOptionsBase programOptions;
        public ArgumentParser(ProgramOptionsBase programOptions, TextWriter output = null)
        {
            this.programOptions = programOptions;
            this.output = output;
            if (output == null)
            {
                this.output = System.Console.Out;
            }
        }

        private Dictionary<string, ProgramOption> optionsDictionary = new Dictionary<string, ProgramOption>();
        private ProgramOption prevOption = null;
        private ArgumentType expectedArgType = ArgumentType.ANY;

        internal List<string> ParseCommandLine(List<ProgramOption> optionList, string[] args)
        {
            /* initialize for new parsing */
            expectedArgType = ArgumentType.ANY;
            ProcessOptions(optionList);            
            List<string> arguments = new List<string>();
            
            for (int argsPosition = 0; argsPosition < args.Length; argsPosition++)
            {
                string arg = args[argsPosition];
                ArgumentType currentArgType = GetArgumentType(arg);
                switch (currentArgType)
                {
                    case ArgumentType.ARGUMENT:
                        ParseArgument(arg, arguments);
                        break;

                    case ArgumentType.OPTION_SHORT:
                        ParseShortOption(arg);
                        break;

                    case ArgumentType.OPTION_LONG:
                        bool continueParsing = ParseLongOption(arg);
                        if (!continueParsing)
                        {
                            arguments.AddRange(args.Skip(argsPosition + 1));
                            argsPosition = args.Length;
                        }
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
            Log("\nPLAIN ARGUMENTS:");
            Log(string.Join("\n", arguments));
            return arguments;// as they are local variable for now (and not private parser field), we dont need to: new List<string>(arguments);
        }

        /* reset the options dictionary and refill it with new field information from programoptions options */
        private void ProcessOptions(List<ProgramOption> optionList)
        {
            optionsDictionary.Clear();
            Log("POPULATING PARSER DICTIONARY:");
            foreach(ProgramOption programOption in optionList)
            {
                AddNamesToDictionary(programOption);
            }
        }

        private void AddNamesToDictionary(ProgramOption programOption)
        {
            foreach (string name in programOption.ShortNames)
            {
                optionsDictionary.Add(name, programOption);
                Log("Adding option " + name + " to dictionary.");
            }

            foreach (string name in programOption.LongNames)
            {
                optionsDictionary.Add(name, programOption);
                Log("Adding option " + name + " to dictionary.");
            }
        }

        private ArgumentType GetArgumentType(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return ArgumentType.ARGUMENT;
            }
            if (arg[0] != '-')
            {
                return ArgumentType.ARGUMENT;
            }
            /* now arg starts with '-' */
            if (arg == "-")
            {
                return ArgumentType.ARGUMENT;
            }
            if (arg[1] != '-')
            {
                return ArgumentType.OPTION_SHORT;
            }
            return ArgumentType.OPTION_LONG;
        }

        private void ParseShortOption(string arg)
        {
            ProgramOption option = null;
            switch (expectedArgType)
            {
                case ArgumentType.ARGUMENT_PARAMETER_OPTIONAL:
                    prevOption.SetValueToDefault(programOptions);
                    goto case ArgumentType.ANY;

                case ArgumentType.ANY:
                    try
                    {
                        option = optionsDictionary[arg.Substring(1)];
                    }
                    catch (KeyNotFoundException)
                    {
                        LogInvalidArgument(arg);
                        throw new NotSupportedException("Invalid option -- " + arg.Substring(1));
                    }

                    /* prepare for parameters? */
                    if (option.GetOptionAttributeType() == typeof(OptionAttribute))
                    {
                        option.SetValue("true", programOptions);
                        expectedArgType = ArgumentType.ANY;
                    }
                    else if (option.GetOptionAttributeType() == typeof(OptionWithOptionableParameterAttribute))
                    {
                        expectedArgType = ArgumentType.ARGUMENT_PARAMETER_OPTIONAL;
                    }
                    else if (option.GetOptionAttributeType() == typeof(OptionWithParameterAttribute))
                    {
                        expectedArgType = ArgumentType.ARGUMENT_PARAMETER;
                    }
                    break;
                
                /* required option parameter, but short option found => exception */
                case ArgumentType.ARGUMENT_PARAMETER:
                    throw new NotSupportedException("Option found, required parameter expected.");

                default:
                    throw new NotImplementedException("Unknown ArgumentParser.ArgumentType!");
            }
            prevOption = option;
            return;
        }

        private bool ParseLongOption(string arg)
        {
            ProgramOption option = null;
            switch (expectedArgType)
            {
                case ArgumentType.ARGUMENT_PARAMETER_OPTIONAL:
                    prevOption.SetValueToDefault(programOptions);
                    goto case ArgumentType.ANY;

                case ArgumentType.ANY:
                    if (arg == "--")
                    {
                        /* finish parsing options and parameters */
                        return false;
                    }
                    int eqPosition = arg.LastIndexOf('=');
                    try
                    {
                        if (eqPosition == -1)
                        {
                            option = optionsDictionary[arg.Substring(2)];
                        }
                        else
                        {
                            option = optionsDictionary[arg.Substring(2, eqPosition - 2)];
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        LogInvalidArgument(arg);
                        throw new NotSupportedException("Invalid option -- " + arg.Substring(2));
                    }

                    /* prepare for parameters? */
                    if (option.GetType() == typeof(OptionAttribute))
                    {
                        if (eqPosition != -1)
                        {
                            throw new NotSupportedException("Option parameter for option " + option.Name + " disallowed.");
                        }
                        option.SetValue("true", programOptions);
                    }
                    else if (option.GetType() == typeof(OptionWithOptionableParameterAttribute))
                    {
                        if (eqPosition != -1)
                        {
                            option.SetValueToDefault(programOptions);
                        }
                        else
                        {
                            option.SetValue(arg.Substring(eqPosition + 1), programOptions);
                        }
                    }
                    else if (option.GetType() == typeof(OptionWithParameterAttribute))
                    {
                        if (eqPosition != -1)
                        {
                            throw new NotSupportedException("Required parameter for option " + option.Name + " is missing.");
                        }
                        else
                        {
                            option.SetValue(arg.Substring(eqPosition + 1), programOptions);
                        }
                    }
                    break;

                /* required option parameter, but short option found => exception */
                case ArgumentType.ARGUMENT_PARAMETER:
                    throw new NotSupportedException("Option found, required parameter expected.");

                default:
                    throw new NotImplementedException("Unknown ArgumentParser.ArgumentType!");
            }
            expectedArgType = ArgumentType.ANY;
            prevOption = option;
            return true;
        }

        private void ParseArgument(string arg, List<string> arguments)
        {
            switch (expectedArgType)
            {
                case ArgumentType.ARGUMENT_PARAMETER:
                    /* fall through */
                case ArgumentType.ARGUMENT_PARAMETER_OPTIONAL:
                    Debug.Assert(prevOption != null, "prevOption parameter is null");
                    prevOption.SetValue(arg, programOptions);
                    break;

                case ArgumentType.ARGUMENT:
                    /* fall through */
                case ArgumentType.ANY:
                    arguments.Add(arg);
                    break;
                    
                default:
                    throw new NotImplementedException("Can't expect any option!");
            }
            expectedArgType = ArgumentType.ANY;
        }

        [Conditional("DEBUG")]
        private void Log(string msg)
        {
            output.WriteLine(msg);
        }

        private void LogInvalidArgument(string option)
        {
            //AssemblyTitleAttribute assemblyTitleAttr = (AssemblyTitleAttribute) Attribute.GetCustomAttribute(programOptions.GetType().Assembly, typeof(AssemblyTitleAttribute));
            //output.WriteLine(assemblyTitleAttr.Title + ": invalid option - " + option);
            output.WriteLine("invalid option - " + option);
        }
    }

}