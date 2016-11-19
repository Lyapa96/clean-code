using System.Collections.Generic;

namespace Markdown.Tags
{
    public class TagHelper
    {
        private const string Backslash = @"\";

        public static bool IsNotTagEscaped(string text, int position)
        {
            if (position == 0) return true;
            return !text[position - 1].ToString().Equals(Backslash);
        }

        public static bool IsSubstringEqualTag(string line, int position, string tagname)
        {
            if (position + tagname.Length > line.Length) return false;
            return line.Substring(position, tagname.Length).Equals(tagname);
        }

        public static MdTag DetermineCurrentTag(string line, int position, List<MdTag> mdTags)
        {
            foreach (var mdTag in mdTags)
            {
                if (mdTag.IsStartedPositionTagStart(line, position))
                    return mdTag;
            }
            return new EmptyTag();
        }

    }
}