using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class OptionAttribute : OptionBase
    {
        public OptionAttribute(string description)
        {
            this.description = description;
        }

        public OptionAttribute(ICollection<string> shortNames, ICollection<string> longNames, string description, bool required = false)
        {
 
        }
    }
}
