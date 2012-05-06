using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    class Printer
    {
        public const string DEFAULT_PARAMETER_NAME = "ARG";
        public const string FIRST_LEVEL_INDENT = "    ";
        public const string SECOND_LEVEL_INDENT = "        ";

        public static string FormatTextToPrint(string text, string indent, int width) {
            StringBuilder formattedText = new StringBuilder();
            int lineLength = 0;
            int indentLength = indent.Length;
            int startInsert = 0;
            formattedText.Append(indent);
            lineLength += indentLength;
            int wordLength = 0;
            
            for (int index = 0; index < text.Length; index++) {
                char c = text[index];
                if (char.IsWhiteSpace(c)) {
                    switch (c) {
                        case '\n':
                            formattedText.Append(text, startInsert, index - startInsert + 1);
                            startInsert = index + 1;
                            formattedText.Append(indent);
                            lineLength = indentLength;
                            wordLength = 0;
                            break;
                        case ' ':
                        default:
                            if (lineLength + wordLength > width) {
                                formattedText.Append(text, startInsert, index - startInsert - wordLength - 1);
                                formattedText.AppendLine();
                                startInsert = index - wordLength;
                                formattedText.Append(indent);
                                lineLength = indentLength;
                            }
                            else {
                                lineLength += wordLength;
                                ++lineLength;
                            }
                            wordLength = 0;
                            break;
                    }
                }
                else {
                    ++wordLength;
                }
            }
            lineLength += wordLength;
            if (lineLength > 0) {
                formattedText.Append(text, startInsert, text.Length - startInsert);
            }
            return formattedText.ToString();
        }


        public static string GetParameterName(string parameterName) {
            return parameterName != null ? parameterName : DEFAULT_PARAMETER_NAME;
        }
    }
}
