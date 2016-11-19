namespace Markdown
{
    public class Md
    {
        public static string BasicUri => @"http://example.net/";

        public static string Render(string mdText)
        {
            var builder = new MdTreeBuilder(mdText);
            var htmlConverter = new HtmlConverter(BasicUri);
            var tree = builder.BuildTree();
            return htmlConverter.Convert(tree);
        }
    }
}