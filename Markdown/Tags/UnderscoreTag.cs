﻿using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class UnderscoreTag : IMdTag
    {
        public string TagName => "_";

        public int FindTagEnd(string line, int position)
        {
            var start = position;
            while (true)
            {
                position++;
                if (position == line.Length) return position;
                if (Char.IsDigit(line, position)) return start;
                if (IsStartedPositionTagEnd(line, position))
                    return position;
            }
        }

        public bool IsStartedPositionTagEnd(string line, int position)
        {
            return TagHelper.IsSubstringEqualTag(line, position, TagName) &&
                   !line[position - 1].ToString().Equals(@" ") &&
                   TagHelper.IsNotTagEscaped(line, position);
        }

        public bool IsStartedPositionTagStart(string line, int position)
        {
            if (position == line.Length - 1) return false;
            return TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName) &&
                   !line[position + 1].ToString().Equals(@"_") &&
                   !line[position + 1].ToString().Equals(@" ");
        }


        public List<IMdTag> GetNestedTags => new List<IMdTag>();
    }
}