using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLine
{
    /// <summary>
    /// Represents a boolean command line option.
    /// </summary>
    public class CommandLineBoolOption : CommandLineOption
    {
        /// <summary>
        /// Initializes an boolean command-line option with the specified long name.
        /// </summary>
        /// <param name="name">Long name of this option.</param>
        public CommandLineBoolOption(string name) : this(name, null) { }

        /// <summary>
        /// Initializes an boolean command-line option with the specified long name and short name.
        /// </summary>
        /// <param name="name">Long name of this option.</param>
        /// <param name="shortName">Short name of this option.</param>
        public CommandLineBoolOption(string name, string shortName)
            : base(name, shortName)
        {
            ParameterType = ParameterType.None;
        }

        internal override void ParseParameter(object parameterValue)
        {
            throw new InvalidOperationException(); // cannot occur
        }
    }
}
