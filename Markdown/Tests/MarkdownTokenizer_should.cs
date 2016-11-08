using NUnit.Framework;

namespace Markdown.Tests
{
    public class MarkdownTokenizer_should
    {
        [Test]
        public void returnWordWithoutTags()
        {
            var tokenizer = new MarkdownTokenizer("a b c");
            var result = tokenizer.ReadLine();

            Assert.That(result, Is.EqualTo("a b c"));
        }


        private static readonly TestCaseData[] TwoWordsCase =
        {
            new TestCaseData("a _b_").Returns(new[] {"a ", "<em>b</em>"}),
            new TestCaseData("_a__b_").Returns(new[] {"<em>a</em>", "<em>b</em>"}),
            new TestCaseData("a __b__").Returns(new[] {"a ", "<strong>b</strong>"}),
            new TestCaseData("__a__b").Returns(new[] {"<strong>a</strong>", "b"})
        };

        [TestCaseSource(nameof(TwoWordsCase))]
        public string[] divideTwoWords(string input)
        {
            var tokenizer = new MarkdownTokenizer(input);

            var a = tokenizer.ReadLine();
            var b = tokenizer.ReadLine();

            return new string[] {a, b};
        }


        private static readonly TestCaseData[] ThreeWordsCase =
        {
            new TestCaseData("a _b_ c").Returns(new[] {"a ", "<em>b</em>", " c"}),
            new TestCaseData("a __b___c_").Returns(new[] {"a ", "<strong>b</strong>", "<em>c</em>"}),
            new TestCaseData("a _b___c__").Returns(new[] {"a ", "<em>b</em>", "<strong>c</strong>"})
        };

        [TestCaseSource(nameof(ThreeWordsCase))]
        public string[] divideThreeWords(string input)
        {
            var tokenizer = new MarkdownTokenizer(input);

            var a = tokenizer.ReadLine();
            var b = tokenizer.ReadLine();
            var c = tokenizer.ReadLine();

            return new string[] {a, b, c};
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
            var tokenizer = new MarkdownTokenizer(input);

            var result = tokenizer.ReadAllLines();

            return new string[] {result};
        }


        [Test]
        public void skipTag_WhenSpaceAfterTag()
        {
            var input = "a_ a_ a";
            var tokenizer = new MarkdownTokenizer(input);

            var answer = tokenizer.ReadLine();
            Assert.That(answer, Is.EqualTo(input));
        }


        [Test]
        public void notCloseTag_WhenSpaceBeforeTag()
        {
            var input = "_b _c_";
            var tokenizer = new MarkdownTokenizer(input);

            var answer = tokenizer.ReadLine();
            Assert.That(answer, Is.EqualTo("<em>b _c</em>"));
        }


        private static readonly TestCaseData[] NeatedTagsCase =
        {
            new TestCaseData(@"__a _b_ __").Returns("<strong>a <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ _b_ __").Returns("<strong>a <em>b</em> <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ a _b_ a _b_ __").Returns(
                "<strong>a <em>b</em> a <em>b</em> a <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ _b___").Returns("<strong>a <em>b</em> <em>b</em></strong>"),
            new TestCaseData(@"_a __b__ a_").Returns("<em>a _</em>")
        };

        [TestCaseSource(nameof(NeatedTagsCase))]
        public string handleNestedTags(string input)
        {
            var tokenizer = new MarkdownTokenizer(input);

            var result = tokenizer.ReadLine();

            return result;
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
            var tokenizer = new MarkdownTokenizer(input);

            var result = tokenizer.ReadAllLines();

            return result;
        }


        private static readonly TestCaseData[] UnpairedTagsCase =
        {
            new TestCaseData("_ __").Returns("<em> _</em>"),
            new TestCaseData("_ ").Returns("_ "),
            new TestCaseData("__ ").Returns("__ "),
            new TestCaseData("__ _a_ ").Returns("__ <em>a</em> "),
            new TestCaseData("__ _ _ ").Returns("__ _ _ ")
        };

        [TestCaseSource(nameof(UnpairedTagsCase))]
        public string handleUnpairedTags(string input)
        {
            var tokenizer = new MarkdownTokenizer(input);

            var result = tokenizer.ReadLine();

            return result;
        }
    }
}