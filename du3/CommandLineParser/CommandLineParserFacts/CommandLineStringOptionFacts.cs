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
        public void stringOptionRemembersAllowedValues()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("scientific");
            view.AllowedValues.Add("programmer");
            view.AllowedValues.Add("standard");
            /* in different order */
            List<string> expectedAllowedValues = new List<string>(new string[] { "programmer", "scientific", "standard" });

            Assert.Equal(expectedAllowedValues, view.AllowedValues, new CollectionEquivalenceComparer<string>());
        }

        [Fact]
        public void stringOptionAllowedAnyFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            parser.AddOption(format);
            string expectedValue = "Value of N is %7d\n";
            List<string> arguments = parser.Parse(new string[] { "--format=" + expectedValue });
            Assert.Equal(expectedValue, format.Value);
        }

        [Fact]
        public void stringOptionAllowedFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("scientific");
            view.AllowedValues.Add("programmer");
            view.AllowedValues.Add("standard");
            parser.AddOption(view);

            List<string> arguments = parser.Parse(new string[] { "--view=standard" });
            Assert.Equal("standard", view.Value);
        }

        [Fact]
        public void stringOptionCaseSensitiveThrowsException()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("standard");
            parser.AddOption(view);

            Assert.Throws<ParsingException>(
                delegate
                {
                    List<string> arguments = parser.Parse(new string[] { "--view=STANDARD" });
                });
        }

        [Fact]
        public void stringOptionNotAllowedThrowsExcption()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("scientific");
            view.AllowedValues.Add("programmer");
            view.AllowedValues.Add("standard");
            parser.AddOption(view);

            Assert.Throws<ParsingException>(
                delegate
                {
                    List<string> arguments = parser.Parse(new string[] { "--view=statistics" });
                });
        }

        [Fact]
        public void stringOptionEmptyParameterAllowedFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add("");
            parser.AddOption(view);
            List<string> arguments = parser.Parse(new string[] { "--view=" });

            Assert.Equal("", view.Value);
        }

        [Fact]
        public void stringOptionNullParameterAllowedFact()
        {
            CommandLineStringOption view = new CommandLineStringOption("view");
            view.AllowedValues.Add(null);
            parser.AddOption(view);
            List<string> arguments = parser.Parse(new string[] { "--view", null });

            Assert.Null(view.Value);
        }

    }
}