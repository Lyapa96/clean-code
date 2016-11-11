using System.Collections.Generic;

namespace Markdown
{
    public class MdNode
    {
        public List<MdNode> InnerMdNodes = new List<MdNode>();
        public readonly string Context;
        public IMdTag MdTag { get; set; }

        public MdNode(string context, IMdTag mdTag)
        {
            Context = context;
            MdTag = mdTag;
        }

        public MdNode(IMdTag mdTag)
        {
            MdTag = mdTag;
        }
    }
}