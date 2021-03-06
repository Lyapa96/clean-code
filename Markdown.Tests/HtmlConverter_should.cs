﻿using FluentAssertions;
using Markdown.Converter;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class HtmlConverter_should
    {
        private static readonly TestCaseData[] MdNodeCase =
        {
            new TestCaseData(new MdNode("text", new DoubleUnderscoreTag())).Returns("<strong>text</strong>"),
            new TestCaseData(new MdNode("text", new UnderscoreTag())).Returns("<em>text</em>"),
            new TestCaseData(new MdNode("text", new ParagraphTag())).Returns("<p>text</p>"),
            new TestCaseData(new MdNode("text", new EmptyTag())).Returns("text"),
            new TestCaseData(new MdNode("text", new ListItemTag())).Returns("<li>text</li>"),
            new TestCaseData(new MdNode("text", new OrderListTag())).Returns("<ol>text</ol>"),
            new TestCaseData(new MdNode("function{\r\nvar a = 1 + 2\r\n}", new CodeTag())).Returns(
                "<pre><code>function{\r\nvar a = 1 + 2\r\n}</code></pre>"),
            new TestCaseData(new MdNode("This link](http://example.net/", new HyperlinkTag())).Returns(
                $"<a href=\"http://example.net/\">This link</a>"),
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

        [Test]
        public void createHtmlHyperlinkWithAbsolutePath()
        {
            var htmlConverter = new HtmlConverter(@"http://test.ru/");
            var tree = new MdTree(new MdNode("test](/img", new HyperlinkTag()));
            var html = htmlConverter.Convert(tree);

            html.ShouldBeEquivalentTo($"<a href=\"http://test.ru//img\">test</a>");
        }


        private static readonly TestCaseData[] CssClassNameCase =
        {
            new TestCaseData(new MdNode("text", new DoubleUnderscoreTag())).Returns(
                "<strong class=\"mdClass\">text</strong>"),
            new TestCaseData(new MdNode("text", new UnderscoreTag())).Returns("<em class=\"mdClass\">text</em>"),
            new TestCaseData(new MdNode("text", new EmptyTag())).Returns("text"),
            new TestCaseData(new MdNode("This link](http://example.net/", new HyperlinkTag())).Returns(
                $"<a class=\"mdClass\" href=\"http://example.net/\">This link</a>"),
        };


        [TestCaseSource(nameof(CssClassNameCase))]
        public string createHtmlWithCssClass(MdNode mdNode)
        {
            var tree = new MdTree(mdNode);
            var properties = new CssProperties() {ClassName = "mdClass"};
            var htmlConverter = new HtmlConverter(properties);
            return htmlConverter.Convert(tree);
        }
    }
}