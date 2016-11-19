using System.Collections.Generic;
using System.Diagnostics;

namespace Markdown.Tags
{
    public class HyperlinkTag : MdTag
    {
        private bool wereAlreadyClosedSquareAndOpenRoundBrackets = false;
        public string Link { get; private set; }

        public HyperlinkTag() : base("[", new List<MdTag>())
        {
        }

        public override int FindTagEnd(string line, int position)
        {
            while (true)
            {
                if (position == line.Length) return position;
                if (IsStartedPositionTagEnd(line, position))
                    return position;
                position++;
                if (!wereAlreadyClosedSquareAndOpenRoundBrackets)
                {
                    wereAlreadyClosedSquareAndOpenRoundBrackets = CheckClosedSquareAndOpenRoundBrackets(line, position);
                }
            }
        }

        public override bool IsStartedPositionTagStart(string line, int position)
        {
            return TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, TagName);
        }

        public override bool IsStartedPositionTagEnd(string line, int position)
        {
            return wereAlreadyClosedSquareAndOpenRoundBrackets &&
                   TagHelper.IsNotTagEscaped(line, position) &&
                   TagHelper.IsSubstringEqualTag(line, position, ")");
        }


        private bool CheckClosedSquareAndOpenRoundBrackets(string line, int position)
        {
            if (position < 2 || line.Length - position < 2) return false;
            if (line.Substring(position - 2, 2) == "](") return true;
            return false;
        }
    }
}