using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommandLine
{
    /// <summary>
    /// A class representing a command line parser.
    /// </summary>
    public class CommandLineParser
    {
        private List<CommandLineOption> Options = new List<CommandLineOption>();

        /// <summary>
        /// Parses the command line.
        /// </summary>
        /// <param name="args">Command line.</param>
        /// <returns>The rest of the command line, that is not options.</returns>
        public List<string> Parse(string[] args)
        {
            try
            {
                List<string> extraParameters = ParseInternal(args);

                foreach (var option in Options)
                {
                    if (option.Required && !option.Present)
                    {
                        throw new ParsingException("A required option is not present on the command line.", option);
                    }
                }

                return extraParameters;
            }
            catch (ParsingException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ParsingException("An exception occurred while parsing the command line.", e);
            }
        }

        private List<string> ParseInternal(string[] args)
        {
            List<string> extraParameters = new List<string>();

            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                if (arg.Length < 2) continue;

                bool isShortOption = (arg[0] == '-') && (arg[1] != '-');
                bool isLongOption = (arg[0] == '-') && (arg[1] == '-');
                bool isSeparator = (arg.Length == 2) && isLongOption;

                if (isSeparator)
                {
                    i++;
                    break;
                }
                else if (isLongOption)
                {
                    i += ParseLongOption(args, i);
                }
                else if (isShortOption)
                {
                    i +=  ParseShortOptions(args, i);
                }
                else
                {
                    extraParameters.Add(arg);
                    i++;
                }
            }

            if (i != args.Length)
                extraParameters.AddRange(args.Skip(i));

            return extraParameters;
        }

        private int ParseShortOptions(string[] args, int position)
        {
            string argument = args[position].Substring(1);

            while (argument != "")
            {
                string letter = argument.Substring(0, 1);
                string restOfArgument = argument.Substring(1);
                CommandLineOption option = GetOptionByShortName(letter);

                option.Present = true;

                if (option.ParameterType == ParameterType.Required)
                {
                    if (restOfArgument != "")
                    {
                        option.SetParameter(restOfArgument);
                        return 1;
                    }

                    if (args.Length <= position + 1)
                    {
                        throw new ParsingException("Parameter required, but no parameter present on the command line.", option);
                    }

                    option.SetParameter(args[position + 1]);
                    return 2;
                }
                else if (option.ParameterType == ParameterType.Optional)
                {
                    if (restOfArgument != "")
                    {
                        option.SetParameter(restOfArgument);
                        return 1;
                    }

                    if (args.Length <= position + 1)
                    {
                        return 1;
                    }

                    option.SetParameter(args[position + 1]);
                    return 2;
                }
                else if (option.ParameterType == ParameterType.None)
                {
                    argument = restOfArgument;
                }
                else
                {
                    throw new InvalidOperationException(); // cannot occur
                }
            }

            return 1;
        }

        private int ParseLongOption(string[] args, int position)
        {
            // option is something like "hello=world"
            string firstArgument = args[position].Substring(2);
            bool equalSignPresent = (firstArgument.IndexOf('=') != -1);

            string optionName;
            string parameterValue = null;

            if (equalSignPresent)
            {
                optionName = firstArgument.Substring(0, firstArgument.IndexOf('='));
                parameterValue = firstArgument.Substring(firstArgument.IndexOf('=') + 1);
            }
            else
            {
                optionName = firstArgument;
            }

            CommandLineOption option = GetOptionByName(optionName);

            option.Present = true;

            if (option.ParameterType == ParameterType.Required)
            {
                if (parameterValue != null)
                {
                    option.SetParameter(parameterValue);
                    return 1;
                }

                if (args.Length <= position + 1)
                {
                    throw new ParsingException("Parameter required, but no parameter present on the command line.", option);
                }

                option.SetParameter(args[position + 1]);
                return 2;
            }
            else if (option.ParameterType == ParameterType.Optional)
            {
                if (parameterValue != null)
                {
                    option.SetParameter(parameterValue);
                    return 1;
                }

                if (args.Length <= position + 1)
                {
                    return 1;
                }

                option.SetParameter(args[position + 1]);
                return 2;
            }
            else if (option.ParameterType == ParameterType.None)
            {
                if (parameterValue != null)
                {
                    throw new ParsingException("Parameter present for an option that doesn't take parameters.", option);
                }

                return 1;
            }

            throw new InvalidOperationException(); // cannot occur
        }

        internal void CheckNameAvailable(string name)
        {
            if (Options.Find(x => x.Name == name) != null)
            {
                throw new ConfigurationException("The option with this name already exists.");
            }
        }

        internal void CheckShortNameAvailable(string shortName)
        {
            if (Options.Find(x => x.ShortName == shortName) != null)
            {
                throw new ConfigurationException("The option with this short name already exists.");
            }
        }

        /// <summary>
        /// Adds the specified CommandLineOption object to the list of supported command-line options.
        /// </summary>
        /// <param name="option">The option to add.</param>
        public void AddOption(CommandLineOption option)
        {
            CheckNameAvailable(option.Name);

            Options.Add(option);
            option.Parser = this;
        }

        /// <summary>
        /// Prints the summary of all supported options, including their names, short names and descriptions.
        /// The output is printed into Console.Out object.
        /// </summary>
        public void PrintHelp()
        {
            PrintHelp(Console.Out);
        }

        /// <summary>
        /// Prints the summary of all supported options, including their names, short names and descriptions
        /// into the specified TextWriter object.
        /// </summary>
        /// <param name="output">A TextWriter object that is used for output.</param>
        public void PrintHelp(TextWriter output)
        {
            output.WriteLine("Options");
			
            foreach (CommandLineOption option in Options)
			{
				option.PrintHelp(output);
			}
        }

        /// <summary>
        /// Retrieves a option previously added with AddOption by its name.
        /// </summary>
        /// <param name="name">The name of the option to retrieve.</param>
        /// <returns>The option with the speicified name</returns>
        public CommandLineOption GetOptionByName(string name)
        {
            return Options.First(x => x.Name == name);
        }

        /// <summary>
        /// Retrieves a option previously added with AddOption by its short name.
        /// </summary>
        /// <param name="shortName">The short name of the option to retrieve.</param>
        /// <returns>The option with the speicified short name</returns>
        public CommandLineOption GetOptionByShortName(string shortName)
        {
            return Options.First(x => x.ShortName == shortName);
        }
    }

    /// <summary>
    /// Specifies whether an option takes a parameter, and if it is optional or required.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// The option with this ParameterType doesn't take any parameter.
        /// </summary>
        None,

        /// <summary>
        /// The parameter for an option with this ParameterType can have a parameter, but doesn't have to.
        /// If there is any value specified, it is considered a parameter.
        /// </summary>
        Optional,

        /// <summary>
        /// Option with this ParameterType must have a parameter present, otherwise parsing such command line
        /// will throw an exception.
        /// </summary>
        Required
    }

    /// <summary>
    /// The type of the callback delegate that is used when parsing options.
    /// </summary>
    /// <param name="option">The option that was parsed.</param>
    public delegate void ParameterDelegate(CommandLineOption option);

}
