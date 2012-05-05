using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OptionBase : Attribute
    {
        
    }

    /* docasne reseni i umisteni, jen pro alespon minimalni testovaci ucely */
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShortNameAttribute : OptionBase
    {
        protected string shortName;
        public string ShortName
        {
            get { return shortName; }
        }
        
        public ShortNameAttribute(string name)
        {
            shortName = name;
        }

    }

    /* docasne reseni i umisteni, jen pro alespon minimalni testovaci ucely */
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LongNameAttribute : OptionBase
    {
        protected string longName;
        public string LongName
        {
            get { return longName; }
        }
        
        public LongNameAttribute(string name)
        {
            longName = name;
        }

    }
}
