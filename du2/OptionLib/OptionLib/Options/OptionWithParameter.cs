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
        /// <summary>
        /// Class constructor.
        /// </summary>
        public OptionWithParameterAttribute()
            : base() {
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
        /// <remarks>Uses ParameterName for displaying the required parameter./>.
        /// <example><para><c>-f</c> is short option string</para><para><c>--format</c> is long option string</para></example></remarks>
        protected override string GetHelpTextForName(string name, NameType type)
        {
            switch (type) {
                case NameType.Short: return string.Format("-{0} {1}", name, Printer.GetParameterName(ParameterName));
                case NameType.Long: return string.Format("--{0}={1}", name, Printer.GetParameterName(ParameterName));
                default: throw new NotImplementedException();
            }
        }
    }
}
