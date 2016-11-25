using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.MdLines
{
    public class ListItemLine : MdLine
    {
        public ListItemLine(string content) : base(content)
        {
        }

        public override List<MdTag> SupportedMdTags => new ListItemTag().GetInnerTags;
    }
}