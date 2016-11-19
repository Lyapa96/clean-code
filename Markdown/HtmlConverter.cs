using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tags;


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

        private readonly string basicUri;
        private readonly string cssClassName;
        private string cssClass => (cssClassName != null) ? $" class=\"{cssClassName}\"" : "";

        public HtmlConverter(string basicUri)
        {
            this.basicUri = basicUri;
        }

        public HtmlConverter(string basicUri, string cssClassName)
        {
            this.basicUri = basicUri;
            this.cssClassName = cssClassName;
        }

        public HtmlConverter()
        {
        }

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
            result.Append(mdNode.Content);
            if (mdNode.MdTag is HyperlinkTag)
            {
                return WrapMdNodeWithHyperlinkTag(mdNode);
            }
            return WrapMdNodeWithoutInnerTag(result.ToString(), mdNode.MdTag.TagName);
        }

        private string WrapMdNodeWithoutInnerTag(string words, string tag)
        {
            var htmlTag = mdToHtml[tag];
            var startTag = htmlTag.StartTag;
            var endTag = htmlTag.EndTag;
            if (cssClassName != null && startTag!="")
            {
                startTag = startTag.Insert(startTag.Length-1, cssClass);
            }
            return $"{startTag}{words}{endTag}";
        }

        private string WrapMdNodeWithHyperlinkTag(MdNode mdNode)
        {
            var index = mdNode.Content.IndexOf("](", StringComparison.Ordinal);
            var text = mdNode.Content.Substring(0, index);
            var href = mdNode.Content.Substring(index+2);
            if (IsRelativePath(href))
            {
                href = basicUri + href;
            }
            return $"<a{cssClass} href=\"{href}\">{text}</a>";
        }

        private bool IsRelativePath(string href)
        {
            return href[0]== '/';
        }
    }
}