using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    public class MdNode
    {
        public List<MdNode> InnerMdNodes = new List<MdNode>();
        public string Content { get; }
        public MdTag MdTag { get; }

        public MdNode(string content, MdTag mdTag)
        {
            Content = content;
            MdTag = mdTag;
        }

        public MdNode(MdTag mdTag)
        {
            MdTag = mdTag;
            Content = "";
        }

        public override bool Equals(object obj)
        {
            var other = obj as MdNode;
            if (other == null) return false;
            return Content.Equals(other.Content) && Equals(MdTag, other.MdTag) &&
                   !InnerMdNodes.Except(other.InnerMdNodes).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = InnerMdNodes?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (Content?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (MdTag?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}