using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class UnderscoreTag : MdTag
    {
        public UnderscoreTag() : base("_", new List<MdTag>())
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            var start = position;
            while (true)
            {
                position++;
                if (position == line.Length) return position;
                if (char.IsDigit(line, position)) return start;
                if (IsStartedPositionTagEnd(line, position))
                    return position;
            }
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            if (position == line.Length - 1) return false;
            return TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName) &&
                   !line[position + 1].ToString().Equals(@"_") &&
                   !line[position + 1].ToString().Equals(@" ");
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return TagHelper.IsSubstringEqualTag(line, position, TagName) &&
                   !line[position - 1].ToString().Equals(@" ") &&
                   TagHelper.IsNotTagEscaped(line, position);
        }
    }
}