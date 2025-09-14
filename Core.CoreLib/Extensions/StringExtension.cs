
using System.Globalization;
using System.Text.RegularExpressions;

namespace Core.CoreLib.Extensions
{
    public static class StringExtension
    {
        public static string RegExMatchAndReplace(this string baseString, string replaceStart, string replaceEnd, string regEx)
        {
            return
            (new Regex($"{replaceStart}{regEx}")
                .Matches(baseString)
                .Cast<Match>()
                .Select(m => m.Value)
                .FirstOrDefault() ?? string.Empty)
                .Replace(replaceStart, string.Empty)
                .Replace(replaceEnd, string.Empty);
        }

        public static string FindBetweenTwoStrings(this string baseString, string start, string end)
        {
            var startPosition  = baseString.IndexOf(start) + start.Length;
            var endPosition = baseString.IndexOf(end, startPosition);

            return
                end == string.Empty ? 
                    baseString.Substring(startPosition) :
                    baseString.Substring(startPosition, endPosition - startPosition);

        }

        public static bool ContainsOnlyAllowedCharacters(this string value) =>
            new Regex(@"^[a-zA-Z0-9&\-.,–\s]+$").IsMatch(value.ToString());

        public static bool ContainsNumbersOnly(this string value) =>
            new Regex(@"^[0-9]+$").IsMatch(value.ToString());

        public static string ConvertUTCDateTimeStringToEST(this string dtUtcString)
        {
            var easternDateTime =
                TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.Parse(dtUtcString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            return
                easternDateTime.ToString();

        }

        //I hate this name, someone name it better
        public static string? ScrubForDB(this string? dbData) =>
            string.IsNullOrEmpty(dbData) ||
            string.IsNullOrWhiteSpace(dbData) ?
                null :
                dbData;
    }
}
