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
        public override void CheckDefinition(System.Reflection.FieldInfo fieldInfo) {
            if (fieldInfo.FieldType != typeof(bool)) {
                throw new InvalidDefinitionException("Corresponding field to Option must be bool.");
            }
        }
    }
}
