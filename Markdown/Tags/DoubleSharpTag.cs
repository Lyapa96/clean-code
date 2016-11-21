using System.Collections.Generic;

namespace Markdown.Tags
{
    public class DoubleSharpTag : MdTag
    {
        public DoubleSharpTag() : base("## ", new List<MdTag>() { new UnderscoreTag(), new DoubleUnderscoreTag() })
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            while (true)
            {
                position++;
                if (IsStartedPositionTagEnd(line, position))
                    return position;
            }
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            return position == 0 &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName);
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return position == line.Length;
        }
    }
}