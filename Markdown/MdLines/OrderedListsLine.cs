using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.MdLines
{
    public class OrderedListsLine : MdLine
    {
        public override List<MdTag> SupportedMdTags => new List<MdTag>() {new OrderListTag()};
    }
}