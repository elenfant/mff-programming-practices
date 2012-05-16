using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLine
{
    /// <summary>
    /// Represents a integer command line option.
    /// </summary>
    public class CommandLineIntOption : CommandLineOption
    {
        /// <summary>
        /// The minimal valid value of this option's parameter, or null if there is no minimal value.
        /// </summary>
        public int? MinValue;

        /// <summary>
        /// The maximal valid value of this option's parameter, or null if there is no maximal value.
        /// </summary>
        public int? MaxValue;

        /// <summary>
        /// Initializes an integer command-line option with the specified long name.
        /// </summary>
        /// <param name="name">Long name of this option.</param>
        public CommandLineIntOption(string name) : base(name) { }

        /// <summary>
        /// Initializes an integer command-line option with the specified long name and short name.
        /// </summary>
        /// <param name="Name">Long name of this option.</param>
        /// <param name="ShortName">Short name of this option.</param>
        public CommandLineIntOption(string Name, string ShortName) : base(Name, ShortName) { }

        internal override void ParseParameter(object parameterValue)
        {
            int newValue = Convert.ToInt32(parameterValue);

            if (MinValue != null && newValue < MinValue)
            {
                throw new ParsingException("Parameter value is less than MinValue.", this);
            }

            if (MaxValue != null && newValue > MaxValue)
            {
                throw new ParsingException("Parameter value is greater than MaxValue.", this);
            }

            this.Value = newValue;
        }
    }
}
