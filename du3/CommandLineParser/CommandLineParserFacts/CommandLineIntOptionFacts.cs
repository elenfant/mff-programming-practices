using System;
using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{
    /// <summary>Class for testing usage of CommandLineIntOption class.</summary>
    public class IntOptionFacts
    {
        private CommandLineParser parser;

        public IntOptionFacts()
        {
            this.parser = new CommandLineParser();
        }

        /// <summary>Tests assigning of minimal allowed value.</summary>
        [Fact(Timeout = 5000)]
        public void minValueBorderFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.ParameterType = ParameterType.Optional;
            port.MinValue = 1;
            parser.AddOption(port);

            parser.Parse(new string[] { "-p1"});
            Assert.Equal(1, port.Value);
        }

        /// <summary>Tests assigning of value bigger than minimal allowed value.</summary>
        [Fact(Timeout = 5000)]
        public void minValueFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MinValue = 1;
            port.ParameterType = ParameterType.Optional;
            parser.AddOption(port);

            parser.Parse(new string[] { "-p", Int32.MaxValue.ToString() });
            Assert.Equal(Int32.MaxValue, port.Value);
        }

        /// <summary>Tests assigning of maximal allowed value.</summary>
        [Fact(Timeout = 5000)]
        public void maxValueBorderFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "-1" });
            Assert.Equal(-1, port.Value);
        }

        /// <summary>Tests assigning of value lesser than maximal allowed value.</summary>
        [Fact(Timeout = 5000)]
        public void maxValueFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", Int32.MinValue.ToString() });
            Assert.Equal(Int32.MinValue, port.Value);
        }

        /// <summary>Tests assigning of value in the user-defined range.</summary>
        [Fact(Timeout = 5000)]
        public void minmaxValueFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.MaxValue = 1;
            port.MinValue = -1;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "0" });
            Assert.Equal(0, port.Value);
        }

        /// <summary>Tests assigning of value bigger than maximal allowed value.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests assigning of value lesser than minimal allowed value.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests assigning of too large integer.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests assigning of too small integer.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests assigning of float number.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests assigning of string.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests assigning of empty parameter.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests assigning of null parameter.</summary>
        [Fact(Timeout = 5000)]
        public void nullParameterThrowsException()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", null });
            
            Assert.Equal(0, port.Value);
        }

        /// <summary>Tests setting ParameterType to ParameterType.None.</summary>
        [Fact(Timeout = 5000)]
        public void parameterTypeNoneFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            port.ParameterType = ParameterType.None;
            parser.AddOption(port);

            parser.Parse(new string[] { "--port", "8080" });

            Assert.Null(port.Value);
        }

        /// <summary>Tests parsing of missing optional parameter.</summary>
        [Fact(Timeout = 5000)]
        public void missingParameterTypeOptionalFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            port.ParameterType = ParameterType.Optional;
            parser.AddOption(port);

            parser.Parse(new string[] { "-p" });

            Assert.Null(port.Value);
        }

        /// <summary>Tests parsing of present optional parameter.</summary>
        [Fact(Timeout = 5000)]
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