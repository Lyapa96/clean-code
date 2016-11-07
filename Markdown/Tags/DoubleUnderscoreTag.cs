using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class DoubleUnderscoreTag : IMdTag
    {
        public string NameTag => "__";

        public int FindTagEnd(string line, int position)
        {
            
            while (true)
            {
                position++;
                if (position == line.Length - 1) return position +1;
                if (IsStartedPositionTag(line,position)) return position+1;
            }
        }

        public bool IsStartedPositionTag(string line, int position)
        {
            return line.Substring(position, 2).Equals("__") && TagHelper.IsNotTagShielded(line, position);
        }

        public List<string> GetNestedTags => new List<string>() {"_"};
    }
}