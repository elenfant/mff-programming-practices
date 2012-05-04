using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    class OptionWithParameterAttribute : OptionBase
    {
        public OptionWithParameterAttribute(string shortName, string longName, string description, string parameterName = "PAR", bool required = false)
        {
 
        }
    }
}
