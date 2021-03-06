﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    /// <summary>
    /// Base class for option names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class NameAttribute : Attribute
    {
        private List<string> names;

        /// <summary>
        /// Names collection for corresponding option.
        /// </summary>
        public List<string> Names {
            get {
                return names;
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="names">List of names for corresponding option.</param>
        protected NameAttribute(params string[] names) {
            this.names = new List<string>();
            if (names == null) {
                return;
            }
            foreach (string name in names) {
                if (name == null || name.Trim().Length == 0) {
                    continue;
                }
                this.names.Add(name);
            }
        }
    }
}
