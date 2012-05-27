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
        public void boolOptionParameterTypeRequiredFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            verbose.ParameterType = ParameterType.Required;
            parser.AddOption(verbose);
            
            Assert.Throws<ParsingException>(
                delegate {
                    List<string> arguments = parser.Parse(new string[] { "--verbose=true"});
                });
            Assert.True(verbose.Present);
            Assert.Null(verbose.Value);
        }

        [Fact]
        public void boolOptionParameterTypeOptionalFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            verbose.ParameterType = ParameterType.Optional;
            parser.AddOption(verbose);

            Assert.Throws<ParsingException>(
                delegate
                {
                    List<string> arguments = parser.Parse(new string[] { "--verbose=true" });
                });
            Assert.True(verbose.Present);
            Assert.Null(verbose.Value);
        }
       
    }
}