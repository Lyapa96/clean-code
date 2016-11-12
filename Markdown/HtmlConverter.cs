using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class HtmlConverter: IMdTreeConverter<string>
    {
        private readonly Dictionary<string, HtmlTags> mdToHtml = new Dictionary<string, HtmlTags>()
        {
            {"__", new HtmlTags("<strong>", @"</strong>")},
            {"##", new HtmlTags("<h2>", @"</h2>")},
            {"_", new HtmlTags("<em>", @"</em>")},
            {"#", new HtmlTags("<h1>", @"</h1>")},
            {"", new HtmlTags("", "")}
        };

        //public string WrapInTags(string words, string tag)
        //{
        //    var htmlTag = mdToHtml[tag];
        //    return $"{htmlTag.StartTag}{GetStringWithoutMdTag(words, tag)}{htmlTag.EndTag}";
        //}

        //public string WrapInTags(string words, string tag, int start, int end)
        //{
        //    var beforeContent = words.Substring(0, start);
        //    var afterContent = words.Substring(end + tag.Length);
        //    var mdContent = words.Substring(start, end + tag.Length - start);

        //    var html = WrapInTags(mdContent, tag);
        //    return beforeContent + html + afterContent;
        //}

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

        private static string GetStringWithoutMdTag(string words, string tag)
        {
            return words.Substring(tag.Length, words.Length - 2*tag.Length);
        }


        public string Convert(MdTree tree)
        {
            var root = tree.Root;
            return WrapMdNode(root);
        }
    }
}