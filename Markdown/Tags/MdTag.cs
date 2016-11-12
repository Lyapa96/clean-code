
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace Markdown
{
    public abstract class MdTag
    {
        public string TagName { get; }
        public List<MdTag> GetInnerTags { get; set; }

        protected MdTag(string tagName, List<MdTag> getInnerTags)
        {
            TagName = tagName;
            GetInnerTags = getInnerTags;
        }

        public abstract int FindTagEnd(string line, int position);
        public abstract bool IsStartedPositionTagStart(string line, int position);
        public abstract bool IsStartedPositionTagEnd(string line, int position);


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            

            var other = obj as MdTag;
            return TagName.Equals(other.TagName);
        }

        public override int GetHashCode()
        {
            return TagName.GetHashCode();
        }
    }
}