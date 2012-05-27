using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{

    public class CommandLineOptionFacts
    {
        CommandLineParser parser;

        public CommandLineOptionFacts() {
            this.parser = new CommandLineParser();
        }

        [Fact]
        public void nullNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption(null);
                });
        }

        [Fact]
        public void emptyOptionNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption("");
                });
        }

        [Fact]
        public void emptyOptionShortNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption("version", "");
                });
        }

        [Fact]
        public void multicharOptionShortNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption("version", "version");
                });
        }

        [Fact]
        public void sameOptionNameThrowsException()
        {
            const string sameOptionName = "verbose";
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
        [Fact]
        public void sameOptionShortNameThrowsException()
        {
            const string sameOptionShortName = "v";
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", sameOptionShortName);
            parser.AddOption(verbose);

            CommandLineBoolOption version = new CommandLineBoolOption("version", sameOptionShortName);

            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    parser.AddOption(version);
                });
        }

        [Fact]
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