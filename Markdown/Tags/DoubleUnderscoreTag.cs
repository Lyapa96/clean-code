using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class DoubleUnderscoreTag : IMdTag
    {
        public string TagName => "__";

        public int FindTagEnd(string line, int position)
        {
            while (true)
            {
                position++;
                if (position == line.Length - 1) return position + 1;
                if (IsStartedPositionTagEnd(line, position)) return position + 1;
            }
        }

        public bool IsStartedPositionTagStart(string line, int position)
        {
            return TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName);
        }

        public bool IsStartedPositionTagEnd(string line, int position)
        {
            return TagHelper.IsSubstringEqualTag(line, position, TagName) &&
                   TagHelper.IsNotTagEscaped(line, position);
        }

        public List<IMdTag> GetInnerTags => new List<IMdTag>() {new UnderscoreTag()};
    }
}