using System;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string mdText)
        {
            var tokenizer = new MdTreeBuilder(mdText);
            return tokenizer.GetHtmlText();

        }
    }
}