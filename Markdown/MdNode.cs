using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Markdown
{
    public class MdNode
    {
        public List<MdNode> InnerMdNodes = new List<MdNode>();
        public readonly string Context;
        public IMdTag MdTag { get; }

        public MdNode(string context, IMdTag mdTag)
        {
            Context = context;
            MdTag = mdTag;
        }

        public MdNode(IMdTag mdTag)
        {
            MdTag = mdTag;
        }



        public bool Equals(MdNode other)
        {
            return  Context.Equals(other.Context) && Equals(MdTag, other.MdTag) && Equals(InnerMdNodes,other.InnerMdNodes);
        }

        // && Equals(MdTag, other.MdTag);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Context?.GetHashCode() ?? 0;
                
                return hashCode;
            }

            
        }

        //hashCode = (hashCode*397) ^ (Context?.GetHashCode() ?? 0);
        //hashCode = (hashCode*397) ^ (MdTag?.GetHashCode() ?? 0);
    }
}