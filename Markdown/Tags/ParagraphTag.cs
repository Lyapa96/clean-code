using System.Collections.Generic;

namespace Markdown.Tags
{
    public class ParagraphTag : MdTag
    {
        public ParagraphTag()
            : base("", new List<MdTag>() {new HyperlinkTag(), new DoubleUnderscoreTag(), new UnderscoreTag()})
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            while (true)
            {
                if (IsStartedPositionTagEnd(line, position))
                    return position;
                position++;
            }
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            return position == 0;
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return position == line.Length;
        }
    }
}