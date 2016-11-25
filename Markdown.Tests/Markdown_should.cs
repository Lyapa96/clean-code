using System.Collections.Generic;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Markdown_should
    {
        private static readonly TestCaseData[] TwoWordsCase =
        {
            new TestCaseData("a _b_").Returns("a <em>b</em>"),
            new TestCaseData("# H1").Returns("<h1>H1</h1>"),
            new TestCaseData("_a__b_").Returns("<em>a</em><em>b</em>"),
            new TestCaseData("# a##b#").Returns("<h1>a##b#</h1>"),
            new TestCaseData("a __b__").Returns("a <strong>b</strong>"),
            new TestCaseData("__a__b").Returns("<strong>a</strong>b"),
            new TestCaseData("## aaabbb").Returns("<h2>aaabbb</h2>")
        };

        [TestCaseSource(nameof(TwoWordsCase))]
        public string divideTwoWords(string input)
        {
            return Md.Render(input);
        }


        private static readonly TestCaseData[] ThreeWordsCaseMd =
        {
            new TestCaseData("a _b_ c").Returns("a <em>b</em> c"),
            new TestCaseData("# _H1_").Returns("<h1><em>H1</em></h1>"),
            new TestCaseData("a __b___c_").Returns("a <strong>b</strong><em>c</em>"),
            new TestCaseData("a _b___c__").Returns("a <em>b</em><strong>c</strong>"),
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
            //new TestCaseData("_a123_").Returns("<em>a123</em>"),
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


        private static readonly TestCaseData[] TextMdCase =
        {
            new TestCaseData(new List<string>()
            {
                "a _b_ c",
                "       ",
                "c _b_ a",
            }).Returns("<p>a <em>b</em> c</p>\r\n<p>c <em>b</em> a</p>\r\n"),
            new TestCaseData(new List<string>()
            {
                "         ",
                "a __b__ c",
                "       ",
                "       ",
                "c _b_ a",
            }).Returns("<p>a <strong>b</strong> c</p>\r\n<p>c <em>b</em> a</p>\r\n"),
            new TestCaseData(new List<string>()
            {
                "         ",
                "text",
                "text",
                "## Header2",
                "         ",
                "c _b_ a",
                "# Header1",
            }).Returns("<p>text text</p>\r\n<h2>Header2</h2>\r\n<p>c <em>b</em> a</p>\r\n<h1>Header1</h1>\r\n"),
            new TestCaseData(new List<string>()
            {
                "## Header2",
                "         ",
                "    function{",
                "    var a = 1 + 2",
                "    }",
                "text",
            }).Returns("<h2>Header2</h2>\r\n<pre><code>function{\r\nvar a = 1 + 2\r\n}</code></pre>\r\n<p>text</p>\r\n"),
            new TestCaseData(new List<string>()
            {
                "1. milk",
                "2. coffee",
                "3. cheese",
                "text",
            }).Returns("<ol><li>milk</li><li>coffee</li><li>cheese</li></ol>\r\n<p>text</p>\r\n")
        };

        [TestCaseSource(nameof(TextMdCase))]
        public string createHtmlText(List<string> input)
        {
            return Md.Render(input.ToArray());
        }
    }
}