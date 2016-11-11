using System.Collections.Generic;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTokenizer_should
    {

        [Test]
        public void MDreturnWordWithoutTags()
        {
            var tokenizer = new MdTokenizer("a b c");
            var result = tokenizer.GetMdNode();

            Assert.That(result.Context, Is.EqualTo("a b c"));
        }

        private static readonly TestCaseData[] TwoWordsCaseMd =
        {
            new TestCaseData("a _b_").Returns("a <em>b</em>"),
            new TestCaseData("_a__b_").Returns("<em>a</em><em>b</em>"),
            new TestCaseData("a __b__").Returns("a <strong>b</strong>"),
            new TestCaseData("__a__b").Returns("<strong>a</strong>b")
        };

        [TestCaseSource(nameof(TwoWordsCaseMd))]
        public string MddivideTwoWords(string input)
        {
            var tokenizer = new MdTokenizer(input);

            var res = tokenizer.GetHtmlText();

            return res;
        }


        private static readonly TestCaseData[] ThreeWordsCaseMd =
        {
           new TestCaseData("a _b_ c").Returns("a <em>b</em> c"),
            new TestCaseData("a __b___c_").Returns("a <strong>b</strong><em>c</em>"),
            new TestCaseData("a _b___c__").Returns("a <em>b</em><strong>c</strong>")
        };

        [TestCaseSource(nameof(ThreeWordsCaseMd))]
        public string MddivideThreeWords(string input)
        {
            var tokenizer = new MdTokenizer(input);

            var res = tokenizer.GetHtmlText();

            return res;
        }


        private static readonly TestCaseData[] IgnoreCase =
        {
            new TestCaseData(@"a \_b\_ c").Returns(new[] {@"a \_b\_ c"}),
            new TestCaseData(@"_\_b\__a").Returns(new[] {@"<em>\_b\_</em>a"}),
            new TestCaseData(@"\__b_").Returns(new[] {@"\_<em>b</em>"}),
            new TestCaseData(@"__b\___").Returns(new[] {@"<strong>b\_</strong>"})
        };

        [TestCaseSource(nameof(IgnoreCase))]
        public string[] ignoreTagsInSlash(string input)
        {
            var tokenizer = new MdTokenizer(input);

            var result = tokenizer.GetHtmlText();

            return new string[] { result };
        }


        [Test]
        public void skipTag_WhenSpaceAfterTag()
        {
            var input = "a_ a_ a";
            var tokenizer = new MdTokenizer(input);

            var answer = tokenizer.GetMdNode();
            Assert.That(answer.Context, Is.EqualTo(input));
        }


        [Test]
        public void notCloseTag_WhenSpaceBeforeTag()
        {
            var input = "_b _c_";
            var tokenizer = new MdTokenizer(input);

            var answer = tokenizer.GetHtmlText();
            Assert.That(answer, Is.EqualTo("<em>b _c</em>"));
        }


        private static readonly TestCaseData[] MdNeatedTagsCase =
       {
           new TestCaseData(@"__a _b_ __").Returns("<strong>a <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ _b_ __").Returns("<strong>a <em>b</em> <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ a _b_ a _b_ __").Returns(
                "<strong>a <em>b</em> a <em>b</em> a <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ _b___").Returns("<strong>a <em>b</em> <em>b</em></strong>"),
            new TestCaseData(@"_a __b__ a_").Returns("<em>a _</em>b__ a_"),
        };

        [TestCaseSource(nameof(MdNeatedTagsCase))]
        public string MdhandleNestedTags(string input)
        {
            var tokenizer = new MdTokenizer(input);


            return tokenizer.GetHtmlText();
        }


        private static readonly TestCaseData[] NumbersInTextCase =
       {
            new TestCaseData("_123_").Returns("_123_"),
            new TestCaseData("a_123_").Returns("a_123_"),
            new TestCaseData("a123_b_").Returns("a123<em>b</em>"),
            new TestCaseData("a123_b1_").Returns("a123_b1_"),
        };

        [TestCaseSource(nameof(NumbersInTextCase))]
        public string doNotWrapTextWithNumbers(string input)
        {
            var tokenizer = new MdTokenizer(input);

            var result = tokenizer.GetHtmlText();

            return result;
        }


       private static readonly TestCaseData[] UnpairedTagsCase =
       {
            new TestCaseData("_a__").Returns("<em>a</em>_"),
            new TestCaseData("_ __").Returns("_ __"),
            new TestCaseData("_ ").Returns("_ "),
            new TestCaseData("__ ").Returns("__ "),
            new TestCaseData("__ _a_ ").Returns("__ <em>a</em> "),
            new TestCaseData("__ _ _ ").Returns("__ _ _ "),
            new TestCaseData(@"__ a_").Returns("__ a_"),
            new TestCaseData(@"__ _a_ _b").Returns("__ <em>a</em> _b")
        };

        [TestCaseSource(nameof(UnpairedTagsCase))]
        public string handleUnpairedTags(string input)
        {
            var tokenizer = new MdTokenizer(input);

            var result = tokenizer.GetHtmlText();

            return result;
        }
    }
}