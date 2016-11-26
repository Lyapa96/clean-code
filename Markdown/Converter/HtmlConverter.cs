using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tags;

namespace Markdown.Converter
{
    public class HtmlConverter : IMdTreeConverter<string>
    {
        private readonly Dictionary<Type, HtmlTags> typeMdToHtml = new Dictionary<Type, HtmlTags>()
        {
            {new DoubleUnderscoreTag().GetType(), new HtmlTags("<strong>", @"</strong>")},
            {new DoubleSharpTag().GetType(), new HtmlTags("<h2>", @"</h2>")},
            {new UnderscoreTag().GetType(), new HtmlTags("<em>", @"</em>")},
            {new SharpTag().GetType(), new HtmlTags("<h1>", @"</h1>")},
            {new EmptyTag().GetType(), new HtmlTags("", "")},
            {new CodeTag().GetType(), new HtmlTags("<pre><code>", "</code></pre>")},
            {new HyperlinkTag().GetType(), new HtmlTags("<a>", "</a>")},
            {new ParagraphTag().GetType(), new HtmlTags("<p>", "</p>")},
            {new OrderListTag().GetType(), new HtmlTags("<ol>", "</ol>")},
            {new ListItemTag().GetType(), new HtmlTags("<li>", "</li>")},
        };

        private readonly string basicUri;
        private readonly CssProperties css;

        public HtmlConverter(string basicUri)
        {
            this.basicUri = basicUri;
            css = new CssProperties();
        }

        public HtmlConverter(string basicUri, CssProperties properties)
        {
            this.basicUri = basicUri;
            css = properties;
        }

        public HtmlConverter(CssProperties properties)
        {
            css = properties;
        }

        public HtmlConverter()
        {
            css = new CssProperties();
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
            return WrapMdNodeWithoutInnerTag(result.ToString(), mdNode.MdTag);
        }

        private string WrapMdNodeWithoutInnerTag(string words, MdTag tag)
        {
            var htmlTag = typeMdToHtml[tag.GetType()];
            htmlTag.AddCssProperties(css);

            if (tag is HyperlinkTag)
            {
                var index = words.IndexOf("](", StringComparison.Ordinal);
                var text = words.Substring(0, index);
                var href = words.Substring(index + 2);
                htmlTag.AddHref(href, basicUri);
                words = text;
            }

            var startTag = htmlTag.StartTag;
            var endTag = htmlTag.EndTag;

            return $"{startTag}{words}{endTag}";
        }
    }
}