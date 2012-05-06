using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class OptionWithParameterAttribute : OptionBase
    {
        protected bool parameterRequired;
        public bool ParameterRequired
        {
            get { return parameterRequired; }
        }

        public OptionWithParameterAttribute(string description, string parameterName = "PAR", bool required = false)
        {
            this.parameterRequired = required;
        }
    }
}
