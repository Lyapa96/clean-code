using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.MdLines
{
    public class HeaderLine : MdLine
    {
        public MdTag HeaderTag;

        public HeaderLine(string content) : base(content)
        {
        }

        public HeaderLine(MdTag headerTag)
        {
            HeaderTag = headerTag;
        }

        public override List<MdTag> SupportedMdTags => new List<MdTag>()
        {
            new UnderscoreTag(),
            new DoubleUnderscoreTag(),
            new HyperlinkTag()
        };
    }
}