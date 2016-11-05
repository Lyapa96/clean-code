using System;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string mdText)
        {
            if (mdText.ContainsMdTags())
            {
                var mdTokenizer = new MarkdownTokenizer(mdText);
                var lines = mdTokenizer.ReadAllLines();
                
                var html = new StringBuilder();
                foreach (var line in lines)
                {
                    var htmlLine = HtmlWrapper.WrapInTags(line);
                    html.Append(htmlLine);
                }
                return Render(html.ToString());
            }
            return mdText;
        }
    }
}