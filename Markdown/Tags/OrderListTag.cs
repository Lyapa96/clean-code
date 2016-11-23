using System.Collections.Generic;

namespace Markdown.Tags
{
    public class OrderListTag : MdTag
    {
        public OrderListTag() : base("1.", new List<MdTag>() {new ListItemTag()})
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            throw new System.NotImplementedException();
        }
    }
}