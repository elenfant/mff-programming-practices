using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// Abstract base class for argument parsing exceptions.
    /// </summary>
    public abstract class OptionException : Exception
    {
        /// <summary>
        /// name of the option causing the exception
        /// </summary>
        public string optionName;
        /// <summary>
        /// Exception constructor.
        /// </summary>
        /// <param name="optionName">name of the option causing the exception</param>
        /// <param name="message">optional message</param>
        public OptionException(string optionName, string message = "")
            : base(message) {
            this.optionName = optionName;
        }
    }

    /// <summary>
    /// RequiredOptionMissingException is thrown when required option is missing in program arguments.
    /// </summary>
    public class RequiredOptionMissingException : OptionException
    {
        /// <summary>
        /// Exception constructor.
        /// </summary>
        /// <param name="optionName">name of the option causing the exception</param>
        /// <param name="message">optional message</param>
        public RequiredOptionMissingException(string optionName, string message = "")
            : base(optionName, message) {
        }
    }

    /// <summary>
    /// OptionOutOfBoundException is thrown when argument value is out of given bound(s).
    /// </summary>
    public class OptionOutOfBoundException : OptionException
    {
        /// <summary>
        /// value, which violates given bounds
        /// </summary>
        public IComparable value = null;
        /// <summary>
        /// value of the violated bound (either upper or lower)
        /// </summary>
        public object bound = null;

        /// <summary>
        /// Exception constructor.
        /// </summary>
        /// <param name="optionName">name of the option causing the exception</param>
        /// <param name="message">optional message</param>
        /// <param name="value">value, which violates given bounds</param>
        /// <param name="bound">value of the violated bound (either upper or lower)</param>
        public OptionOutOfBoundException(string optionName, string message = "", IComparable value = null, object bound = null)
            : base(optionName)
        {
            this.value = value;
            this.bound = bound;
        }
    }

    /// <summary>
    /// OptionInvalidException is thrown when argument doesn't match any option and starts with <c>'-'</c> character
    /// </summary>
    public class OptionInvalidException : OptionException
    {
        /// <summary>
        /// Exception constructor.
        /// </summary>
        /// <param name="optionName">name of the option causing the exception</param>
        /// <param name="message">optional message</param>
        public OptionInvalidException(string optionName, string message = "")
            : base(optionName, message) {
        }
    }

    /// <summary>
    /// OptionsClashException is thrown when required parameter for some option is expected, but another option found.
    /// </summary>
    public class OptionsClashException : OptionException
    {
        /// <summary>
        /// name of the option found instead of expected required parameter
        /// </summary>
        public string argName;
        /// <summary>
        /// Exception constructor.
        /// </summary>
        /// <param name="optionName">name of the option causing the exception</param>
        /// <param name="argName">name of the option found instead of expected required parameter</param>
        /// <param name="message">optional message</param>
        public OptionsClashException(string optionName, string argName, string message = "")
            : base(optionName, message) {
            this.argName = argName;
        }
    }

    /// <summary>
    /// OptionParameterDisallowedException is thrown when user tries to specify parameter for option with no parameter.
    /// </summary>
    public class OptionParameterDisallowedException : OptionException
    {
        /// <summary>
        /// value of the parameter of option defined without parameter
        /// </summary>
        public string parameterValue;
        /// <summary>
        /// Exception constructor.
        /// </summary>
        /// <param name="optionName">name of the option causing the exception</param>
        /// <param name="value">value of the parameter of option defined eithout parameters</param>
        /// <param name="message">optional message</param>
        public OptionParameterDisallowedException(string optionName, string value, string message = "")
            : base(optionName, message) {
            this.parameterValue = value;
        }
    }

    /// <summary>
    /// RequiredParameterMissingException is thrown when required parameter is missing for given option.
    /// </summary>
    public class RequiredParameterMissingException : OptionException
    {
        /// <summary>
        /// Exception constructor.
        /// </summary>
        /// <param name="optionName">name of the option without required parameter specified</param>
        /// <param name="message">optional message</param>
        public RequiredParameterMissingException(string optionName, string message = "")
            : base(optionName, message) {
        }
    }

}
