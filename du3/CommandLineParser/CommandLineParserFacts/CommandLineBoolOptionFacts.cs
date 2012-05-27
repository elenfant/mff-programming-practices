using System;
using System.Collections.Generic;
using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{
    public class CommandLineBoolOptionFacts
    {
        private CommandLineParser parser;

        public CommandLineBoolOptionFacts()
        {
            this.parser = new CommandLineParser();
        }

        [Fact]
        public void presentValueAlwaysNull()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            parser.Parse(new string[] { "--verbose" });

            Assert.Null(verbose.Value);
        }

        [Fact]
        public void notPresentValueAlwaysNull()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            parser.Parse(new string[] { });

            Assert.Null(verbose.Value);
        }

        [Fact]
        public void parameterTypeRequiredFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            verbose.ParameterType = ParameterType.Required;
            parser.AddOption(verbose);
            
            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--verbose=true"});
                });
        }

        [Fact]
        public void parameterTypeOptionalFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            verbose.ParameterType = ParameterType.Optional;
            parser.AddOption(verbose);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--verbose", "true" });
                });
        }
       
    }
}