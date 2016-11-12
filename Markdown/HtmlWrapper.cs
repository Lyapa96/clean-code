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
        private readonly Dictionary<string, HtmlTags> mdToHtml = new Dictionary<string, HtmlTags>()
        {
            {"__", new HtmlTags("<strong>", @"</strong>")},
            {"##", new HtmlTags("<strong>", @"</strong>")},
            {"_", new HtmlTags("<em>", @"</em>")},
            {"#", new HtmlTags("<em>", @"</em>")},
            {"", new HtmlTags("", "")}
        };

        public string WrapInTags(string words, string tag)
        {
            var htmlTag = mdToHtml[tag];
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

        public string WrapMdTree(MdNode mdNode)
        {
            var result = new StringBuilder();

            foreach (var innerNode in mdNode.InnerMdNodes)
            {
                result.Append(WrapMdTree(innerNode));
            }
            result.Append(mdNode.Context);
            return WrapMdNode(result.ToString(), mdNode.MdTag.TagName);
        }

        private string WrapMdNode(string words, string tag)
        {
            var htmlTag = mdToHtml[tag];
            return $"{htmlTag.StartTag}{words}{htmlTag.EndTag}";
        }

        private static string GetStringWithoutMdTag(string words, string tag)
        {
            return words.Substring(tag.Length, words.Length - 2*tag.Length);
        }
    }
}