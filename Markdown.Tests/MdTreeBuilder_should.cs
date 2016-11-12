using System.CodeDom;
using System.Collections.Generic;
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
            var mdNode = builder.GetMdNode();

            Assert.That(mdNode, Is.EqualTo(expectedMdNode));
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
            var mdNode = builder.GetMdNode();

            Assert.That(mdNode.InnerMdNodes[0], Is.EqualTo(expectedInnerMdNode));
        }

        private static readonly TestCaseData[] MdNodeWithInnerTagsCase =
        {
            new TestCaseData("___abc_text_abc___",
                new MdNode("", new DoubleUnderscoreTag())
                {
                    InnerMdNodes = new List<MdNode>()
                    {
                        new MdNode("abc",new UnderscoreTag()),
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
            var mdNode = builder.GetMdNode();


            mdNode.InnerMdNodes.ShouldBeEquivalentTo
                (
                    expectedInnerMdNode
                );
        }
    }
}