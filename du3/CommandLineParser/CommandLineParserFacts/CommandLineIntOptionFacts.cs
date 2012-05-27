using System;
using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{
    public class CommandLineIntOptionFacts
    {
        private CommandLineParser parser;

        public CommandLineIntOptionFacts()
        {
            this.parser = new CommandLineParser();
        }

        [Fact]
        public void positiveIntOptionBorderFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MinValue = 1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "1"});
            Assert.True(port.Present);
            Assert.InRange(port.Value, 1, Int32.MaxValue);
        }

        [Fact]
        public void positiveIntOptionFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MinValue = 1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", Int32.MaxValue.ToString() });
            Assert.True(port.Present);
            Assert.InRange(port.Value, 1, Int32.MaxValue);
        }

        [Fact]
        public void negativeIntOptionBorderFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "-1" });
            Assert.True(port.Present);
            Assert.InRange(port.Value, Int32.MinValue, -1);
        }

        [Fact]
        public void negativeIntOptionFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", Int32.MinValue.ToString() });
            Assert.True(port.Present);
            Assert.InRange(port.Value, Int32.MinValue, -1);
        }

        [Fact]
        public void boundedIntOptionFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = 1;
            port.MinValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "0" });
            Assert.True(port.Present);
            Assert.InRange(port.Value, -1, 1);
        }

        [Fact]
        public void intOptionValueGreaterThanBoundaryThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = 1;
            port.MinValue = -1;
            parser.AddOption(port);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--port", "2012" });
                });
            Assert.True(port.Present);
            Assert.Null(port.Value);
        }

        [Fact]
        public void intOptionValueLesserThanBoundaryThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = 1;
            port.MinValue = -1;
            parser.AddOption(port);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--port", "-37" });
                });
            Assert.True(port.Present);
            Assert.Null(port.Value);
        }

    }
}