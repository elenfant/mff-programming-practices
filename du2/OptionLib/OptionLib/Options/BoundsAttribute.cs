using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//TODO: predelat na int i float, tj. zadávat hodnotu attributu jako string
namespace OptionLib
{
    /// <summary>
    /// Bounds used for corresponding integer option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class BoundsAttribute : Attribute
    {
        public BoundsAttribute()
            : base() {
                LowerBound = int.MinValue;
                UpperBound = int.MaxValue;
        }

        public int LowerBound
        {
            get;
            set;
        }

        public int UpperBound
        {
            get;
            set;
        }
    }
}
