using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// Bounds used for corresponding comparable type option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class BoundsAttribute : Attribute
    {
        /// <summary>
        /// Class contructor. Both bounds are set to null, representing no limits.
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
            return LowerBound == null ? true : value.CompareTo(LowerBound) >= 0;
        }

        /// <summary>
        /// Checks if value is lesser or equal than UpperBound
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        public bool CheckUpperBound(IComparable value) {
            return UpperBound == null ? true : value.CompareTo(UpperBound) <= 0;
        }

        /// <summary>
        /// Checks if bound values have correct type.
        /// </summary>
        /// <param name="fieldInfo">Corresponding option field info</param>
        /// <param name="optionName">Option name</param>
        public  void CheckBoundsDefinition(System.Reflection.FieldInfo fieldInfo, string optionName) {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(fieldInfo);
            if (converter.CanConvertFrom(LowerBound.GetType()) && converter.CanConvertFrom(UpperBound.GetType())) {
                LowerBound = converter.ConvertFrom(LowerBound);
                UpperBound = converter.ConvertFrom(UpperBound);
            }
            else {
                throw new InvalidDefinitionException(optionName, "Values in LowerBound and UpperBound must have same type as corresponding option field " + fieldInfo.Name);
            }
        }

    }
}
