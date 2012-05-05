using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class OptionAttribute : OptionBase
    {
        protected string description;
        public string Description
        {
            get { return description; }
        }

        public OptionAttribute(string description, bool required = false)
        {
            this.description = description;
        }

        public OptionAttribute(ICollection<string> shortNames, ICollection<string> longNames, string description, bool required = false)
        {
 
        }
    }
}
