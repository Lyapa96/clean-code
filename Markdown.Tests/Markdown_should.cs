using NUnit.Framework;

namespace Markdown.Tests
{
    public class Markdown_should
    {
        private static readonly TestCaseData[] StringMdCase =
        {
            new TestCaseData("a _b_ c").Returns("a <em>b</em> c"),
            new TestCaseData("__s _ss s__ _sdk l_").Returns("__s <em>ss s</em>_ <em>sdk l</em>")
        };

        [TestCaseSource(nameof(StringMdCase))]
        public string createHtmlWithoutTags(string input)
        {
            var answer = Md.Render(input);
            return answer;
        }
    }
}