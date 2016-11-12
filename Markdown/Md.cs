using System;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string mdText)
        {
            var builder = new MdTreeBuilder(mdText);
            var htmlConverter = new HtmlConverter();
            var tree = builder.BuildTree();
            return htmlConverter.Convert(tree);
        }
    }
}