using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLine
{
    /// <summary>
    /// Represents a string command line option.
    /// </summary>
    public class CommandLineStringOption : CommandLineOption
    {
        /// <summary>
        /// The list of allowed values that the parameter of this option can be. If this list is empty,
        /// then the parameter can have any value. A ParsingException is thrown during parsing when the value
        /// of this parameter is not from this list.
        /// </summary>
        public List<string> AllowedValues = new List<string>();

        /// <summary>
        /// Initializes an string command-line option with the specified long name.
        /// </summary>
        /// <param name="name">Long name of this option.</param>
        public CommandLineStringOption(string name) : base(name) { }

        /// <summary>
        /// Initializes an string command-line option with the specified long name and short name.
        /// </summary>
        /// <param name="name">Long name of this option.</param>
        /// <param name="shortName">Short name of this option.</param>
        public CommandLineStringOption(string name, string shortName) : base(name, shortName) { }

        internal override void ParseParameter(object parameterValue)
        {
            if (AllowedValues.Count > 0)
            {
                if (!AllowedValues.Contains(parameterValue))
                {
                    throw new ParsingException("The specified value is not from the allowed list.", this);
                }
            }

            this.Value = parameterValue;
        }
    }
}
