using System;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string mdText)
        {
            var builder = new MdTreeBuilder(mdText);
            var htmlWrapper = new HtmlWrapper();
            var root = builder.BuildTree();
            return htmlWrapper.WrapMdTree(root);
        }
    }
}