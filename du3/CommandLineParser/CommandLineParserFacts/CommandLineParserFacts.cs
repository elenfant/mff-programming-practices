using System;
using System.Collections.Generic;
using Xunit;
using CommandLine;
using System.IO;

namespace CommandLineParserFacts
{

    public class CommandLineParserFacts
    {
        CommandLineParser parser;

        public CommandLineParserFacts()
        {
            this.parser = new CommandLineParser();

            /* restore output to System.Console */
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        [Fact]
        public void printHelpToConsoleFact()
        {
            StringWriter consoleOutputStringWriter = new StringWriter();
            Console.SetOut(consoleOutputStringWriter);

            CommandLineStringOption format = new CommandLineStringOption("format", "f");
            format.Help = "Specify output format, possibly overriding the format specified in the environment variable TIME.";
            format.ExpectedValue = "format";
            parser.AddOption(format);

            CommandLineBoolOption version = new CommandLineBoolOption("version", "v");
            version.Help = "Print version information on standard output, then exit successfully.";
            parser.AddOption(version);

            parser.PrintHelp();

            string expectedHelpText = string.Format("Options{0}", Environment.NewLine);
            expectedHelpText += string.Format("\t-f FORMAT, --format=FORMAT{0}\t\tSpecify output format, possibly overriding the format specified in the environment variable TIME.{0}{0}", Environment.NewLine);
            expectedHelpText += string.Format("\t-v, --version{0}\t\tPrint version information on standard output, then exit successfully.{0}{0}", Environment.NewLine);

            Assert.Equal(expectedHelpText, consoleOutputStringWriter.ToString());
        }

        [Fact]
        public void printHelpToTextWriterFact()
        {
            TextWriter helpTextWriter = new StringWriter();

            CommandLineStringOption format = new CommandLineStringOption("format", "f");
            format.Help = "Specify output format, possibly overriding the format specified in the environment variable TIME.";
            format.ExpectedValue = "format";
            parser.AddOption(format);

            CommandLineBoolOption version = new CommandLineBoolOption("version", "v");
            version.Help = "Print version information on standard output, then exit successfully.";
            parser.AddOption(version);

            parser.PrintHelp(helpTextWriter);

            string expectedHelpText = string.Format("Options{0}", Environment.NewLine);
            expectedHelpText += string.Format("\t-f FORMAT, --format=FORMAT{0}\t\tSpecify output format, possibly overriding the format specified in the environment variable TIME.{0}{0}", Environment.NewLine);
            expectedHelpText += string.Format("\t-v, --version{0}\t\tPrint version information on standard output, then exit successfully.{0}{0}", Environment.NewLine);

            Assert.Equal(expectedHelpText, helpTextWriter.ToString());
        }

        [Fact]
        public void printEmptyHelpFact()
        {
            TextWriter helpTextWriter = new StringWriter();
            parser.PrintHelp(helpTextWriter);
            string expectedHelpText = string.Format("Options{0}", Environment.NewLine);

            Assert.Equal(expectedHelpText, helpTextWriter.ToString());
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

        [Fact]
        public void parseLongNameEqualsSignFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "--port=8080" });
            Assert.Equal(8080, port.Value);
        }

        [Fact]
        public void parseLongNameSpaceFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "--port", "8080" });
            Assert.Equal(8080, port.Value);
        }

        [Fact]
        public void parseShortNameSpaceFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "-p", "8080" });
            Assert.Equal(8080, port.Value);
        }

        [Fact]
        public void parseShortNameCompactFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "-p8080" });
            Assert.Equal(8080, port.Value);
        }
        
        [Fact]
        public void presentOptionNameFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            parser.Parse(new string[] { "--verbose" });

            Assert.True(verbose.Present);
        }

        [Fact]
        public void notPresentOptionNameFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            parser.Parse(new string[] { });

            Assert.False(verbose.Present);
        }

        [Fact]
        public void presentOptionShortNameFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", "v");
            parser.AddOption(verbose);

            parser.Parse(new string[] { "-v" });

            Assert.True(verbose.Present);
        }

        [Fact]
        public void notPresentOptionShortNameFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", "v");
            parser.AddOption(verbose);
            
            parser.Parse(new string[] { });

            Assert.False(verbose.Present);
        }

        [Fact]
        public void unknownOptionThrowsException()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version");
            parser.AddOption(version);
            
            Assert.Throws<CommandLine.ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--verbose" });
                });
        }

        [Fact]
        public void renameOptionNameAfterAddingToParserFact()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version");
            parser.AddOption(version);
            version.Name = "verbose";
            
            parser.Parse(new string[] { "--verbose" });

            Assert.True(version.Present);
        }

        [Fact]
        public void renameOptionShortNameAfterAddingToParserFact()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version", "v");
            parser.AddOption(version);
            version.ShortName = "X";

            parser.Parse(new string[] { "-X" });

            Assert.True(version.Present);
        }

        [Fact]
        public void missingRequiredOptionThrowsException()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            format.Required = true;
            parser.AddOption(format);

            Assert.Throws<CommandLine.ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { });
                });
        }

        [Fact]
        public void missingRequiredParameterThrowsException()
        {
            CommandLineStringOption format = new CommandLineStringOption("verbose");
            format.ParameterType = ParameterType.Required;
            parser.AddOption(format);

            Assert.Throws<CommandLine.ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--verbose" });
                });
        }

        [Fact]
        public void requiredParameterEatsNextOptionFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            format.ParameterType = ParameterType.Required;
            parser.AddOption(format);
            
            CommandLineStringOption sdk = new CommandLineStringOption("sdk");
            sdk.AllowedValues.Add("4.0");
            sdk.ParameterType = ParameterType.Required;
            parser.AddOption(sdk);
            
            parser.Parse(new string[] { "--format", "--sdk", "4.0"});

            Assert.Equal("--sdk", format.Value);
        }

        [Fact]
        public void optionalParameterEatsNextOptionFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            format.ParameterType = ParameterType.Optional;
            parser.AddOption(format);
            
            CommandLineStringOption sdk = new CommandLineStringOption("sdk");
            sdk.AllowedValues.Add("4.0");
            sdk.ParameterType = ParameterType.Required;
            parser.AddOption(sdk);
            
            parser.Parse(new string[] { "--format", "--sdk", "4.0" });

            Assert.Equal("--sdk", format.Value);
        }

        [Fact]
        public void optionalParameterEatsSeparatorFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            format.ParameterType = ParameterType.Optional;
            parser.AddOption(format);

            CommandLineStringOption sdk = new CommandLineStringOption("sdk");
            parser.AddOption(sdk);

            parser.Parse(new string[] { "--format", "--", "--sdk", "4.0" });

            Assert.Equal("4.0", sdk.Value);
        }

        [Fact]
        public void requiredParameterEatsSeparatorFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            format.ParameterType = ParameterType.Required;
            parser.AddOption(format);

            CommandLineStringOption sdk = new CommandLineStringOption("sdk");
            parser.AddOption(sdk);

            parser.Parse(new string[] { "--format", "--", "--sdk", "4.0" });

            Assert.Equal("4.0", sdk.Value);
        }

        [Fact]
        public void parseSomeArgumentsFact()
        {
            string[] inputArgs = new string[] { "arg1", "arg2", "arg3" };
            List<string> arguments = parser.Parse(inputArgs);

            Assert.Equal(inputArgs, arguments, new CollectionEquivalenceComparer<string>());
        }

        [Fact]
        public void parseInputWithSpaces()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            parser.AddOption(format);

             /* user forgot some quotation marks... */
            List<string> arguments = parser.Parse(new string[] {"--format=Price", "of", "your", "ticket", "is", "%7d\n"});

            Assert.Equal(new string[] {"of", "your", "ticket", "is", "%7d\n"}, arguments, new CollectionEquivalenceComparer<string>());
        }

        /* fails to parse less than two characters long arguments */
        [Fact(Timeout = 5000)]
        public void parseEmptyStringArgumentFact()
        {
            string[] inputArgs = new string[] { "" };
            List<string> arguments = parser.Parse(inputArgs);

            Assert.Equal(inputArgs, arguments, new CollectionEquivalenceComparer<string>());
        }

        /* fails to parse less than two characters long arguments */
        [Fact(Timeout = 5000)]
        public void parseOneCharStringArgumentFact()
        {
            string[] inputArgs = new string[] { "1" };
            List<string> arguments = parser.Parse(inputArgs);

            Assert.Equal(inputArgs, arguments, new CollectionEquivalenceComparer<string>());
        }

        [Fact]
        public void parseNullOptionThrowsException()
        {
            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { null });
                });
        }

        [Fact]
        public void parseNullArgumentFact()
        {
            List<string> arguments = parser.Parse(new string[] { "--", null });

            Assert.Equal(new string[] { null }, arguments);
        }
        
        [Fact]
        public void parseCaseSensitiveOptionsThrowsException()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--VERBOSE" });
                });
        }

        [Fact]
        public void parseSeparatorFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            List<string> arguments = parser.Parse(new string[] { "--", "--version", "--verbose"});

            Assert.Equal(new string[] {"--version", "--verbose"}, arguments, new CollectionEquivalenceComparer<string>());
        }

    }

    public class CollectionEquivalenceComparer<T> : IEqualityComparer<IEnumerable<T>>
        where T : IEquatable<T>
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            List<T> leftList = new List<T>(x);
            List<T> rightList = new List<T>(y);
            leftList.Sort();
            rightList.Sort();

            IEnumerator<T> enumeratorX = leftList.GetEnumerator();
            IEnumerator<T> enumeratorY = rightList.GetEnumerator();

            while (true)
            {
                bool hasNextX = enumeratorX.MoveNext();
                bool hasNextY = enumeratorY.MoveNext();

                if (!hasNextX || !hasNextY)
                    return (hasNextX == hasNextY);

                if (!enumeratorX.Current.Equals(enumeratorY.Current))
                    return false;
            }
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            throw new NotImplementedException();
        }
    }
}