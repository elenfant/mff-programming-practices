using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    class OptionAttribute : OptionBase
    {
        public OptionAttribute(string shortName, string longName, string description, bool required = false)
        {

        }

        public OptionAttribute(ICollection<string> shortNames, ICollection<string> longNames, string description, bool required = false)
        {
 
        }
    }
}
