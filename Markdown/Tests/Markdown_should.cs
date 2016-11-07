using NUnit.Framework;

namespace Markdown.Tests
{
    public class Markdown_should
    {
        private static readonly TestCaseData[] StringMdCase =
        {
            new TestCaseData("a _b_ c").Returns("a <em>b</em> c")
        };

        [TestCaseSource(nameof(StringMdCase))]
        public string createHtmlWithoutTags(string input)
        {
            var answer = Md.Render(input);
            return answer;
        }
    }
}