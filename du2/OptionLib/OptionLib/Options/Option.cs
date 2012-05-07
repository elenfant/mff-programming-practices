using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// Basic type of option, corresponds to simple bool field.
    /// </summary>
    public sealed class OptionAttribute : OptionBase
    {
        /// <summary>
        /// Class contructor. OptionAttribute corresponds to simple bool field.
        /// </summary>
        public OptionAttribute() {

        }

        /// <summary>
        /// Checks if corresponding field is bool, as should be.
        /// </summary>
        /// <param name="fieldInfo">Corresponding option field info</param>
        /// <param name="optionName">Option name</param>
        public override void CheckDefinition(System.Reflection.FieldInfo fieldInfo, string optionName) {
            if (fieldInfo.FieldType != typeof(bool)) {
                throw new InvalidDefinitionException(optionName, "Corresponding field to Option must be bool.");
            }
        }
    }
}
