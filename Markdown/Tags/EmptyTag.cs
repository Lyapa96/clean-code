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

        public bool IsStartNewTag(string line, int position)
        {
            return TagHelper.IsSubstringEqualTag(line, position, "_")
                   && TagHelper.IsNotTagEscaped(line, position)
                   && !line[position + 1].ToString().Equals(@" ");
        }

        public bool IsStartedPositionTagEnd(string line, int position)
        {
            //этот метод никогда не должен вызываться для пустого тега
            throw new NotImplementedException("Нет смысла искать конец пустого тега, его конец там где начинается новый");
        }

        public bool IsStartedPositionTagStart(string line, int position)
        {
            //этот метод никогда не должен вызываться для пустого тега
            throw new NotImplementedException("Нет смысла искать начало, пустой тег может начинаться в любом месте");
        }

        public List<IMdTag> GetNestedTags => new List<IMdTag>();
    }
}