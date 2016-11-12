using System.Collections.Generic;

namespace Markdown.Tags
{
    public class EmptyTag : MdTag
    {
        private static readonly string[] BeginningTags = new[] {"_", "#"};

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
            return IsStartNewTag(line, position);
        }

        private bool IsStartNewTag(string line, int position)
        {
            if (position == line.Length - 1)
            {
                return false;
            }
            return IsSubstringEqualTag(line, position)
                   && TagHelper.IsNotTagEscaped(line, position)
                   && !line[position + 1].ToString().Equals(@" ");
        }

        private bool IsSubstringEqualTag(string line, int position)
        {
            foreach (var beginning in BeginningTags)
            {
                if (TagHelper.IsSubstringEqualTag(line, position, beginning))
                    return true;
            }
            return false;
        }
    }
}