using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// Option with required parameter, corresponds to field, which holds the parameter value. 
    /// </summary>
    public sealed class OptionWithParameterAttribute : OptionBase
    {
        public OptionWithParameterAttribute()
            : base() {
        }

        public string ParameterName {
            get;
            set;
        }

        protected override string GetHelpTextForName(string name, NameType type) {
            switch (type) {
                case NameType.Short: return string.Format("-{0} {1}", name, ParameterName);
                case NameType.Long: return string.Format("--{0}={1}", name, ParameterName);
                default: throw new NotImplementedException();
            }
        }
    }
}
