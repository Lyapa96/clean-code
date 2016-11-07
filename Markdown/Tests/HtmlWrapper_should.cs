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
    }
}