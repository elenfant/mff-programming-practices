using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OptionLib
{   
    /// <summary>
    /// Class representing all information about option in running program.
    /// </summary>
    class ProgramOption
    {
        private System.Reflection.FieldInfo fieldInfo;
        private OptionBase optionAttribute;

        public ProgramOption(System.Reflection.FieldInfo fieldInfo, OptionBase optionBase, ShortNameAttribute shortNameAttribute, LongNameAttribute longNameAttribute) {

            this.fieldInfo = fieldInfo;
            this.optionAttribute = optionBase;
            if (shortNameAttribute != null) {
                shortNames = shortNameAttribute.Names;
            }
            else {
                shortNames = new List<string>();
            }
            if (longNameAttribute != null) {
                longNames = longNameAttribute.Names;
            }
            else {
                longNames = new List<string>();
            }
        }

        public List<string> ShortNames {
            get {
                return shortNames;
            }
        }

        public List<string> LongNames {
            get {
                return longNames;
            }
        }

        public bool IsPresent {
            get;
            private set;
        }

        private List<string> shortNames;
        private List<string> longNames;

        public Type GetOptionType() {
            return fieldInfo.FieldType;
        }

        public bool IsRequired()
        {
            return optionAttribute.Required;
        }

        /// <summary>
        /// Sets value to corresponding option. Parser string representation and sets it to corresponding variable
        /// </summary>
        /// <param name="textValue">Value in string representation</param>
        /// <param name="options">Options object to set value on</param>
        /// <returns></returns>
        private bool SetValue(string textValue, ProgramOptionsBase options) {
            Type fieldType = fieldInfo.FieldType;

            if (fieldType == typeof(string)) {
                fieldInfo.SetValue(options, textValue);
            } else if (fieldInfo.FieldType.IsEnum) {
                return SetEnumValue(textValue, options);
            } else {
                try {
                    object value = TypeDescriptor.GetConverter(fieldType).ConvertFromString(textValue);
                    fieldInfo.SetValue(options, value);
                } catch (NotSupportedException) {
                    return false;
                }
            }
            IsPresent = true;
            return true;
        }

        /// <summary>
        /// Parses and sets Enum value to option.
        /// </summary>
        /// <param name="textValue">Value in string representation</param>
        /// <param name="options">Options object to set value on</param>
        /// <returns></returns>
        private bool SetEnumValue(string textValue, ProgramOptionsBase options) {
            try {
                var value = Enum.Parse(fieldInfo.FieldType, textValue, true);
                fieldInfo.SetValue(options, value);
            } catch {
                return false;
            }
            IsPresent = true;
            return true;
        }

        /// <summary>
        /// Prints description of option.
        /// </summary>
        /// <returns>Formatted description</returns>
        public string PrintHelp() {
            return optionAttribute.GetHelpText(shortNames, longNames);
        }
    }


}
