using System;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string mdText)
        {
            var tokenizer = new MarkdownTokenizer(mdText);
            return tokenizer.ReadAllLines();
        }
    }
}