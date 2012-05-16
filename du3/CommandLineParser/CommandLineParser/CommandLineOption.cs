using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommandLine
{
    /// <summary>
    /// Provides a base class for all command-line options.
    /// </summary>
    public abstract class CommandLineOption
    {
        internal CommandLineParser Parser;

        private string m_name;

        /// <summary>
        /// Name of the option (long name). For example when this value is "format", then this option
        /// is recognized as "--format" on the command line.
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (Parser != null)
                {
                    Parser.CheckNameAvailable(value);
                }

                if ((value == null) || (value == ""))
                {
                    throw new ConfigurationException("The name of the option cannot be null or an empty string.");
                }

                m_name = value;
            }
        }

        private string m_shortName;

        /// <summary>
        /// Short name of the option, must be a single alphanumeric character, or null if you don't require the
        /// option to be available as a short option. For example, short name "f" will be available as "-f" on
        /// the command line.
        /// </summary>
        public string ShortName
        {
            get
            {
                return m_shortName;
            }
            set
            {
                if (Parser != null)
                {
                    Parser.CheckShortNameAvailable(value);
                }

                if ((value != null) && (value.Length != 1))
                {
                    throw new ConfigurationException("The short name of an option must be only one letter or null.");
                }

                m_shortName = value;
            }
        }

        /// <summary>
        /// If true, then the option must be present on the command line (or an ParsingException is thrown when parsing).
        /// </summary>
        public bool Required;

        /// <summary>
        /// Specifies whether an option takes a parameter, and if it is optional or required.
        /// </summary>
        public ParameterType ParameterType = ParameterType.Required;

        /// <summary>
        /// A string representing a description of this option. It is used when printing help message to the console.
        /// </summary>
        public string Help;

        /// <summary>
        /// The description of the expected parameter value type. It is used when printing help message to the console.
        /// </summary>
        public string ExpectedValue = "value";

        /// <summary>
        /// A bool indicating whether this option was present on the command line when parsing.
        /// </summary>
        public bool Present;

        /// <summary>
        /// The value (parameter) of this option. Before parsing, this is null, after it, it contains the value of this
        /// option's parameter. The type of this property depends on the type of the CommandLineOption object. For example
        /// when using CommandLineIntOption, this property is an int.
        /// </summary>
        public dynamic Value
        {
            get;
            internal set;
        }

        /// <summary>
        /// A delegate that is called after the parameter value was parsed.
        /// </summary>
        public ParameterDelegate Delegate;

        internal CommandLineOption(string Name) : this(Name, null) { }

        internal CommandLineOption(string Name, string ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }

        internal void SetParameter(object parameterValue)
        {
            ParseParameter(parameterValue);

            if (Delegate != null)
            {
                Delegate(this);
            }
        }

        /// <summary>
        /// Prints the description (name, short name, help text) of this option into the Console.Out object.
        /// </summary>
        public void PrintHelp()
        {
            PrintHelp(Console.Out);
        }

        /// <summary>
        /// Prints the description (name, short name, help text) of this option into the specified TextWriter object.
        /// </summary>
        /// <param name="output">A TextWriter object that is used for output.</param>
        public void PrintHelp(TextWriter output)
        {
            String expectedValue = "";

            if (ParameterType == CommandLine.ParameterType.Required)
            {
                expectedValue = ExpectedValue.ToUpper();
            }
            else
            {
                expectedValue = ExpectedValue.ToLower();
            }

            output.Write("\t");

            if (ShortName != null)
            {
                output.Write("-{0}", ShortName);

                if (ParameterType == CommandLine.ParameterType.Required || ParameterType == CommandLine.ParameterType.Optional)
                {
                    output.Write(" {0}", expectedValue);
                }

                if (Name != null)
                {
                    output.Write(", ");
                }
            }

            if (Name != null)
            {
                output.Write("--{0}", Name);

                if (ParameterType == CommandLine.ParameterType.Required || ParameterType == CommandLine.ParameterType.Optional)
                {
                    output.Write("={0}", expectedValue);
                }
            }

            if (Help != null)
            {
                output.WriteLine();
                output.WriteLine("\t\t{0}", Help);
            }

            output.WriteLine();
        }

        abstract internal void ParseParameter(object parameterValue);
    }

}
