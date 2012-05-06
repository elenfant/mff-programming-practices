using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptionLib;

namespace UseCases
{
    class LsOptions : ProgramOptionsBase
    {
        [Option(Description = "do not hide entries starting with .")]
        [ShortName("a"), LongName("all")]
        public bool all = false;

        [Option(Description = "do not list implied . and ..")]
        [ShortName("A"), LongName("almost-all")]
        public bool almostAll = false;

        [OptionWithParameter(ParameterName = "SIZE", Description = "use SIZE-byte blocks")]
        [LongName("block-size")]
        public int blockSize = 8;

        [Option(Description = "use a long listing format")]
        [ShortName("l")]
        public bool longListing = false;

        [OptionWithParameter(ParameterName = "WORD", Description = "use quoting style WORD for entry names: literal, locale, shell, shell-always, c, escape")]
        [LongName("quoting-style")]
        public QuotingStyle quotingStyle = QuotingStyle.shell;

        protected override string GetProgramHelpText() {
            return "ls - list directory contents\nls [OPTION]... [FILE]...";
        }

        protected override string GetVersionInformation() {
            return "Simple version of ls.";
        }


    }

    public enum QuotingStyle
    {
        literal, locale, shell, shellAlways, c, escape
    }

    class ls_simple
    {

        /*
        static void Main(string[] args) {
            ls(args);
        }
        */

        private static void ls(string[] args) {
            LsOptions options = new LsOptions();
            options.Initialize(args);
            
            ListDirectoryContents(options);
        }

        private static void ListDirectoryContents(LsOptions options) {

            throw new NotImplementedException();
        }
    }
}
