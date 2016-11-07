using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class UnderscoreTag : IMdTag
    {
        public string NameTag => "_";

        public int FindTagEnd(string line, int position)
        {
            var start = position;
            while (true)
            {
                position++;
                if (position == line.Length) return position;
                if (Char.IsDigit(line, position)) return start;
                if (IsStartedPositionTag(line,position))
                    return position;
            }
        }

        public bool IsStartedPositionTag(string line, int position)
        {
            return line[position].ToString().Equals("_") && !line[position - 1].ToString().Equals(@" ") && TagHelper.IsNotTagShielded(line, position);
        }

        
        public List<string> GetNestedTags => new List<string>();
    }
}