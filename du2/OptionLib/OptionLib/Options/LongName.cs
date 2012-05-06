using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// List of long names used for corresponding option.
    /// </summary>
    public sealed class LongNameAttribute : NameAttribute
    {
        public LongNameAttribute(params string[] names)
            : base(names) {
        }
    }
}
