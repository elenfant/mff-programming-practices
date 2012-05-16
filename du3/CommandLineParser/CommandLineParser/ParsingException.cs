using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLine
{
    /// <summary>
    /// An exception that is thrown during parsing when the command line doesn't meet the requirements of the specified options.
    /// </summary>
    public class ParsingException : Exception
    {
        /// <summary>
        /// The option that is invalid.
        /// </summary>
        public CommandLineOption Option;

        /// <summary>
        /// Initializes a new instance of the ParsingException class with the specified message.
        /// </summary>
        /// <param name="message">The message of this exception.</param>
        public ParsingException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ParsingException class with the specified message and
        /// an inner exception.
        /// </summary>
        /// <param name="message">The message of this exception.</param>
        /// <param name="innerException">The inner exception that caused this exception to happen.</param>
        public ParsingException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the ParsingException class with the specified message and
        /// an instance of CommandLineOption that is invalid.
        /// </summary>
        /// <param name="message">The message of this exception.</param>
        /// <param name="option">The option that caused this exception.</param>
        public ParsingException(string message, CommandLineOption option)
            : base(message)
        {
            this.Option = option;
        }
    }
}
