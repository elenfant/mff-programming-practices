﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// Base class for all option types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class OptionBase : Attribute
    {
        public OptionBase() { }

        /// <summary>
        /// Description of option, printed in help.
        /// </summary>
        public string Description {
            get;
            set;
        }

        /// <summary>
        /// Defines attribute as required.
        /// </summary>
        public bool Required {
            get;
            set;
        }

        /// <summary>
        /// Builts output help text.
        /// </summary>
        /// <param name="shortNames">Short names of this option.</param>
        /// <param name="longNames">Long names of this option.</param>
        /// <returns>Formatted help text descripting this option.</returns>
        public string GetHelpText(List<string> shortNames, List<string> longNames) {
            StringBuilder namesHelpText = new StringBuilder();
            bool firstName = true;
            foreach (string shortName in shortNames) {
                if (firstName) {
                    firstName = false;
                }
                else {
                    namesHelpText.Append(", ");
                }
                namesHelpText.Append(GetHelpTextForName(shortName, NameType.Short));
            }
            foreach (string longName in longNames) {
                if (firstName) {
                    firstName = false;
                }
                else {
                    namesHelpText.Append(", ");
                }
                namesHelpText.Append(GetHelpTextForName(longName, NameType.Long));
            }
            string formattedNamesHelpText = Printer.FormatTextToPrint(namesHelpText.ToString(), Printer.FIRST_LEVEL_INDENT, Console.WindowWidth);
            StringBuilder helpText = new StringBuilder();
            helpText.AppendLine(formattedNamesHelpText);
            if (Required) {
                helpText.AppendLine(Printer.FormatTextToPrint("This option is required.", Printer.SECOND_LEVEL_INDENT, Console.WindowWidth));
            }
            
            helpText.Append(Printer.FormatTextToPrint(Description, Printer.SECOND_LEVEL_INDENT, Console.WindowWidth));
            return helpText.ToString();
        }

        protected virtual string GetHelpTextForName(string name, NameType type) {
            switch (type) {
                case NameType.Short: return string.Format("-{0}", name);
                case NameType.Long: return string.Format("--{0}", name);
                default: throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Types of names for options.
    /// </summary>
    public enum NameType { Short, Long }
}