using System;
using System.Collections.Generic;
using Markdown.Tags;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class HtmlWrapper
    {
        public static Dictionary<string,HtmlTags> MdToHtml = new Dictionary<string, HtmlTags>()
        {
            {"__", new HtmlTags("<strong>",@"</strong>")},
            {"_", new HtmlTags("<em>",@"</em>")},
            {"", new HtmlTags("","")}
        };


        public static string WrapInTags(string words, string tag)
        {
            return $"{MdToHtml[tag].StartTag}{words.Substring(tag.Length,words.Length-2*tag.Length)}{MdToHtml[tag].EndTag}";
        }


        public static string WrapInTags(string words, string tag, int start, int end)
        {
            var beforeContent = words.Substring(0, start);
            var afterContent = words.Substring(end + tag.Length);
            var mdContent = words.Substring(start, end + tag.Length - start);

            var html = WrapInTags(mdContent, tag);

            return beforeContent+html+afterContent;
        }
    }

    public class HtmlTags
    {
        public readonly string StartTag;
        public readonly string EndTag;

        public HtmlTags(string startTag, string endTag)
        {
            StartTag = startTag;
            EndTag = endTag;
        }
    }
}