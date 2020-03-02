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
}
