using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown
{
    public class TagHelper
    {
        public static bool IsNotTagEscaped(string text, int position)
        {
            if (position == 0) return true;
            return !text[position - 1].ToString().Equals(@"\");
        }

        public static bool IsSubstringEqualTag(string line, int position, string tagname)
        {
            if (position + tagname.Length > line.Length) return false;
            return line.Substring(position, tagname.Length).Equals(tagname);
        }

        public static IMdTag DetermineCurrentTag(string line, int position, List<IMdTag> mdTags)
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