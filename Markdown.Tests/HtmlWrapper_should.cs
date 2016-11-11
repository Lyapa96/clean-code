using System.Net.Mime;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class HtmlWrapper_should
    {
        private static readonly TestCaseData[] StringCase =
        {
            new TestCaseData("abc", "").Returns("abc"),
            new TestCaseData("_abc_", "_").Returns("<em>abc</em>"),
            new TestCaseData("__abc__", "__").Returns("<strong>abc</strong>"),
        };


        [TestCaseSource(nameof(StringCase))]
        public string wrapWordsInHtml(string words, string mdTag)
        {
            return HtmlWrapper.WrapInTags(words, mdTag);
        }


        private static readonly TestCaseData[] StringStartEndCaseCase =
        {
            new TestCaseData("a_b_c", "_").Returns("a<em>b</em>c"),
        };


        [TestCaseSource(nameof(StringStartEndCaseCase))]
        public string wrapWordsStartInHtml(string words, string mdTag)
        {
            return HtmlWrapper.WrapInTags(words, mdTag, 1, 3);
        }


        private static readonly TestCaseData[] MdNodeCase =
       {
            new TestCaseData(new MdNode("text",new DoubleUnderscoreTag())).Returns("<strong>text</strong>"),
            new TestCaseData(new MdNode("text",new UnderscoreTag())).Returns("<em>text</em>"),
            new TestCaseData(new MdNode("text",new EmptyTag())).Returns("text"),
        };


        [TestCaseSource(nameof(MdNodeCase))]
        public string wrapMdNodeInHtml(MdNode mdNode)
        {
            return HtmlWrapper.WrapInTags(mdNode);
        }

        private static readonly TestCaseData[] MdNodeInnerNodeCase =
      {
            new TestCaseData(new MdNode("",new DoubleUnderscoreTag())).Returns("<strong><em>a</em>text<em>b</em></strong>"),
        };


        [TestCaseSource(nameof(MdNodeInnerNodeCase))]
        public string wrapInnerMdNodeInHtml(MdNode mdNode)
        {
            mdNode.InnerMdNodes.Add(new MdNode("a", new UnderscoreTag()));
            mdNode.InnerMdNodes.Add(new MdNode("text", new EmptyTag()));
            mdNode.InnerMdNodes.Add(new MdNode("b", new UnderscoreTag()));

            return HtmlWrapper.WrapInTags(mdNode);
        }
    }
}