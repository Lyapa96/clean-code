using System.Net.Mime;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class HtmlWrapper_should
    {
        private static readonly TestCaseData[] MdNodeCase =
        {
            new TestCaseData(new MdNode("text", new DoubleUnderscoreTag())).Returns("<strong>text</strong>"),
            new TestCaseData(new MdNode("text", new UnderscoreTag())).Returns("<em>text</em>"),
            new TestCaseData(new MdNode("text", new EmptyTag())).Returns("text"),
        };


        [TestCaseSource(nameof(MdNodeCase))]
        public string wrapMdNodeInHtml(MdNode mdNode)
        {
            var tree = new MdTree(mdNode);
            var htmlConverter = new HtmlConverter();
            return htmlConverter.Convert(tree);
        }

        private static readonly TestCaseData[] MdNodeInnerNodeCase =
        {
            new TestCaseData(new MdNode("", new DoubleUnderscoreTag())).Returns(
                "<strong><em>a</em>text<em>b</em></strong>"),
        };


        [TestCaseSource(nameof(MdNodeInnerNodeCase))]
        public string wrapInnerMdNodeInHtml(MdNode mdNode)
        {
            var htmlConverter = new HtmlConverter();
            mdNode.InnerMdNodes.Add(new MdNode("a", new UnderscoreTag()));
            mdNode.InnerMdNodes.Add(new MdNode("text", new EmptyTag()));
            mdNode.InnerMdNodes.Add(new MdNode("b", new UnderscoreTag()));
            var tree = new MdTree(mdNode);
            return htmlConverter.Convert(tree);
        }
    }
}