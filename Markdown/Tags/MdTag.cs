using System.Collections.Generic;

namespace Markdown.Tags
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
            var other = obj as MdTag;
            if (other == null) return false;
            return TagName.Equals(other.TagName);
        }

        public override int GetHashCode()
        {
            return TagName.GetHashCode();
        }
    }
}