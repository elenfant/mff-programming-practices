using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class ProgramOptionsBase
    {
        internal const string argumentsVariableName = "arguments";
        List<string> arguments;
        internal const string parserVariableName = "parser";
        ArgumentParser parser = new ArgumentParser();

        public void Initialize(string[] args)
        {
            arguments = parser.ProcessCommandLine(this, args);
            //throw new NotImplementedException();
        }
    }
}
