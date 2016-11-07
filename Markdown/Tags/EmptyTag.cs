using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class EmptyTag : IMdTag
    {
        public string NameTag => "";

        public int FindTagEnd(string line, int position)
        {
            while (true)
            {
                position++;
                if (position >= line.Length - 1) return position;
                if (IsStartNewTag(line, position)) return position - 1;
            }
        }

        public bool IsStartNewTag(string line, int position)
        {
            return line[position].ToString().Equals("_")
                   && TagHelper.IsNotTagShielded(line, position)
                   && !line[position + 1].ToString().Equals(@" ");
        }

        public bool IsStartedPositionTag(string line, int position)
        {
            throw new NotImplementedException();
        }

        public List<string> GetNestedTags => new List<string>();
    }
}