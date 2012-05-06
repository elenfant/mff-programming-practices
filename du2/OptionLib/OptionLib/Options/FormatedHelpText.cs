using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class FormatedHelpTextAttribute : Attribute
    {
        private string formatedHelpText;

        public string HelpText {
            get {
                return formatedHelpText;
            }
        }

        public FormatedHelpTextAttribute(string helpText) {
            formatedHelpText = helpText;
        }
    }
}
