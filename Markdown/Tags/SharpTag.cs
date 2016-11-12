using System.Collections.Generic;

namespace Markdown.Tags
{
    public class SharpTag : MdTag
    {
        public SharpTag() : base("#", new List<MdTag>())
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            while (true)
            {
                position++;
                if (position == line.Length) return position;
                if (IsStartedPositionTagEnd(line, position))
                    return position;
            }
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            if (position == line.Length - 1) return false;
            return TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName) &&
                   !line[position + 1].ToString().Equals(@"#");
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return TagHelper.IsSubstringEqualTag(line, position, TagName) &&
                   TagHelper.IsNotTagEscaped(line, position);
        }
    }
}