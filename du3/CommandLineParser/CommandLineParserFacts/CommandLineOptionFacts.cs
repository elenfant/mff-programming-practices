using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{
    /// <summary>Class for testing usage of base abstract CommandLineOption class.</summary>
    public class OptionFacts
    {
        CommandLineParser parser;

        public OptionFacts()
        {
            this.parser = new CommandLineParser();
        }

        /// <summary>Tests creating of option with null long name.</summary>
        [Fact(Timeout = 5000)]
        public void nullNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption(null);
                });
        }

        /// <summary>Tests creating of option with empty long name.</summary>
        [Fact(Timeout = 5000)]
        public void emptyOptionNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption("");
                });
        }

        /// <summary>Tests creating of option with empty short name.</summary>
        [Fact(Timeout = 5000)]
        public void emptyOptionShortNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption("version", "");
                });
        }

        /// <summary>Tests creating of option short name set to multicharacter string.</summary>
        [Fact(Timeout = 5000)]
        public void multicharOptionShortNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption("version", "version");
                });
        }

        /// <summary>Tests adding two options with same long name to the parser.</summary>
        [Fact(Timeout = 5000)]
        public void sameOptionNameThrowsException()
        {
            string sameOptionName = "verbose";
            CommandLineBoolOption verbose = new CommandLineBoolOption(sameOptionName);
            parser.AddOption(verbose);

            CommandLineBoolOption version = new CommandLineBoolOption(sameOptionName);

            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    parser.AddOption(version);
                });
        }

        //TODO Resolve: Two options with same ShortName should be banned.
        /* this should throw ConfigurationException, since we end up with two options with the same ShortName */
        /// <summary>Tests adding two options with same short name to the parser.</summary>
        [Fact(Timeout = 5000)]
        public void sameOptionShortNameThrowsException()
        {
            string sameOptionShortName = "v";
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", sameOptionShortName);
            parser.AddOption(verbose);

            CommandLineBoolOption version = new CommandLineBoolOption("version", sameOptionShortName);

            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    parser.AddOption(version);
                });
        }

        /// <summary>Tests adding delegate to option.</summary>
        [Fact(Timeout = 5000)]
        public void delegateFact()
        {
            int portNumber = -1;
            CommandLineIntOption portOption = new CommandLineIntOption("port", "p");
            portOption.Delegate = arg => portNumber = arg.Value;
            parser.AddOption(portOption);

            parser.Parse(new string[] {"-p8080"});
            Assert.Equal(8080, portNumber);
        }

    }
}