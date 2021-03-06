﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown.MdLines;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTreeBuilder_should
    {
        private static readonly TestCaseData[] MdNodeWithoutInnerTagsCase =
        {
            new TestCaseData("a b c", new MdNode("a b c", new EmptyTag())),
            new TestCaseData("_a_", new MdNode("a", new UnderscoreTag())),
        };

        [TestCaseSource(nameof(MdNodeWithoutInnerTagsCase))]
        public void createMdNodeWithoutInnerTags(string input, MdNode expectedMdNode)
        {
            var builder = new MdTreeBuilder(input);
            var tree = builder.BuildTree();

            tree.Root.InnerMdNodes[0].ShouldBeEquivalentTo(expectedMdNode);
        }


        private static readonly TestCaseData[] MdNodeWithInnerTagCase =
        {
            new TestCaseData("__a__", new MdNode("a", new EmptyTag())),
            new TestCaseData("___abc___", new MdNode("abc", new UnderscoreTag())),
        };

        [TestCaseSource(nameof(MdNodeWithInnerTagCase))]
        public void createMdNodeWithInnerTag(string input, MdNode expectedInnerMdNode)
        {
            var builder = new MdTreeBuilder(input);
            var mdNode = builder.BuildTree().Root.InnerMdNodes[0];

            mdNode.InnerMdNodes.ShouldAllBeEquivalentTo(expectedInnerMdNode);
        }

        private static readonly TestCaseData[] MdNodeWithInnerTagsCase =
        {
            new TestCaseData("___abc_text_abc___",
                new MdNode("", new DoubleUnderscoreTag())
                {
                    InnerMdNodes = new List<MdNode>()
                    {
                        new MdNode("abc", new UnderscoreTag()),
                        new MdNode("text", new EmptyTag()),
                        new MdNode("abc", new UnderscoreTag())
                    }
                }),
            new TestCaseData("# a", new MdNode("", new SharpTag())
            {
                InnerMdNodes = new List<MdNode>()
                {
                    new MdNode("a", new EmptyTag())
                },
            }),
        };

        [TestCaseSource(nameof(MdNodeWithInnerTagsCase))]
        public
        void createMdNodeWithInnerTags(string input, MdNode expectedInnerMdNode)
        {
            var builder = new MdTreeBuilder(input);
            var tree = builder.BuildTree();

            tree.Root.InnerMdNodes.ShouldAllBeEquivalentTo(expectedInnerMdNode);
        }

        private static readonly TestCaseData[] MdNodeWithHyperlinkTagsCase =
        {
            new TestCaseData(@"[This link](http://example.net/)",
                new MdNode(@"This link](http://example.net/", new HyperlinkTag())),
            new TestCaseData(@"[text)](http://example.net/)",
                new MdNode(@"text)](http://example.net/", new HyperlinkTag())),
            new TestCaseData(@"[text(http://example.net/)", new MdNode(@"[text(http://example.net/)", new EmptyTag())),
            new TestCaseData(@"[text(]http://example.net/)", new MdNode(@"[text(]http://example.net/)", new EmptyTag())),
        };

        [TestCaseSource(nameof(MdNodeWithHyperlinkTagsCase))]
        public void createMdNodeWithHyperlink(string input, MdNode expectedMdNode)
        {
            var builder = new MdTreeBuilder(input);
            var tree = builder.BuildTree();

            tree.Root.InnerMdNodes[0].ShouldBeEquivalentTo(expectedMdNode);
        }

        private static readonly TestCaseData[] MdLineCase =
        {
            new TestCaseData(new TextLine("a b c"),
                new MdNode("", new ParagraphTag())
                {
                    InnerMdNodes = new List<MdNode>() {new MdNode("a b c", new EmptyTag())}
                }),
            new TestCaseData(new TextLine("_a_"),
                new MdNode("", new ParagraphTag())
                {
                    InnerMdNodes = new List<MdNode>() {new MdNode("a", new UnderscoreTag())}
                }),
            new TestCaseData(new CodeLine("    function{\r\nvar a = 1 + 2\r\n}"),
                new MdNode("function{\r\nvar a = 1 + 2\r\n}", new CodeTag())),
        };

        [TestCaseSource(nameof(MdLineCase))]
        public void createMdTreeFromMdLine(MdLine input, MdNode expectedMdNode)
        {
            var builder = new MdTreeBuilder(input);
            var tree = builder.BuildTree();

            tree.Root.InnerMdNodes[0].ShouldBeEquivalentTo(expectedMdNode);
        }


        private static readonly TestCaseData[] MdLineWithInnerTagCase =
        {
            new TestCaseData(new HeaderLine("## H2") {HeaderTag = new DoubleSharpTag()},
                new MdNode("H2", new EmptyTag())),
            new TestCaseData(new HeaderLine("# H1") {HeaderTag = new SharpTag()}, new MdNode("H1", new EmptyTag())),
        };

        [TestCaseSource(nameof(MdLineWithInnerTagCase))]
        public void createMdNodeWithInnerTagFromMdLine(MdLine input, MdNode expectedInnerMdNode)
        {
            var builder = new MdTreeBuilder(input);
            var mdNode = builder.BuildTree().Root.InnerMdNodes[0];

            mdNode.InnerMdNodes.ShouldAllBeEquivalentTo(expectedInnerMdNode);
        }

        private string CreateBigString()
        {
            var line = "__text _text_ text _text_ text _text_ __";
            var result = new StringBuilder();
            for (var i = 0; i < 100; i++)
            {
                result.Append(line);
            }
            return result.ToString();
        }

        [Test]
        public void workLinearly()
        {
            var input = CreateBigString();

            var timer = new Stopwatch();
            timer.Start();
            var builder = new MdTreeBuilder(input);
            builder.BuildTree();
            timer.Stop();

            Assert.That(timer.ElapsedMilliseconds, Is.LessThan(1000));
        }
    }
}