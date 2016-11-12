using NUnit.Framework;

namespace Markdown.Tests
{
    public class Markdown_should
    {
        private static readonly TestCaseData[] TwoWordsCaseMd =
        {
            new TestCaseData("a _b_").Returns("a <em>b</em>"),
            new TestCaseData("a #b#").Returns("a <em>b</em>"),
            new TestCaseData("_a__b_").Returns("<em>a</em><em>b</em>"),
            new TestCaseData("#a##b#").Returns("<em>a</em><em>b</em>"),
            new TestCaseData("a __b__").Returns("a <strong>b</strong>"),
            new TestCaseData("__a__b").Returns("<strong>a</strong>b"),
            new TestCaseData("a ##b##").Returns("a <strong>b</strong>"),
            new TestCaseData("##a##b").Returns("<strong>a</strong>b")
        };

        [TestCaseSource(nameof(TwoWordsCaseMd))]
        public string MddivideTwoWords(string input)
        {
            return Md.Render(input);
        }


        private static readonly TestCaseData[] ThreeWordsCaseMd =
        {
            new TestCaseData("a _b_ c").Returns("a <em>b</em> c"),
            new TestCaseData("a #b# c").Returns("a <em>b</em> c"),
            new TestCaseData("a ##b##_c_").Returns("a <strong>b</strong><em>c</em>"),
            new TestCaseData("a __b___c_").Returns("a <strong>b</strong><em>c</em>"),
            new TestCaseData("a _b___c__").Returns("a <em>b</em><strong>c</strong>"),
            new TestCaseData("a #b#__c__").Returns("a <em>b</em><strong>c</strong>")
        };

        [TestCaseSource(nameof(ThreeWordsCaseMd))]
        public string divideThreeWords(string input)
        {
            return Md.Render(input);
        }


        private static readonly TestCaseData[] IgnoreCase =
        {
            new TestCaseData(@"a \_b\_ c").Returns(@"a \_b\_ c"),
            new TestCaseData(@"a \#b\# c").Returns(@"a \#b\# c"),
            new TestCaseData(@"_\_b\__a").Returns(@"<em>\_b\_</em>a"),
            new TestCaseData(@"\__b_").Returns(@"\_<em>b</em>"),
            new TestCaseData(@"__b\___").Returns(@"<strong>b\_</strong>")
        };

        [TestCaseSource(nameof(IgnoreCase))]
        public string ignoreTagsInSlash(string input)
        {
            return Md.Render(input);
        }


        [Test]
        public void skipTag_WhenSpaceAfterTag()
        {
            var input = "a_ a_ a";
            var answer = Md.Render(input);
            Assert.That(answer, Is.EqualTo(input));
        }


        [Test]
        public void notCloseTag_WhenSpaceBeforeTag()
        {
            var input = "_b _c_";
            var answer = Md.Render(input);
            Assert.That(answer, Is.EqualTo("<em>b _c</em>"));
        }


        private static readonly TestCaseData[] InnerTagsCase =
        {
            new TestCaseData(@"__a _b_ __").Returns("<strong>a <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ _b_ __").Returns("<strong>a <em>b</em> <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ a _b_ a _b_ __").Returns(
                "<strong>a <em>b</em> a <em>b</em> a <em>b</em> </strong>"),
            new TestCaseData(@"__a _b_ _b___").Returns("<strong>a <em>b</em> <em>b</em></strong>"),
            new TestCaseData(@"_a __b__ a_").Returns("<em>a _</em>b__ a_"),
        };

        [TestCaseSource(nameof(InnerTagsCase))]
        public string handleNestedTags(string input)
        {
            return Md.Render(input);
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
            return Md.Render(input);
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
            return Md.Render(input);
        }

        private static readonly TestCaseData[] StringMdCase =
        {
            new TestCaseData("a _b_ c").Returns("a <em>b</em> c"),
            new TestCaseData("__s _ss s__ _sdk l_").Returns("__s <em>ss s</em>_ <em>sdk l</em>")
        };

        [TestCaseSource(nameof(StringMdCase))]
        public string createHtml(string input)
        {
            return Md.Render(input);
        }
    }
}