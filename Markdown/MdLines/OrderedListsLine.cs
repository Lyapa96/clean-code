using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tags;

namespace Markdown.MdLines
{
    public class OrderedListsLine : MdLine
    {
        public override List<MdTag> SupportedMdTags => new List<MdTag>() {new OrderListTag()};


        public override void AddContent(string newLine)
        {            
            var builder = new StringBuilder();
            if (Content != null)
            {
                builder.AppendLine(Content);
            }
            builder.Append(newLine);

            Content = builder.ToString();
        }


        public IEnumerable<MdLine> GetListItems()
        {
            string[] separator = { Environment.NewLine };
            var lines = Content.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select( str => new ListItemLine(str));
            return lines;
        } 
    }

    

    
}