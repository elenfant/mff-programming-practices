using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandLine
{
    /// <summary>
    /// An exception that is thrown when an invalid configuration of command line options is used.
    /// </summary>
    public class ConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ConfigurationException class with the specified message.
        /// </summary>
        /// <param name="message">The message of this exception.</param>
        public ConfigurationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ConfigurationException class with the specified message and
        /// an inner exception.
        /// </summary>
        /// <param name="message">The message of this exception.</param>
        /// <param name="innerException">The inner exception that caused this exception to happen.</param>
        public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
