using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tags;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Execution;

namespace Markdown
{
    public class HtmlWrapper
    {
        private Dictionary<string, HtmlTags> MdToHtml = new Dictionary<string, HtmlTags>()
        {
            {"__", new HtmlTags("<strong>", @"</strong>")},
            {"_", new HtmlTags("<em>", @"</em>")},
            {"#", new HtmlTags("<em>", @"</em>")},
            {"", new HtmlTags("", "")}
        };

        public string WrapInTags(string words, string tag)
        {
            var htmlTag = MdToHtml[tag];
            return $"{htmlTag.StartTag}{GetStringWithoutMdTag(words, tag)}{htmlTag.EndTag}";
        }

        public string WrapInTags(string words, string tag, int start, int end)
        {
            var beforeContent = words.Substring(0, start);
            var afterContent = words.Substring(end + tag.Length);
            var mdContent = words.Substring(start, end + tag.Length - start);

            var html = WrapInTags(mdContent, tag);
            return beforeContent + html + afterContent;
        }

        public string WrapInTags(MdNode mdNode)
        {
            var result = new StringBuilder();
            if (!mdNode.InnerMdNodes.Any())
            {
                return WrapMdNode(mdNode.Context, mdNode.MdTag.TagName);
            }

            foreach (var innerNode in mdNode.InnerMdNodes)
            {
                result.Append(WrapMdNode(innerNode.Context, innerNode.MdTag.TagName));
            }
            return WrapMdNode(result.ToString(), mdNode.MdTag.TagName);
        }

        public string WrapMdNode(string words, string tag)
        {
            var htmlTag = MdToHtml[tag];
            return $"{htmlTag.StartTag}{words}{htmlTag.EndTag}";
        }

        private static string GetStringWithoutMdTag(string words, string tag)
        {
            return words.Substring(tag.Length, words.Length - 2*tag.Length);
        }
    }
}