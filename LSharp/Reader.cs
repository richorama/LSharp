#region Copyright (c) 2008, Rob Blackwell.  All rights reserved.
// Software License Agreement (BSD License)

// Copyright (c) 2008, Rob Blackwell.  All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:

//   * Redistributions of source code must retain the above copyright
//     notice, this list of conditions and the following disclaimer.

//   * Redistributions in binary form must reproduce the above
//     copyright notice, this list of conditions and the following
//     disclaimer in the documentation and/or other materials
//     provided with the distribution.

// THIS SOFTWARE IS PROVIDED BY THE AUTHOR 'AS IS' AND ANY EXPRESSED
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LSharp
{
    /// <summary>
    /// An S-Expression reader suitable for use with Lisp-like languages.
    /// </summary>
    public class Reader
    {

        // Tokens
        private static Symbol UNQUOTE_SPLICING = Symbol.FromName("unquote-splicing");
        private static Symbol UNQUOTE = Symbol.FromName("unquote");
        private static Symbol QUASIQUOTE = Symbol.FromName("quasiquote");
        private static Symbol QUOTE = Symbol.FromName("quote");
        private static Symbol FN = Symbol.FromName("fn");
        public static object EOF = Symbol.FromName("eof");

        // TODO Are these needed - they arent returned?
        private static Symbol RPAREN = Symbol.FromName(")");
        private static Symbol DOT = Symbol.FromName(".");
        private static Symbol RBRACKET = Symbol.FromName("]");
        


        private static bool IsTerminator(char c)
        {
            bool b = (Char.IsWhiteSpace(c) || c == '\"' || c == ')' || c == '(' || c == '[' || c == ']');
            return b;
        }

        public static Object AtomReader(int c, TextReader textReader)
        {
            if (((Char)c) == ')')
            {
                return RPAREN;
            }

            if (((Char)c) == ']')
            {
                return RBRACKET;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append((char)c);
            bool done = false;

            do
            {
                int nextChar = textReader.Peek();


                if ((nextChar == -1) || (IsTerminator((Char)nextChar)))
                    done = true;
                else
                {
                    c = textReader.Read();
                    stringBuilder.Append((char)c);
                }

            } while (!done);

            string token = stringBuilder.ToString();

            Double d;

            // Try reading the number as an integer
            if (Double.TryParse(token, System.Globalization.NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out d))
            {
                return (int)d;
            }

            // Try reading the number as a double
            if (Double.TryParse(token, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out d))
            {
                return d;
            }
            else
            {
                return Symbol.FromName(token);
            }
        }


        public static void ReadWhiteSpace(TextReader textReader)
        {
            while (Char.IsWhiteSpace((Char)textReader.Peek()))
            {
                textReader.Read();
            }

        }

        public static Pair LParReader(TextReader textReader)
        {

            ReadWhiteSpace(textReader);

            object o = Read(textReader, EOF);

            if (o != RPAREN)
                return new Pair(o, LParReader(textReader));

            return null;

        }



        public static Object[] LBracketArrayReader(TextReader textReader)
        {
            
            ReadWhiteSpace(textReader);

            List<object> a = new List<object>();

            while (true)
            {
                object o = Read(textReader, EOF);
                if (o == RBRACKET)
                    return a.ToArray();

                a.Add(o);

            }

        }

        public static Pair LBracketReader(TextReader textReader)
        {
            int c;


            while (Char.IsWhiteSpace((Char)textReader.Peek()))
            {
                c = textReader.Read();
            }

            object o = Read(textReader, EOF);
            if (o != RBRACKET)
                return new Pair(o, LBracketReader(textReader));

            return null;

        }

        public static Object StringReader(TextReader textReader)
        {

            StringBuilder stringBuilder = new StringBuilder();
            bool done = false;
            int c;

            do
            {
                c = textReader.Read();

                if (c == -1) throw new Exception("EOF Reached");

                if (c == '\"') done = true;

                if (c == '\\')	// Deal with Escape characters ..
                {
                    c = textReader.Read();
                    if (c == -1)
                    {
                        throw new Exception("Read error - eof found before matching: \"");
                    }
                    switch (c)
                    {
                        case 't':
                            c = '\t';
                            break;
                        case 'r':
                            c = '\r';
                            break;
                        case 'n':
                            c = '\n';
                            break;
                        case '\\':
                            break;
                        case '"':
                            break;
                        default:
                            throw new Exception("Unsupported escape character: \\" + (Char)c);
                    }
                }

                if (!done)
                    stringBuilder.Append((char)c);

            } while (!done);

            return stringBuilder.ToString();
        }

        public static Object LineCommentReader(TextReader textReader)
        {
            int c;

            do
            {
                c = textReader.Read();
            } while (c != -1 && c != '\n' && c != '\r');

            return Read(textReader, EOF);
        }

        public  static Object QuoteReader(TextReader textReader)
        {
            return new Pair(QUOTE, new Pair(Read(textReader, EOF)));
        }

        public static Object QuasiQuoteReader(TextReader textReader)
        {
            return new Pair(QUASIQUOTE, new Pair(Read(textReader, EOF)));
        }

        public static Object UnQuoteReader(TextReader textReader)
        {
            if (textReader.Peek() == '@')
            {
                textReader.Read();
                return new Pair(UNQUOTE_SPLICING, new Pair(Read(textReader, EOF)));
            }
            return new Pair(UNQUOTE, new Pair(Read(textReader, EOF)));
        }


        public static Object CharacterReader(TextReader textReader)
        {
            int c;

            c = textReader.Read();

            return (char)c;
        }

        public static Object NamedCharacterReader(TextReader textReader)
        {
            int c;

            c = textReader.Read();

            string s = ((Symbol)AtomReader(c, textReader)).Name;

            if (s == "\\newline")
                return '\n';

            if (s == "\\space")
                return ' ';

            if (s == "\\tab")
                return '\t';

            // TODO: More character names here

            return (char)(s[1]);
        }

        public static Object MultiLineCommentReader(TextReader textReader)
        {
            int c;

            do
            {
                do
                {
                    c = textReader.Read();
                } while (c != -1 && c != '|');

                c = textReader.Read();
            } while (c != -1 & c != '#');

            return Read(textReader, EOF);

        }

        public static Object Read(TextReader textReader)
        {
            return Read(textReader, EOF);
        }

        public static Object Read(TextReader textReader, object eofValue)
        {
            int c = textReader.Read();


            while (Char.IsWhiteSpace((Char)c))
            {
                c = textReader.Read();
            }

            if (c == -1)
            {
                // End of file
                return eofValue;
            }

            if (c == '(')
            {
                return LParReader(textReader);
            }

            if (c == '[')
            {
 
                return LBracketArrayReader(textReader);

                // If you want Scheme like functionality, do this instead
                //return LBracketReader(textReader);
            }

            if (c == '\"')
            {
                return StringReader(textReader);
            }

            if (c == ';')
            {
                return (LineCommentReader(textReader));
            }
  
            if (c == '\'')
            {
                return (QuoteReader(textReader));
            }

            // Deal with quasiquote / backquote syntax
            if (c == '`')
            {
                return (QuasiQuoteReader(textReader));
            }

            // Deal with unquote and unquote-splicing with quaiquote syntax
            if (c == ',')
            {
                return (UnQuoteReader(textReader));
            }

            if (c == '|')
            {
                return (MultiLineCommentReader(textReader));
            }

            if (c == '\\')
            {
                return (CharacterReader(textReader));
            }

            if (c == '#')
            {
                return (NamedCharacterReader(textReader));
            }

            return AtomReader(c, textReader);
            
        }

        

        
    }
}
