using System.Collections.Generic;

namespace Markdown.Tags
{
    public class CodeTag : MdTag
    {
        public CodeTag() : base("    ", new List<MdTag>())
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
            if (position != 0) return false;
            return line.Substring(0, 4) == TagName;
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return position == line.Length-1 + TagName.Length;
        }
    }
}