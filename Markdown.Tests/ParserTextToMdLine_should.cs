using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class ParserTextToMdLine_should
    {
        private static readonly TestCaseData[] TextCase =
        {
            new TestCaseData(new List<string>
            {
                "tra la la",
                "tra la la",
            }).Returns(new List<string>() {"tra la la tra la la"}),
            new TestCaseData(new List<string>
            {
                "aaa",
                "   ",
                "bbb",
            }).Returns(new List<string>() {"aaa", "bbb"}),
            new TestCaseData(new List<string>
            {
                "aaa",
                "   ",
                "   ",
                "bbb",
            }).Returns(new List<string>() {"aaa", "bbb"}),
            new TestCaseData(new List<string>
            {
                "   ",
                "aaa",
                "   ",
                "   ",
                "bbb",
                "   ",
            }).Returns(new List<string>() {"aaa", "bbb"}),
            new TestCaseData(new List<string>
            {
                "# aa",
                "bbb",
                "# cc",
            }).Returns(new List<string>() {"# aa", "bbb", "# cc"}),
            new TestCaseData(new List<string>
            {
                "## aa",
                "bbb",
                "# cc",
            }).Returns(new List<string>() {"## aa", "bbb", "# cc"}),
            new TestCaseData(new List<string>
            {
                "text",
                "text",
                "## cc",
            }).Returns(new List<string>() {"text text", "## cc"}),
        };


        [TestCaseSource(nameof(TextCase))]
        public List<string> createParagraph(List<string> text)
        {
            var parser = new ParserTextToMdLines(text.ToArray());
            var paragraphs = parser.CreateMdLines();
            return paragraphs.Select(x => x.Content).ToList();
        }
    }
}