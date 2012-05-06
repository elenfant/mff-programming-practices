using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionLib
{
    class Printer
    {
        public const string FIRST_LEVEL_INDENT = "    ";
        public const string SECOND_LEVEL_INDENT = "        ";

        public static string FormatTextToPrint(string text, string indent, int width) {
            StringBuilder formattedText = new StringBuilder();
            int lineLenght = 0;
            int indentLenght = indent.Length;
            int startInsert = 0;
            formattedText.Append(indent);
            lineLenght += indentLenght;
            int wordLenght = 0;
            
            for (int index = 0; index < text.Length; index++) {
                char c = text[index];
                if (char.IsWhiteSpace(c)) {
                    switch (c) {
                        case '\n':
                            formattedText.Append(text, startInsert, index - startInsert + 1);
                            startInsert = index + 1;
                            formattedText.Append(indent);
                            lineLenght = indentLenght;
                            wordLenght = 0;
                            break;
                        //case '\t':
                        //    if (lineLenght + indentLenght < width) {
                        //        int appendLenght = index - startInsert;
                        //        formattedText.Append(text, startInsert, appendLenght);
                        //        lineLenght += appendLenght;
                        //        formattedText.Append(indent);
                        //        lineLenght += indentLenght;
                        //        startInsert = index + 1;
                        //    }
                        //    else {
                        //        goto case ' ';
                        //    }
                        //    break;
                        case ' ':
                            if (lineLenght + wordLenght > width) {
                                formattedText.Append(text, startInsert, index - startInsert - wordLenght - 1);
                                formattedText.AppendLine();
                                startInsert = index - wordLenght;
                                formattedText.Append(indent);
                                lineLenght = indentLenght;
                            }
                            else {
                                lineLenght += wordLenght;
                                ++lineLenght;
                            }
                            wordLenght = 0;
                            break;
                    }
                }
                else {
                    ++wordLenght;
                }
            }
            lineLenght += wordLenght;
            if (lineLenght > 0) {
                formattedText.Append(text, startInsert, text.Length - startInsert);
            }
            return formattedText.ToString();
        }
           
    }
}
