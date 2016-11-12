using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class EmptyTag : MdTag
    {
        public EmptyTag() : base("", new List<MdTag>())
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            while (true)
            {
                position++;
                if (position >= line.Length - 1) return position;
                if (IsStartNewTag(line, position)) return position - 1;
            }
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            return true;
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            if (++position >= line.Length) return true;
            return (IsStartNewTag(line, position));
        }

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


        //public override bool Equals(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return false;
        //    }

        //    var other = obj as EmptyTag;
        //    return TagName.Equals(other.TagName);
        //}

        //public override int GetHashCode()
        //{
        //    return TagName.GetHashCode();
        //}
    }
}