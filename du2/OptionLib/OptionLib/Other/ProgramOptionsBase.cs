using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class ProgramOptionsBase
    {
        public List<string> arguments = new List<string>();
        private ArgumentParser parser = new ArgumentParser();
        private SortedSet<OptionBase> requiredOptions = new SortedSet<OptionBase>();

        public void Initialize(string[] args)
        {
            arguments = parser.ProcessCommandLine(this, args);
        }

        public void AddRequiredOption(OptionBase option)
        {
            requiredOptions.Add(option);
        }
    }
}
