using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
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
            new TestCaseData("#a#", new MdNode("a", new SharpTag())),
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
                })
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
            new TestCaseData(@"[This link](http://example.net/)", new MdNode(@"This link](http://example.net/", new HyperlinkTag())),
            new TestCaseData(@"[text)](http://example.net/)", new MdNode(@"text)](http://example.net/", new HyperlinkTag())),
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