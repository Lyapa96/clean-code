using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string formatMd)
        {
            var lines = StringSeparator.DivideIntoMdTags(formatMd);
            var html = new StringBuilder();

            foreach (var line in lines)
            {
                //выяснить с какого md тега начинается строка
                //получить эквивалентый html тег
                ITag tagHtml;
                html.Append(tagHtml.WrapStringInTag(line));
            }

            return html.ToString();
        }
    }
}