using System;

namespace FunctionalX
{
    using static Functional;
    public static class StringExtensions
    {
        /// <summary>
        /// This function trys to parse a string to an integer.
        /// If succeeds, it then returns the integer wrapped in a Maybe otherwise Nothing.
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <returns></returns>
        public static Maybe<int> ParseInt(this string s)
        {
            int result;
            return int.TryParse(s, out result)
                ? Just(result)
                : Nothing;
        }

        /// <summary>
        /// Parses the string into an integer. If failed, returns default value
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <param name="default">Default value if parsing fails</param>
        /// <returns></returns>
        public static int ParseInt(this string s, int @default)
            => FromMaybe(s.ParseInt(), @default);

        /// <summary>
        /// This function parses a string to a double.
        /// If succeeds, it then returns the double wrapped in a Maybe struct otherwise Nothing.
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <returns></returns>
        public static Maybe<double> ParseDouble(this string s)
        {
            double result;
            return double.TryParse(s, out result)
                ? Just(result)
                : Nothing;
        }

        /// <summary>
        /// Parses the string into a double. If failed, returns default value
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <param name="default">Default value if parsing fails</param>
        /// <returns></returns>
        public static double ParseDouble(this string s, double @default)
            => FromMaybe(s.ParseDouble(), @default);

        /// <summary>
        /// This function parses a string to a float.
        /// If succeeds, it then returns the float wrapped in a Maybe struct otherwise Nothing.
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <returns></returns>
        public static Maybe<float> ParseFloat(this string s)
        {
            float result;
            return float.TryParse(s, out result)
                ? Just(result)
                : Nothing;
        }

        /// <summary>
        /// Parses the string into a float. If failed, returns default value
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <param name="default">Default value if parsing fails</param>
        /// <returns></returns>
        public static float ParseFloat(this string s, float @default)
            => FromMaybe(s.ParseFloat(), @default);

        /// <summary>
        /// This function parses a string to a float.
        /// If succeeds, it then returns the float wrapped in a Maybe struct otherwise Nothing.
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <returns></returns>
        public static Maybe<decimal> ParseDecimal(this string s)
        {
            decimal result;
            return decimal.TryParse(s, out result)
                ? Just(result)
                : Nothing;
        }

        /// <summary>
        /// Parses the string into a decimal. If failed, returns default value
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <param name="default">Default value if parsing fails</param>
        /// <returns></returns>
        public static decimal ParseDecimal(this string s, decimal @default)
            => FromMaybe(s.ParseDecimal(), @default);

        /// <summary>
        /// Parses the string into a DateTime.
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <returns></returns>
        public static Maybe<DateTime> ParseDate(this string s)
        {
            DateTime d;
            return DateTime.TryParse(s, out d)
                ? Just(d)
                : Nothing;
        }

        /// <summary>
        /// Parses the string into a date. If failed, returns default value
        /// </summary>
        /// <param name="s">String to be parsed</param>
        /// <param name="default">Default value if parsing fails</param>
        /// <returns></returns>
        public static DateTime ParseDate(this string s, DateTime @default)
            => FromMaybe(s.ParseDate(), @default);
    }
}
