using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        public char CurrenChar => SourceString[CurrentIndex];
        public readonly string SourceString;

        public int CurrentIndex { get; private set; }
        private int substringStartIndex;

        public Tokenizer(string sourceString)
        {
            SourceString = sourceString;
            CurrentIndex = 0;
            substringStartIndex = 0;
        }

        public string ReadUntil(params char[] stopChars)
        {
            for (; CurrentIndex < SourceString.Length; CurrentIndex++)
            {
                if (stopChars.Contains(CurrenChar))
                {
                    var substring = SourceString.Substring(substringStartIndex, CurrentIndex - substringStartIndex);
                    substringStartIndex = CurrentIndex;
                    CurrentIndex++;
                    return substring;
                }
            }
            return SourceString.Substring(substringStartIndex);
        }

       

    }
}