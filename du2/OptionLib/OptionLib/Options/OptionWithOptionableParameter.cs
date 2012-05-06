using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// Option with voluntary parameter. If the option is not used, value used for initialization (if any) is used. 
    /// If the option is specified, but the parameter value is not, default value from constructor is used.
    /// </summary>
    public sealed class OptionWithOptionableParameterAttribute : OptionBase
    {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="defaultValue">Value to be used, if option is specified without parameter.</param>
            public OptionWithOptionableParameterAttribute(object defaultValue)
                : base() {
                    this.defaultValue = defaultValue;
            }

            public string ParameterName {
                get;
                set;
            }

            protected override string GetHelpTextForName(string name, NameType type) {
                switch (type) {
                    case NameType.Short: return string.Format("-{0}[ {1}]", name, ParameterName);
                    case NameType.Long: return string.Format("--{0}[={1}]", name, ParameterName);
                    default: throw new NotImplementedException();
                }
            }

            private object defaultValue;

            public object DefaultValue {
                get {
                    return defaultValue;
                }
            }
        }
}
