using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//TODO: predelat na int i float like promenne, tj. zadávat hodnotu attributu jako string
namespace OptionLib
{
    /// <summary>
    /// Bounds used for corresponding integer option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class BoundsAttribute : Attribute
    {
        /// <summary>
        /// Class contructor. Both bounds are set to corresponding integer limits.
        /// </summary>
        public BoundsAttribute()
            : base() {
                LowerBound = int.MinValue;
                UpperBound = int.MaxValue;
        }

        /// <summary>Lower bound property</summary>
        /// <value>Stores the lower bound for given ProgramOptionsBase field.</value>
        public int LowerBound
        {
            get;
            set;
        }

        /// <summary>Upper bound property</summary>
        /// <value>Stores the upper bound for given ProgramOptionsBase field.</value>
        public int UpperBound
        {
            get;
            set;
        }
    }
}
