using System.Collections.Generic;

namespace Markdown.Tags
{
    public class ListItemTag : MdTag
    {
        public ListItemTag() : base("1.", new List<MdTag>() { new UnderscoreTag(), new DoubleUnderscoreTag() })
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