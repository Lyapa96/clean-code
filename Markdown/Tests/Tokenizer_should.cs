using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class Tokenizer_should
    {
        private static readonly TestCaseData[] StringCase =
        {
            new TestCaseData("a b c").Returns(new [] {"a"," b"," c"})          
        };


        [TestCaseSource(nameof(StringCase))]
        public string[] divideWords(string input)
        {
            var tokenizer = new Tokenizer(input);
            var stopChars = new[] {' '};

            var a = tokenizer.ReadUntil(stopChars);
            var b  = tokenizer.ReadUntil(stopChars);
            var c  = tokenizer.ReadUntil(stopChars);

            return new string[] {a,b,c};
        }
    }
}