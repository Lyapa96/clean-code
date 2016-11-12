using System.Collections.Generic;
using System.Text;


namespace Markdown
{
    public class HtmlConverter : IMdTreeConverter<string>
    {
        private readonly Dictionary<string, HtmlTags> mdToHtml = new Dictionary<string, HtmlTags>()
        {
            {"__", new HtmlTags("<strong>", @"</strong>")},
            {"##", new HtmlTags("<h2>", @"</h2>")},
            {"_", new HtmlTags("<em>", @"</em>")},
            {"#", new HtmlTags("<h1>", @"</h1>")},
            {"", new HtmlTags("", "")}
        };

        public string Convert(MdTree tree)
        {
            var root = tree.Root;
            return WrapMdNode(root);
        }

        private string WrapMdNode(MdNode mdNode)
        {
            var result = new StringBuilder();

            foreach (var innerNode in mdNode.InnerMdNodes)
            {
                result.Append(WrapMdNode(innerNode));
            }
            result.Append(mdNode.Context);
            return WrapMdNodeWithoutInnerTag(result.ToString(), mdNode.MdTag.TagName);
        }

        private string WrapMdNodeWithoutInnerTag(string words, string tag)
        {
            var htmlTag = mdToHtml[tag];
            return $"{htmlTag.StartTag}{words}{htmlTag.EndTag}";
        }
       
    }
}