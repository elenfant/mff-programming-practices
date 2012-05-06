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

        private List<string> shortNames;
        private List<string> longNames;

        public void SetValue(object valueToSet) {
            if (fieldInfo.FieldType == valueToSet.GetType()) {
                fieldInfo.SetValue(fieldInfo, valueToSet);
            }
        }

        public Type GetOptionType() {
            return fieldInfo.FieldType;
        }

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
            return true;
        }

        private bool SetEnumValue(string textValue, ProgramOptionsBase options) {
            try {
                var value = Enum.Parse(fieldInfo.FieldType, textValue, true);
                fieldInfo.SetValue(options, value);
            } catch {
                return false;
            }
            return true;
        }

        public string PrintHelp() {
            return optionAttribute.GetHelpText(shortNames, longNames);
        }
    }


}
