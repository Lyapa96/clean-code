using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class EmptyTag : IMdTag
    {
        public string TagName => "";

        public int FindTagEnd(string line, int position)
        {
            while (true)
            {
                position++;
                if (position >= line.Length - 1) return position;
                if (IsStartNewTag(line, position)) return position - 1;
            }
        }

        public bool IsStartedPositionTagStart(string line, int position)
        {
            return true;
        }

        public bool IsStartedPositionTagEnd(string line, int position)
        {
            if (++position >= line.Length) return true;
            return (IsStartNewTag(line, position));
        }

        public List<IMdTag> GetInnerTags => new List<IMdTag>();

        private bool IsStartNewTag(string line, int position)
        {
            if (position == line.Length - 1)
            {
                return false;
            }
            return (TagHelper.IsSubstringEqualTag(line, position, "_") ||
                    TagHelper.IsSubstringEqualTag(line, position, "#"))
                   && TagHelper.IsNotTagEscaped(line, position)
                   && !line[position + 1].ToString().Equals(@" ");
        }


        protected bool Equals(EmptyTag other)
        {
            return TagName.Equals(other.TagName);
        }

        public override int GetHashCode()
        {
            return TagName.GetHashCode();
        }
    }
}