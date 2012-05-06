using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib.Other
{
    public class ProgramOptionsBase
    {
        /* asi stejne jako uzivatele definovane volby bych nechal argumenty public
         * stejne se jedna o kopie pole argumentu z parseru,
         * tak si je muze uzivatel zprasit jak chce */
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
