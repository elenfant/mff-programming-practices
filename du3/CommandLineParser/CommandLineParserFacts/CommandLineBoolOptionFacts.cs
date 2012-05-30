using System;
using System.Collections.Generic;
using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{
    /// <summary>Class for testing usage of CommandLineBoolOption class.</summary>
    public class BoolOptionFacts
    {
        private CommandLineParser parser;

        public BoolOptionFacts()
        {
            this.parser = new CommandLineParser();
        }

        /// <summary>Value of present boolean option is always null.</summary>
        [Fact(Timeout = 5000)]
        public void presentValueAlwaysNull()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            parser.Parse(new string[] { "--verbose" });

            Assert.Null(verbose.Value);
        }

        /// <summary>Value of missing option is always null.</summary>
        [Fact(Timeout = 5000)]
        public void notPresentValueAlwaysNull()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            parser.Parse(new string[] { });

            Assert.Null(verbose.Value);
        }

        /// <summary>Setting parameter type to required for boolean option is nonsense.</summary>
        [Fact(Timeout = 5000)]
        public void parameterTypeRequiredThrowsException()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", "v");
            verbose.ParameterType = ParameterType.Required;
            parser.AddOption(verbose);
            
            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "-v", "true"});
                });
        }

        /// <summary>Setting parameter type to optional for boolean option is nonsense.</summary>
        [Fact(Timeout = 5000)]
        public void parameterTypeOptionalThrowsException()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", "v");
            verbose.ParameterType = ParameterType.Optional;
            parser.AddOption(verbose);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "-v", "true" });
                });
        }

        /// <summary>Setting parameter value of boolean option is nonsense.</summary>
        [Fact(Timeout = 5000)]
        public void presentParameterThrowsException()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--verbose=true" });
                });
        }

    }
}