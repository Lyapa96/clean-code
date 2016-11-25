using System.Collections.Generic;

namespace Markdown.Tags
{
    public class OrderListTag : MdTag
    {
        public OrderListTag() : base("", new List<MdTag>() {new ListItemTag()})
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