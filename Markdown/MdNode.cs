using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    public class MdNode
    {
        public List<MdNode> InnerMdNodes = new List<MdNode>();
        public string Context { get; }
        public MdTag MdTag { get; }

        public MdNode(string context, MdTag mdTag)
        {
            Context = context;
            MdTag = mdTag;
        }

        public MdNode(MdTag mdTag)
        {
            MdTag = mdTag;
            Context = "";
        }

        public override bool Equals(object obj)
        {
            var other = obj as MdNode;
            if (other == null) return false;
            return Context.Equals(other.Context) && Equals(MdTag, other.MdTag) &&
                   !InnerMdNodes.Except(other.InnerMdNodes).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = InnerMdNodes?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (Context?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (MdTag?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}