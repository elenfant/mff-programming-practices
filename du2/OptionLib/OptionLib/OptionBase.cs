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
}
