using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class ProgramOptionsBase
    {
        internal const string argumentsVariableName = "arguments";
        public List<string> arguments = new List<string>();

        internal const string parserVariableName = "parser";
        private ArgumentParser parser = new ArgumentParser();

        public void Initialize(string[] args)
        {
            arguments = parser.ProcessCommandLine(this, args);
        }
    }
}
