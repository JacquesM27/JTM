using System.Text.RegularExpressions;

namespace FromQueueMailSender.UnitTests.Helper
{
    internal static class RegexHelper
    {
        internal static int CountOccurrences(this string input, string searchString)
        {
            Regex regex = new Regex(Regex.Escape(searchString));
            MatchCollection matches = regex.Matches(input);

            return matches.Count;
        }
    }
}
