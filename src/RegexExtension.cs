using System.Text.RegularExpressions;

namespace linq2md {
    public static class RegexExtension {
        public static string ValueAt(this Match match, int index) {
            if (index>=match.Groups.Count) {
                return "";
            }
            return match.Groups[index].Value;
        }
    }
}
