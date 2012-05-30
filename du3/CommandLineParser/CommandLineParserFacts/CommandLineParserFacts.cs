using System;
using System.Collections.Generic;
using Xunit;
using CommandLine;
using System.IO;

/// <summary>Namespace for testing classes.</summary>
/// <remarks>Every class in this namespace has something to do with testing the CommandLineParser library.
/// Tests are written in XUnit framework, to run it type "xunit.console.clr4.exe CommandLineParserFacts.dll" (assuming you have xunit console directory in your path).
/// 4 out of 70 tests will fail. Namely:CommandLineParserFacts.OptionFacts.sameOptionShortNameThrowsException, CommandLineParserFacts.Parser.parseEmptyStringArgumentFact, CommandLineParserFacts.Parser.nullExpectedValueFact and CommandLineParserFacts.Parser.parseOneCharStringArgumentFact.</remarks>
namespace CommandLineParserFacts
{
    /// <summary>Class with tests of the parser.</summary>
    /// <remarks>Checks parsing problems, for example missing required options. It doesn't test individual options classes.</remarks>
    public class Parser
    {
        CommandLineParser parser;

        /// <summary>Restores output to System.Console.</summary>
        public Parser()
        {
            this.parser = new CommandLineParser();

            /* restore output to System.Console */
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        /// <summary>Tests printing help to standard output.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests printing help to custom TextWriter.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests printing empty help.</summary>
        [Fact(Timeout = 5000)]
        public void printEmptyHelpFact()
        {
            TextWriter helpTextWriter = new StringWriter();
            parser.PrintHelp(helpTextWriter);
            string expectedHelpText = string.Format("Options{0}", Environment.NewLine);

            Assert.Equal(expectedHelpText, helpTextWriter.ToString());
        }

        //TODO Resolve: Null ExpectedValue should print some default "value", but leads to NullReferenceException during PrintHelp().
        /// <summary>Tests printing help when ExpectedValue set to null.</summary>
        [Fact(Timeout = 5000)]
        public void nullExpectedValueFact()
        {
            CommandLineStringOption verbose = new CommandLineStringOption("format");
            verbose.ExpectedValue = null;
            parser.AddOption(verbose);

            StringWriter consoleOutputStringWriter = new StringWriter();
            Console.SetOut(consoleOutputStringWriter);
            parser.PrintHelp();

            string expectedHelpText = string.Format("Options{0}", Environment.NewLine);
            expectedHelpText += string.Format("\t--verbose value, --format=value{0}{0}", Environment.NewLine);

            Assert.Equal(expectedHelpText, consoleOutputStringWriter.ToString());
        }

        /// <summary>Tests parsing parameter set by long name and compact form with equals sign.</summary>
        [Fact(Timeout = 5000)]
        public void parseLongNameEqualsSignFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            port.ParameterType = ParameterType.Optional;
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "--port=8080" });
            Assert.Equal(8080, port.Value);
        }

        /// <summary>Tests parsing parameter containing equals sign set by long name and compact form with equals sign.</summary>
        [Fact(Timeout = 5000)]
        public void parseLongNameEqualsSignInParameterFormatFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            parser.AddOption(format);

            List<string> arguments = parser.Parse(new string[] { "--format=X=Y" });
            Assert.Equal("X=Y", format.Value);
        }

        /// <summary>Tests parsing parameter set by long name and separeted by space.</summary>
        [Fact(Timeout = 5000)]
        public void parseLongNameSpaceFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port");
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "--port", "8080" });
            Assert.Equal(8080, port.Value);
        }

        /// <summary>Tests parsing parameter set by short name and separeted by space.</summary>
        [Fact(Timeout = 5000)]
        public void parseShortNameSpaceFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "-p", "8080" });
            Assert.Equal(8080, port.Value);
        }

        /// <summary>Tests parsing parameter set by short name and compact form with no space after short name.</summary>
        [Fact(Timeout = 5000)]
        public void parseShortNameCompactFormatFact()
        {
            CommandLineIntOption port = new CommandLineIntOption("port", "p");
            parser.AddOption(port);

            List<string> arguments = parser.Parse(new string[] { "-p8080" });
            Assert.Equal(8080, port.Value);
        }

        /// <summary>Basic test when option is present by long name.</summary>
        [Fact(Timeout = 5000)]
        public void presentOptionNameFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            parser.Parse(new string[] { "--verbose" });

            Assert.True(verbose.Present);
        }

        /// <summary>Basic test when option not present.</summary>
        [Fact(Timeout = 5000)]
        public void notPresentOptionNameFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", "v");
            parser.AddOption(verbose);

            parser.Parse(new string[] { });

            Assert.False(verbose.Present);
        }

        /// <summary>Basic test when option is present by short name.</summary>
        [Fact(Timeout = 5000)]
        public void presentOptionShortNameFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose", "v");
            parser.AddOption(verbose);

            parser.Parse(new string[] { "-v" });

            Assert.True(verbose.Present);
        }

        /// <summary>Basic test when unknown option is parsed.</summary>
        [Fact(Timeout = 5000)]
        public void unknownLongOptionThrowsException()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version");
            parser.AddOption(version);
            
            Assert.Throws<CommandLine.ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--verbose" });
                });
        }

        /// <summary>Basic test when unknown shortname option is parsed.</summary>
        [Fact(Timeout = 5000)]
        public void unknownShortOptionThrowsException()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version");
            parser.AddOption(version);

            Assert.Throws<CommandLine.ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "-v" });
                });
        }

        /// <summary>Tests parsing of renamed option's long name.</summary>
        [Fact(Timeout = 5000)]
        public void renameOptionNameAfterAddingToParserFact()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version");
            parser.AddOption(version);
            version.Name = "verbose";
            
            parser.Parse(new string[] { "--verbose" });

            Assert.True(version.Present);
        }

        /// <summary>Tests parsing of renamed option's short name.</summary>
        [Fact(Timeout = 5000)]
        public void renameOptionShortNameAfterAddingToParserFact()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version", "v");
            parser.AddOption(version);
            version.ShortName = "X";

            parser.Parse(new string[] { "-X" });

            Assert.True(version.Present);
        }

        /// <summary>Tests renaming of option's short name.</summary>
        [Fact(Timeout = 5000)]
        public void addOptionWithExistingShortNameThrowsException()
        {
            CommandLineBoolOption version = new CommandLineBoolOption("version", "v");
            parser.AddOption(version);
            CommandLineStringOption format = new CommandLineStringOption("format", "f");
            parser.AddOption(format);
            Assert.Throws<CommandLine.ConfigurationException>(
            delegate
            {
                format.ShortName = "v";
            });
        }

        /// <summary>Tests parsing when required option is missing.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests parsing when required parameter is missing.</summary>
        [Fact(Timeout = 5000)]
        public void missingRequiredParameterByLongNameThrowsException()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            format.ParameterType = ParameterType.Required;
            parser.AddOption(format);

            Assert.Throws<CommandLine.ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "--format" });
                });
        }

        /// <summary>Tests parsing when required parameter is missing.</summary>
        [Fact(Timeout = 5000)]
        public void missingRequiredParameterByShortNameThrowsException()
        {
            CommandLineStringOption format = new CommandLineStringOption("format", "f");
            format.ParameterType = ParameterType.Required;
            parser.AddOption(format);

            Assert.Throws<CommandLine.ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { "-f" });
                });
        }

        /// <summary>Tests whether requried parameter gets assigned when in form of another optin.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests whether optional parameter gets assigned when in form of another optin.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests whether required parameter gets assigned when in form of the separator.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests whether optional parameter gets assigned when in form of the separator.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Basic arguments parsing test.</summary>
        [Fact(Timeout = 5000)]
        public void parseSomeArgumentsFact()
        {
            string[] inputArgs = new string[] { "arg1", "arg2", "arg3" };
            List<string> arguments = parser.Parse(inputArgs);

            Assert.Equal(inputArgs, arguments, new CollectionEquivalenceComparer<string>());
        }

        /// <summary>Basic arguments parsing test, users wrote path with spaces, but didn't enclose it with quotes.</summary>
        [Fact(Timeout = 5000)]
        public void parseInputWithSpacesFact()
        {
            CommandLineStringOption format = new CommandLineStringOption("format");
            parser.AddOption(format);

             /* user forgot some quotation marks... */
            List<string> arguments = parser.Parse(new string[] {"--format=Price", "of", "your", "ticket", "is", "%7d\n"});

            Assert.Equal(new string[] {"of", "your", "ticket", "is", "%7d\n"}, arguments, new CollectionEquivalenceComparer<string>());
        }

        /* fails to parse less than two characters long arguments */
        /// <summary>Tests parsing empty string as argument.</summary>
        [Fact(Timeout = 5000)]
        public void parseEmptyStringArgumentFact()
        {
            string[] inputArgs = new string[] { "" };
            List<string> arguments = parser.Parse(inputArgs);

            Assert.Equal(inputArgs, arguments, new CollectionEquivalenceComparer<string>());
        }

        /* fails to parse less than two characters long arguments */
        /// <summary>Tests parsing one char long string as argument.</summary>
        [Fact(Timeout = 5000)]
        public void parseOneCharStringArgumentFact()
        {
            string[] inputArgs = new string[] { "1" };
            List<string> arguments = parser.Parse(inputArgs);

            Assert.Equal(inputArgs, arguments, new CollectionEquivalenceComparer<string>());
        }

        /// <summary>Tests parsing null as option.</summary>
        [Fact(Timeout = 5000)]
        public void parseNullOptionThrowsException()
        {
            Assert.Throws<ParsingException>(
                delegate
                {
                    parser.Parse(new string[] { null });
                });
        }

        /// <summary>Tests parsing null as argument.</summary>
        [Fact(Timeout = 5000)]
        public void parseNullArgumentFact()
        {
            List<string> arguments = parser.Parse(new string[] { "--", null });

            Assert.Equal(new string[] { null }, arguments);
        }

        /// <summary>Tests case sensitivity parsing.</summary>
        [Fact(Timeout = 5000)]
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

        /// <summary>Tests parsing after separator was found.</summary>
        [Fact(Timeout = 5000)]
        public void parseSeparatorFact()
        {
            CommandLineBoolOption verbose = new CommandLineBoolOption("verbose");
            parser.AddOption(verbose);

            List<string> arguments = parser.Parse(new string[] { "--", "--version", "--verbose"});

            Assert.Equal(new string[] {"--version", "--verbose"}, arguments, new CollectionEquivalenceComparer<string>());
        }

    }

    /// <summary>Compares two collections by their content</summary>
    /// <remarks></remarks>
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