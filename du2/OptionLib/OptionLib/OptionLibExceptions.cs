using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    public abstract class OptionException : Exception
    {
        public string optionName;
        public OptionException(string optionName, string message = "")
            : base(message) {
            this.optionName = optionName;
        }
    }

    public class RequiredOptionMissingException : OptionException
    {
        public RequiredOptionMissingException(string optionName, string message = "")
            : base(optionName, message) {
        }
    }

    public class OptionOutOfBoundException : OptionException
    {
        public IComparable value = null;
        public object bound = null;

        public OptionOutOfBoundException(string optionName, string message = "", IComparable value = null, object bound = null)
            : base(optionName) {
            this.value = value;
            this.bound = bound;
        }
    }

    public class OptionInvalidException : OptionException
    {
        public OptionInvalidException(string optionName, string message = "")
            : base(optionName, message) {

        }
    }

    public class OptionsClashException : OptionException
    {
        public string argName;
        public OptionsClashException(string optionName, string argName, string message = "")
            : base(optionName, message) {
            this.argName = argName;
        }
    }

    public class OptionParameterDisalowedException : OptionException
    {
        public string parameterValue;
        public OptionParameterDisalowedException(string optionName, string value, string message = "")
            : base(optionName, message) {
            this.parameterValue = value;
        }
    }

    public class RequiredParameterMissingException : OptionException
    {
        public RequiredParameterMissingException(string optionName, string message = "")
            : base(optionName, message) {
        }
    }

    public class DefaultOptionMissingException : OptionException
    {
        public DefaultOptionMissingException(string optionName, string message = "")
            : base(optionName) {
        }
    }
}
