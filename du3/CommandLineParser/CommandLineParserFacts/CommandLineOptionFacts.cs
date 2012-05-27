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
        public void nullOptionNameThrowsException()
        {
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    CommandLineBoolOption version = new CommandLineBoolOption(null);
                });

            /* null the Name after beeing constructed */
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    verbose.Name = null;
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

            /* empty the Name after beeing constructed */
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    verbose.Name = "";
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

            /* empty the ShortName after beeing constructed */
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    verbose.ShortName = "";
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

            /* empty the Name after beeing constructed */
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", "v");
            Assert.Throws<CommandLine.ConfigurationException>(
                delegate
                {
                    verbose.ShortName = "version";
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

        //TODO Resolve: Null ExpectedValue should print some default "value", but leads to NullReferenceException during PrintHelp().
        [Fact]
        public void nullExpectedValueFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            verbose.ExpectedValue = null;
            parser.AddOption(verbose);
            parser.PrintHelp();
        }

    }
}