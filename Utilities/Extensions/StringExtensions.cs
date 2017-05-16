using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MonoCross.Utilities
{
    /// <summary>
    /// Represents methods for manipulating strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Appends the specified path and returns the result.
        /// </summary>
        /// <param name="basePath">The path to be appended to.</param>
        /// <param name="relativePath">The path to append.</param>
        public static string AppendPath(this string basePath, string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return basePath;

            char[] chars = new char[2];
            chars[0] = '/';
            chars[1] = '\\';

            if (string.IsNullOrEmpty(basePath))
            {
                //Match relative path to Device standard
                relativePath = relativePath.TrimStart(chars);
                return chars.Aggregate(relativePath, (current, separator) => current.Replace(separator, Device.DirectorySeparatorChar));
            }

            //Match relative path to base
            int index = basePath.Contains("/") || Device.DirectorySeparatorChar == '/' ? 0 : 1;
            return (basePath.TrimEnd(chars) + chars[index] + relativePath.TrimStart(chars)).Replace(chars[1 - index], chars[index]);
        }

        /// <summary>
        /// Returns a lowercase representation or an empty string if null.
        /// </summary>
        /// <param name="str">The string to get a lowercase representation of.</param>
        public static string Clean(this string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str.ToLower();
        }

        /// <summary>
        /// Changes the first character to uppercase and the rest of the characters to lowercase, then returns the result.
        /// </summary>
        /// <param name="str">The string to modify.</param>
        public static string ToTitleCase(this string str)
        {
            return str == null ? null : Regex.Replace(str, @"\w+", m =>
            {
                string tmp = m.Value;
                return char.ToUpper(tmp[0]) + tmp.Substring(1, tmp.Length - 1).ToLower();
            });
        }

        /// <summary>
        /// A "String.IsNullOrWhiteSpace()" implementation for pre-.NET 4.0 projects
        /// </summary>
        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str.Trim());
        }

        /// <summary>
        /// Returns a value indicating whether the string is a path to a remote resource.
        /// </summary>
        /// <returns><c>true</c> if the string is a path to a remote resource; otherwise, <c>false</c>.</returns>
        public static bool IsRemotePath(this string str)
        {
            return str != null && (str.StartsWith("http") || str.StartsWith("ftp"));
        }

        /// <summary>
        /// Replaces certain characters with their equivalent HTML representations and returns the result.
        /// </summary>
        /// <param name="str">The string to replace the characters of.</param>
        public static string CleanEntities(this string str)
        {
            return str == null ? null : str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>");
        }

        /// <summary>
        /// Removes the leading slash from a string.
        /// </summary>
        /// <param name="value">The string value to process.</param>
        /// <returns></returns>
        public static string RemoveLeadingSlash(this string value)
        {
            if (value == null)
                return null;

            // remove starting / or \ to enforce relative path.
            return (value.StartsWith(@"\") || value.StartsWith(@"/")) ? value.Substring(1) : value;
        }

        /// <summary>
        /// Removes the trailing slash.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string without a trailing slash.</returns>
        public static string RemoveTrailingSlash(this string value)
        {
            if (value == null)
                return null;

            // remove starting / or \ to enforce relative path.
            return (value.EndsWith(@"\") || value.EndsWith(@"/")) ? value.Substring(0, value.Length - 1) : value;
        }
        #region TryParse String Extensions

#if !NETCF
        /// <summary>
        /// Attempts to parse the string into a 32-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static int TryParseInt32(this string value)
        {
            return TryParse<int>(value, int.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a 32-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static int TryParseInt32(this string value, int defaultValue)
        {
            return TryParse<int>(value, int.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 16-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Int16 TryParseInt16(this string value)
        {
            return TryParse<Int16>(value, Int16.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a 16-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Int16 TryParseInt16(this string value, Int16 defaultValue)
        {
            return TryParse<Int16>(value, Int16.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Int64 TryParseInt64(this string value)
        {
            return TryParse<Int64>(value, Int64.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Int64 TryParseInt64(this string value, Int64 defaultValue)
        {
            return TryParse<Int64>(value, Int64.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 8-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static byte TryParseByte(this string value)
        {
            return TryParse<byte>(value, byte.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a 8-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static byte TryParseByte(this string value, byte defaultValue)
        {
            return TryParse<byte>(value, byte.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a boolean and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static bool TryParseBoolean(this string value)
        {
            return TryParseBoolean(value, false);
        }

        /// <summary>
        /// Attempts to parse the string into a boolean and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static bool TryParseBoolean(this string value, bool defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            if (value.Equals("on", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (value.Equals("off", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return TryParse<bool>(value, bool.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 32-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Single TryParseSingle(this string value)
        {
            return TryParse<Single>(value, Single.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a 32-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Single TryParseSingle(this string value, Single defaultValue)
        {
            return TryParse<Single>(value, Single.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Double TryParseDouble(this string value)
        {
            return TryParse<Double>(value, Double.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Double TryParseDouble(this string value, Double defaultValue)
        {
            return TryParse<Double>(value, Double.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a decimal number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Decimal TryParseDecimal(this string value)
        {
            return TryParse<Decimal>(value, Decimal.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a decimal number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Decimal TryParseDecimal(this string value, Decimal defaultValue)
        {
            return TryParse<Decimal>(value, Decimal.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static DateTime TryParseDateTime(this string value)
        {
            return TryParse<DateTime>(value, DateTime.TryParse);
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static DateTime TryParseDateTime(this string value, DateTime defaultValue)
        {
            return TryParse<DateTime>(value, DateTime.TryParse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object set to UTC and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static DateTime TryParseDateTimeUtc(this string value)
        {
            return TryParse<DateTime>(value, DateTime.TryParse).ToUniversalTime();
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object set to UTC and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static DateTime TryParseDateTimeUtc(this string value, DateTime defaultValue)
        {
            return TryParse<DateTime>(value, DateTime.TryParse, defaultValue).ToUniversalTime();
        }
#else
        /// <summary>
        /// Removes the remainder of the string starting at the index.
        /// </summary>
        /// <param name="value">The string to remove characters from</param>
        /// <param name="index">Index position in the string to start removing characters from</param>
        /// <returns></returns>
        public static string Remove(this string value, int index)
        {
            if (index > value.Length) { throw new IndexOutOfRangeException("String length is less than index value"); }
            return value.Remove(index, value.Length - index);
        }

        /// <summary>
        /// Attempts to parse the string into a 32-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static int TryParseInt32(this string value)
        {
            return TryParseInt32(value, Int32.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 32-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static int TryParseInt32(this string value, int defaultValue)
        {
            return TryParse<int>(value, int.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 16-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Int16 TryParseInt16(this string value)
        {
            return TryParseInt16(value, Int16.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 16-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Int16 TryParseInt16(this string value, Int16 defaultValue)
        {
            return TryParse<Int16>(value, Int16.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Int64 TryParseInt64(this string value)
        {
            return TryParseInt64(value, Int64.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Int64 TryParseInt64(this string value, Int64 defaultValue)
        {
            return TryParse<Int64>(value, Int64.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 8-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static byte TryParseByte(this string value)
        {
            return TryParseByte(value, byte.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 8-bit signed integer and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static byte TryParseByte(this string value, byte defaultValue)
        {
            return TryParse<byte>(value, byte.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a boolean and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static bool TryParseBoolean(this string value)
        {
            return TryParseBoolean(value, false);
        }

        /// <summary>
        /// Attempts to parse the string into a boolean and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static bool TryParseBoolean(this string value, bool defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            if (value.Equals("on", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (value.Equals("off", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                 value.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return TryParse<bool>(value, bool.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 32-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Single TryParseSingle(this string value)
        {
            return TryParseSingle(value, Single.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 32-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Single TryParseSingle(this string value, Single defaultValue)
        {
            return TryParse<Single>(value, Single.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Double TryParseDouble(this string value)
        {
            return TryParseDouble(value, Double.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a 64-bit floating point number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Double TryParseDouble(this string value, Double defaultValue)
        {
            return TryParse<Double>(value, Double.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a decimal number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static Decimal TryParseDecimal(this string value)
        {
            return TryParseDecimal(value, Decimal.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a decimal number and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static Decimal TryParseDecimal(this string value, Decimal defaultValue)
        {
            return TryParse<Decimal>(value, Decimal.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static DateTime TryParseDateTime(this string value)
        {
            return TryParseDateTime(value, DateTime.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static DateTime TryParseDateTime(this string value, DateTime defaultValue)
        {
            return TryParse<DateTime>(value, DateTime.Parse, defaultValue);
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object set to UTC and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        public static DateTime TryParseDateTimeUtc(this string value)
        {
            return TryParseDateTimeUtc(value, DateTime.MinValue);
        }

        /// <summary>
        /// Attempts to parse the string into a <see cref="DateTime"/> object set to UTC and returns the parsed value.
        /// </summary>
        /// <param name="value">The string to attempt to parse.</param>
        /// <param name="defaultValue">The value to return if parsing fails.</param>
        public static DateTime TryParseDateTimeUtc(this string value, DateTime defaultValue)
        {
            return TryParse(value, DateTime.Parse, defaultValue).ToUniversalTime();
        }
#endif

        #region Private Members

        private delegate bool TryParseDelegate<T>(string s, out T result);
        private delegate T ParseDelegate<T>(string s);

        private static T TryParse<T>(this string value, TryParseDelegate<T> parse) where T : struct
        {
            T result;
            parse(value, out result);
            return result;
        }

        private static T TryParse<T>(this string value, ParseDelegate<T> parse) where T : struct
        {
            return value.TryParse(parse, default(T));
        }

        private static T TryParse<T>(this string value, ParseDelegate<T> parse, T defaultValue) where T : struct
        {
            T result;

            try
            {
                result = parse(value);
            }
            catch (Exception)
            {
                result = defaultValue;
            }

            return result;
        }

        private static T TryParse<T>(this string value, TryParseDelegate<T> parse, T defaultValue) where T : struct
        {
            T result;
            return parse(value, out result) ? result : defaultValue;
        }

        #endregion

        #endregion

        /// <summary>
        /// Returns the substring after the value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <returns>The substring after the value/</returns>
        public static string SubstringAfter(this string source, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return source;
            }
            int index = source.IndexOf(value);
            if (index < 0)
            {
                //No such substring
                return string.Empty;
            }
            return source.Substring(index + value.Length);
        }

        /// <summary>
        /// Returns the substring before the value provided.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="value">The string value to search for.</param>
        /// <returns></returns>
        public static string SubstringBefore(this string source, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            int index = source.IndexOf(value);
            if (index < 0)
            {
                //No such substring
                return string.Empty;
            }
            return source.Substring(0, index);
        }

        /// <summary>
        /// Encodes the parameter.
        /// </summary>
        /// <param name="parameterToEncode">The parameter to encode.</param>
        /// <returns></returns>
        public static string EncodeParameter(this string parameterToEncode)
        {
            return UrlHelper.Encode(parameterToEncode);
        }

        /// <summary>
        /// Decodes the parameter.
        /// </summary>
        /// <param name="parameterToDecode">The parameter to decode.</param>
        /// <returns></returns>
        public static string DecodeParameter(this string parameterToDecode)
        {
            return UrlHelper.Decode(parameterToDecode);
        }

#if NETCF
        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited by elements of a specified string array. A parameter specifies whether to return empty array elements.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiters">An array of chars that delimit the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more strings in separator.</returns>
        public static string[] Split(this string str, char[] delimiters, StringSplitOptions options)
        {
            return str.Split(delimiters);
        }

        /// <summary>
        /// Returns a string array that contains the substrings in this string that are delimited by elements of a specified string array. A parameter specifies whether to return empty array elements.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="separator">An array of strings that delimit the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more strings in separator.</returns>
        public static string[] Split(this string str, string[] separator, StringSplitOptions options)
        {
            if ((options != StringSplitOptions.None) && (options != StringSplitOptions.RemoveEmptyEntries))
                throw new ArgumentException("Illegal enum value: " + options + ".");

            bool removeEmpty = (options & StringSplitOptions.RemoveEmptyEntries) != 0;

            if (separator == null || separator.Length == 0)
                throw new ArgumentException("Separator list is null.");

            if (str.Length == 0 && removeEmpty)
                return new string[0];

            var arr = new List<String>();

            int pos = 0;
            int matchCount = 0;
            while (pos < str.Length)
            {
                int matchIndex = -1;
                int matchPos = Int32.MaxValue;

                // Find the first position where any of the separators matches
                for (int i = 0; i < separator.Length; ++i)
                {
                    string sep = separator[i];
                    if (string.IsNullOrEmpty(sep))
                        continue;

                    int match = str.IndexOfOrdinalUnchecked(sep, pos, str.Length - pos);
                    if (match >= 0 && match < matchPos)
                    {
                        matchIndex = i;
                        matchPos = match;
                    }
                }

                if (matchIndex == -1)
                    break;

                if (!(matchPos == pos && removeEmpty))
                {
                    if (arr.Count == Int32.MaxValue - 1)
                        break;
                    arr.Add(str.Substring(pos, matchPos - pos));
                }

                pos = matchPos + separator[matchIndex].Length;

                matchCount++;
            }

            if (matchCount == 0)
                return new[] { str };

            // string contained only separators
            if (removeEmpty && matchCount != 0 && pos == str.Length && arr.Count == 0)
                return new string[0];

            if (!(removeEmpty && pos == str.Length))
                arr.Add(str.Substring(pos));

            return arr.ToArray();
        }

        internal static unsafe int IndexOfOrdinalUnchecked(this string str, string value, int startIndex, int count)
        {
            int valueLen = value.Length;
            if (count < valueLen)
                return -1;

            if (valueLen <= 1)
            {
                if (valueLen == 1)
                    return str.IndexOf(value[0], startIndex, count);
                return startIndex;
            }

            fixed (char* thisptr = str, valueptr = value)
            {
                char* ap = thisptr + startIndex;
                char* thisEnd = ap + count - valueLen + 1;
                while (ap != thisEnd)
                {
                    if (*ap == *valueptr)
                    {
                        for (int i = 1; i < valueLen; i++)
                        {
                            if (ap[i] != valueptr[i])
                                goto NextVal;
                        }
                        return (int)(ap - thisptr);
                    }
                NextVal:
                    ap++;
                }
            }
            return -1;
        }
#endif
        /// <summary>
        /// Contains Utility Url helper methods.
        /// </summary>
        public static class UrlHelper
        {
            /// <summary>
            /// URI Encodes the specified string.
            /// </summary>
            /// <param name="str">The string.</param>
            /// <returns></returns>
            public static string Encode(string str)
            {
                var charClass = String.Format("0-9a-zA-Z{0}", Regex.Escape("-_.!~*'()"));
                return Regex.Replace(str,
                    String.Format("[^{0}]", charClass),
                    EncodeEvaluator);
            }

            /// <summary>
            /// URI Decodes the specified string.
            /// </summary>
            /// <param name="str">The STR.</param>
            /// <returns></returns>
            public static string Decode(string str)
            {
                return Regex.Replace(str.Replace('+', ' '), "%[0-9a-zA-Z][0-9a-zA-Z]", DecodeEvaluator);
            }

            /// <summary>
            /// Encodes the evaluator.
            /// </summary>
            /// <param name="match">The match.</param>
            /// <returns></returns>
            static string EncodeEvaluator(Match match)
            {
                return (match.Value == " ") ? "+" : String.Format("%{0:X2}", Convert.ToInt32(match.Value[0]));
            }

            /// <summary>
            /// Decodes the evaluator.
            /// </summary>
            /// <param name="match">The match.</param>
            /// <returns></returns>
            static string DecodeEvaluator(Match match)
            {
                return Convert.ToChar(int.Parse(match.Value.Substring(1), NumberStyles.HexNumber)).ToString();
            }
        }
    }

#if NETCF
    /// <summary>
    /// Specifies whether applicable <see cref="String.Split"/> method overloads include or omit empty substrings from the return value.
    /// This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum StringSplitOptions
    {
        /// <summary>
        /// The return value includes array elements that contain an empty string
        /// </summary>
        None,

        /// <summary>
        /// The return value does not include array elements that contain an empty string
        /// </summary>
        RemoveEmptyEntries,
    }
#endif
}
