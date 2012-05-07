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
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="names">List of long names for corresponding option</param>
        public LongNameAttribute(params string[] names)
            : base(names) {
        }
    }
}
