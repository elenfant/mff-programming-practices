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
        public void minValueBorderFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MinValue = 1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "1"});
            Assert.Equal(1, port.Value);
        }

        [Fact]
        public void minValueFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MinValue = 1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", Int32.MaxValue.ToString() });
            Assert.Equal(Int32.MaxValue, port.Value);
        }

        [Fact]
        public void maxValueBorderFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "-1" });
            Assert.Equal(-1, port.Value);
        }

        [Fact]
        public void maxValueFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", Int32.MinValue.ToString() });
            Assert.Equal(Int32.MinValue, port.Value);
        }

        [Fact]
        public void minmaxValueFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = 1;
            port.MinValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "0" });
            Assert.Equal(0, port.Value);
        }

        [Fact]
        public void greaterThanMaxValueThrowsException()
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
        }

        [Fact]
        public void lesserThanMinValueThrowsException()
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
        }

        [Fact]
        public void tooBigParameterThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--port", Int64.MaxValue.ToString() });
                });
        }

        [Fact]
        public void tooSmallParameterThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--port", Int64.MinValue.ToString() });
                });
        }

        [Fact]
        public void floatParameterThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--port", "4.0" });
                });
        }

        [Fact]
        public void stringParameterThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--port", "eighty" });
                });
        }
        
        [Fact]
        public void emptyParameterThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--port", "" });
                });
        }

        [Fact]
        public void nullParameterThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", null });
            
            Assert.Equal(0, port.Value);
        }

        [Fact]
        public void parameterTypeNoneFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            port.ParameterType = ParameterType.None;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port" });

            Assert.Null(port.Value);
        }

        [Fact]
        public void missingParameterTypeOptionalFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            port.ParameterType = ParameterType.Optional;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port" });

            Assert.Null(port.Value);
        }

        [Fact]
        public void parameterTypeOptionalFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            port.ParameterType = ParameterType.Optional;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "8080" });

            Assert.Equal(8080, port.Value);
        }

    }
}