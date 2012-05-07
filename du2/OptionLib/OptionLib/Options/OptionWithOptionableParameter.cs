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
        /// Class contructor.
        /// </summary>
        /// <param name="defaultValue">Value to be used, if option is specified without parameter.</param>
        public OptionWithOptionableParameterAttribute(object defaultValue)
            : base() {
            this.defaultValue = defaultValue;
        }

        /// <summary>Parameter name property</summary>
        /// <value>Stores the name of the option parameter.</value>
        /// <remarks>ParameterName is used for printing help.</remarks>
        public string ParameterName {
            get;
            set;
        }

        /// <summary>
        /// Prepares help text for given option string <paramref name="name"/> and its <paramref name="type"/>.
        /// </summary>
        /// <param name="name">Option string.</param>
        /// <param name="type">Type of given option string. <see cref="NameType"/> for possible values (Short or Long).</param>
        /// <returns>Help string for given option.</returns>
        /// <seealso cref="NameType" />
        /// <seealso cref="ParameterName"/>
        /// <remarks>Uses ParameterName for displaying the optional parameter./>.
        /// <example><para><c>-f</c> is short option string</para><para><c>--format</c> is long option string</para></example></remarks>
        protected override string GetHelpTextForName(string name, NameType type) {
            switch (type) {
                case NameType.Short: return string.Format("-{0}[ {1}]", name, Printer.GetParameterName(ParameterName));
                case NameType.Long: return string.Format("--{0}[={1}]", name, Printer.GetParameterName(ParameterName));
                default: throw new NotImplementedException();
            }
        }

        private object defaultValue;

        /// <summary>Default value property</summary>
        /// <value>Stores the default value for optionable parameter.</value>
        /// <remarks>Default value is used when option is provided without any parameter.</remarks>
        public object DefaultValue {
            get {
                return defaultValue;
            }
        }

        /// <summary>
        /// Checks if value in defaultValue field has same type as corresponding field, else tries to convert it. If conversion fails, exception is thrown.
        /// </summary>
        /// <param name="fieldInfo">FieldInfo of corresponding field</param>
        /// <param name="optionName">Option name</param>
        public override void CheckDefinition(System.Reflection.FieldInfo fieldInfo, string optionName) {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(fieldInfo);
            if (converter.CanConvertFrom(defaultValue.GetType())) {
                defaultValue = converter.ConvertFrom(defaultValue);
            } else {
                throw new InvalidDefinitionException(optionName, "Value in defaultValue must have same type as corresponding option field " + fieldInfo.Name); 
            }

        }
    }
}
