using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// List of short names used for corresponding option.
    /// </summary>
    public sealed class ShortNameAttribute : NameAttribute
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="names">Short names for corresponding option.</param>
        public ShortNameAttribute(params string[] names)
            : base(names) {
        }
    }
}
