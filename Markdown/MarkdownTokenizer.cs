using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownTokenizer
    {
        private readonly IMdTag currentMdTag;
        private readonly Tokenizer tokenizer;
        private readonly string sourceString;

        private readonly char[] stopChars = new char[] {};
        Dictionary<string, IMdTag> mdInTag = new Dictionary<string, IMdTag>();

        public MarkdownTokenizer(string sourceString)
        {
            this.sourceString = sourceString;
            this.currentMdTag = null;
            tokenizer = new Tokenizer(sourceString);
        }

        public string ReadLine()
        {
            if (PrefixIsTag())
            {
                ReadToEndTag();
            }
            return tokenizer.ReadUntil(stopChars);
        }

        public string ReadToEndTag()
        {
            while (!currentMdTag.IsEndTag(sourceString,tokenizer.CurrentIndex))
            {

            }
            throw new NotImplementedException();
        }

        public bool PrefixIsTag()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ReadAllLines()
        {
            var allLines = new List<string>();
            while (tokenizer.CurrentIndex < sourceString.Length)
            {
                allLines.Add(ReadLine());
            }
            return allLines;
        }


    }
}