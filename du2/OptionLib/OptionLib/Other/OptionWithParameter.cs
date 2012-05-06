using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class OptionWithParameterAttribute : OptionBase
    {
        protected bool parameterRequired;
        public bool isRequired()
        {
            return parameterRequired;
        }

        protected string parameterName;
        public string ParameterName
        {
            get { return parameterName; }
        }

        public OptionWithParameterAttribute(string description, string parameterName = "PAR", bool required = false)
        {
            this.parameterRequired = required;
            this.parameterName = parameterName;
        }
    }
}
