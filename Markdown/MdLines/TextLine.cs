using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.MdLines
{
    public class TextLine : MdLine
    {
        public TextLine(string content) : base(content)
        {
        }

        public TextLine()
        {
        }

        public override List<MdTag> SupportedMdTags => new List<MdTag>()
        {
            new UnderscoreTag(),
            new DoubleUnderscoreTag(),
            new HyperlinkTag()
        };
    }
}