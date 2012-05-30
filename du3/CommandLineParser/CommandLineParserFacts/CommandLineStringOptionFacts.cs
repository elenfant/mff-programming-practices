using System;
using System.Collections.Generic;
using Xunit;
using CommandLine;

namespace CommandLineParserFacts
{
    /// <summary>Class for testing usage of CommandLineStringOption class.</summary>
    public class StringOptionFacts
    {
        private CommandLineParser parser;

        public StringOptionFacts()
        {
            this.parser = new CommandLineParser();
        }

        /// <summary>Tests remembering of allowed parameters.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Basic test for parsing any parameter.</summary>
        [Fact(Timeout = 5000)]
        public void allowedAnyFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            parser.AddOption(format);
            string expectedValue = "Value of N is %7d\n";
            
            parser.Parse(new string[] { "--format=" + expectedValue });
            
            Assert.Equal(expectedValue, format.Value);
        }

        /// <summary>Basic test for parsing some allowed parameter.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests for case sensitivity of allowed parameters.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests of parsing not allowed parameters.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests of parsing empty allowed parameter.</summary>
        [Fact(Timeout = 5000)]
        public void emptyParameterAllowedFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("");
            parser.AddOption(view);
            
            parser.Parse(new string[] { "--view=" });

            Assert.Equal("", view.Value);
        }

        /// <summary>Tests of parsing null allowed parameter.</summary>
        [Fact(Timeout = 5000)]
        public void nullParameterAllowedFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            parser.AddOption(view);
         
            parser.Parse(new string[] { "--view", null });

            Assert.Null(view.Value);
        }

        /// <summary>Tests setting ParameterType to ParameterType.None.</summary>
        [Fact(Timeout = 5000)]
        public void parameterTypeNoneFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.ParameterType = ParameterType.None;
            parser.AddOption(view);

            parser.Parse(new string[] { "--view"});

            Assert.Null(view.Value);
        }

        /// <summary>Tests parsing of missing optional parameter.</summary>
        [Fact(Timeout = 5000)]
        public void missingParameterTypeOptionalFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.ParameterType = ParameterType.Optional;
            parser.AddOption(view);

            parser.Parse(new string[] { "--view" });

            Assert.Null(view.Value);
        }

        /// <summary>Tests parsing of present optional parameter.</summary>
        [Fact(Timeout = 5000)]
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