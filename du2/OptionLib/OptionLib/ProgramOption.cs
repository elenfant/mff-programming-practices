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
                shortNames = shortNameAttribute.Names.AsReadOnly();
            }
            else {
                shortNames = new List<string>().AsReadOnly();
            }
            if (longNameAttribute != null) {
                longNames = longNameAttribute.Names.AsReadOnly();
            }
            else {
                longNames = new List<string>().AsReadOnly();
            }

            if (longNames.Count == 0 && shortNames.Count == 0)
            {
                throw new NotSupportedException("Options must have at least one name. Add ShortName and/or LongName attribute.");
            }
        }

        public IList<string> ShortNames {
            get {
                return shortNames;
            }
        }

        public IList<string> LongNames {
            get {
                return longNames;
            }
        }

        public string Name {
            get {
                if (longNames.Count > 0)
                {
                    return longNames[0];
                }
                else
                {
                    /* there has to be at least one name, otherwise contructor would fail  */
                    return shortNames[0];
                }
            }
        }

        private readonly IList<string> shortNames;
        private readonly IList<string> longNames;

        public bool IsPresent {
            get;
            private set;
        }

        public Type GetOptionType() {
            return fieldInfo.FieldType;
        }

        public Type GetOptionAttributeType()
        {
            return optionAttribute.GetType();
        }

        public bool IsRequired()
        {
            return optionAttribute.Required;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textValue"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool SetValue(string textValue, ProgramOptionsBase options) {
            Type fieldType = fieldInfo.FieldType;

            if (fieldType == typeof(string)) {
                fieldInfo.SetValue(options, textValue);
            } else if (fieldInfo.FieldType.IsEnum) {
                return SetEnumValue(textValue, options);
            } else {
                object value = null;
                try {
                    value = TypeDescriptor.GetConverter(fieldType).ConvertFromString(textValue);
                } catch (NotSupportedException) {
                    return false;
                }
                if (fieldType == typeof(int))
                {
                    BoundsCheck((int)value);
                }
                fieldInfo.SetValue(options, value);
            }
            IsPresent = true;
            return true;
        }

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

        public void SetValueToDefault(ProgramOptionsBase programOptions)
        {
            if (optionAttribute.GetType() != typeof(OptionWithParameterAttribute))
            {
                throw new DefaultOptionMissingException(Name);
            }
            /* user has to set DefaultValue in correct type */
            fieldInfo.SetValue(programOptions, ((OptionWithOptionableParameterAttribute)optionAttribute).DefaultValue);
            IsPresent = true;
        }

        private void BoundsCheck(int value)
        {
            BoundsAttribute bounds = (BoundsAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(BoundsAttribute));
            if (bounds == null)
            {
                return;
            }
            if (value < bounds.LowerBound)
            {
                throw new OptionOutOfBoundException(Name, "", value, bounds.LowerBound);
            }
            if (value > bounds.UpperBound)
            {
                throw new OptionOutOfBoundException(Name, "", value, bounds.UpperBound);
            }

        }

        public string PrintHelp() {
            return optionAttribute.GetHelpText(shortNames, longNames);
        }
    }


}
