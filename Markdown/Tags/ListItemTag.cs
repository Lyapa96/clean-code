using System.Collections.Generic;

namespace Markdown.Tags
{
    public class ListItemTag : MdTag
    {
        public ListItemTag() : base("1. ", new List<MdTag>() {new UnderscoreTag(), new DoubleUnderscoreTag()})
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
            if (position >= line.Length - 2) return false;
            return (char.IsDigit(line, position) && line.Substring(position + 1, 2) == ". ");
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return position == line.Length;
        }
    }
}