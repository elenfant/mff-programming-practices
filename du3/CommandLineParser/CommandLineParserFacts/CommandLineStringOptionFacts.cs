using System;
using System.Collections.Generic;
using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{
    public class CommandLineStringOptionFacts
    {
        private CommandLineParser parser;

        public CommandLineStringOptionFacts()
        {
            this.parser = new CommandLineParser();
        }

        [Fact]
        public void remembersAllowedValues()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("scientific");
            view.AllowedValues.Add("programmer");
            view.AllowedValues.Add("standard");
            /* in different order */
            string[] expectedAllowedValues = new string[] { "programmer", "scientific", "standard" };

            Assert.Equal(expectedAllowedValues, view.AllowedValues, new CollectionEquivalenceComparer<string>());
        }

        [Fact]
        public void allowedAnyFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            parser.AddOption(format);
            string expectedValue = "Value of N is %7d\n";
            
            parser.Parse(new string[] { "--format=" + expectedValue });
            
            Assert.Equal(expectedValue, format.Value);
        }

        [Fact]
        public void allowedSomeFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("scientific");
            view.AllowedValues.Add("programmer");
            view.AllowedValues.Add("standard");
            parser.AddOption(view);

            parser.Parse(new string[] { "--view=standard" });
            
            Assert.Equal("standard", view.Value);
        }

        [Fact]
        public void caseSensitiveThrowsException()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("standard");
            parser.AddOption(view);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--view=STANDARD" });
                });
        }

        [Fact]
        public void notAllowedThrowsException()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("scientific");
            view.AllowedValues.Add("programmer");
            view.AllowedValues.Add("standard");
            parser.AddOption(view);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--view=statistics" });
                });
        }

        [Fact]
        public void emptyParameterAllowedFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("");
            parser.AddOption(view);
            
            parser.Parse(new string[] { "--view=" });

            Assert.Equal("", view.Value);
        }

        [Fact]
        public void nullParameterAllowedFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            parser.AddOption(view);
         
            parser.Parse(new string[] { "--view", null });

            Assert.Null(view.Value);
        }

        [Fact]
        public void parameterTypeNoneFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.ParameterType = ParameterType.None;
            parser.AddOption(view);

            parser.Parse(new string[] { "--view"});

            Assert.Null(view.Value);
        }

        [Fact]
        public void missingParameterTypeOptionalFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.ParameterType = ParameterType.Optional;
            parser.AddOption(view);

            parser.Parse(new string[] { "--view" });

            Assert.Null(view.Value);
        }

        [Fact]
        public void parameterTypeOptionalFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.ParameterType = ParameterType.Optional;
            parser.AddOption(view);

            parser.Parse(new string[] { "--view", "standard" });

            Assert.Equal("standard", view.Value);
        }

    }
}