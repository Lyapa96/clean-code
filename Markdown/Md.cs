using System.Text;
using Markdown.Converter;

namespace Markdown
{
    public class Md
    {
        public static string BasicUri => @"http://example.net/";
        public static CssProperties Css => new CssProperties() {ClassName = "md"};

        public static string Render(string mdText)
        {
            var builder = new MdTreeBuilder(mdText);
            var htmlConverter = new HtmlConverter(BasicUri);
            var tree = builder.BuildTree();
            return htmlConverter.Convert(tree);
        }


        public static string Render(string[] mdText)
        {
            var parser = new ParserTextToMdLines(mdText);
            var mdLines = parser.CreateMdLines();

            var builder = new StringBuilder();
            var htmlConverter = new HtmlConverter(BasicUri);

            foreach (var mdLine in mdLines)
            {
                var treeBuilder = new MdTreeBuilder(mdLine);                
                var tree = treeBuilder.BuildTree();
                builder.AppendLine(htmlConverter.Convert(tree));
            }
            return builder.ToString();
        }
    }
}