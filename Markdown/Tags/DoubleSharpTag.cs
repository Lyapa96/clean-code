﻿using System.Collections.Generic;

namespace Markdown.Tags
{
    public class DoubleSharpTag : MdTag
    {
        public DoubleSharpTag() : base("##", new List<MdTag>() {new SharpTag()})
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            while (true)
            {
                position++;
                if (position == line.Length - 1) return position + 1;
                if (IsStartedPositionTagEnd(line, position)) return position + 1;
            }
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            return TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName);
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName);
        }
    }
}