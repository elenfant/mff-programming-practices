using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                LowerBound = null;
                UpperBound = null;
        }

        /// <summary>Lower bound property</summary>
        /// <value>Stores the lower bound for given ProgramOptionsBase field.</value>
        public object LowerBound
        {
            get;
            set;
        }

        /// <summary>Upper bound property</summary>
        /// <value>Stores the upper bound for given ProgramOptionsBase field.</value>
        public object UpperBound
        {
            get;
            set;
        }

        /// <summary>
        /// Checks if value is greater or equal than LowerBound.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        public bool CheckLowerBound(IComparable value) {
            if (value.GetType() != LowerBound.GetType()) {
                LowerBound = System.ComponentModel.TypeDescriptor.GetConverter(value.GetType()).ConvertFrom(LowerBound);
            }
            return LowerBound == null ? true : value.CompareTo(LowerBound) >= 0;
        }

        /// <summary>
        /// Checks if value is lesser or equal than UpperBound
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        public bool CheckUpperBound(IComparable value) {
            if (value.GetType() != UpperBound.GetType()) {
                UpperBound = System.ComponentModel.TypeDescriptor.GetConverter(value.GetType()).ConvertFrom(UpperBound);
            }
            return UpperBound == null ? true : value.CompareTo(UpperBound) <= 0;
        }

    }
}
