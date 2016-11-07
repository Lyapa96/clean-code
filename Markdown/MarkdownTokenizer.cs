using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class MarkdownTokenizer
    {
        private IMdTag currentMdTag;
        private string sourceString;

        public int CurrentPosition { get; private set; }
        private int substringStartPosition;

        private readonly Dictionary<string, IMdTag> mdInTag = new Dictionary<string, IMdTag>()
        {
            {"_", new UnderscoreTag()},
            {"__", new DoubleUnderscoreTag()}
        };


        public MarkdownTokenizer(string sourceString)
        {
            this.sourceString = sourceString;
            currentMdTag = DetermineCurrentTag();
        }


        public string ReadLine()
        {
            sourceString = WrapInnerTags();
            var offset = FindPositionTagEnd();

            CurrentPosition = offset;

            if (offset == sourceString.Length)
            {
                return HtmlWrapper.WrapInTags(GetSubstring(), "");
            }

            var tagName = currentMdTag.NameTag;

            CurrentPosition++;
            if (CurrentPosition < sourceString.Length)
            {
                currentMdTag = DetermineCurrentTag();
            }

            var htmlLine = GetSubstring();
            return HtmlWrapper.WrapInTags(htmlLine, tagName);
        }

        public string ReadAllLines()
        {
            var htmlText = new StringBuilder();
            while (CurrentPosition < sourceString.Length)
            {
                htmlText.Append(ReadLine());
            }           
            return htmlText.ToString();
        }

        public int FindPositionTagEnd()
        {
            var offset = currentMdTag.FindTagEnd(sourceString, CurrentPosition);
            if (offset == CurrentPosition)
            {
                currentMdTag = new EmptyTag();
                offset = currentMdTag.FindTagEnd(sourceString, CurrentPosition);
            }
            return offset;
        }

        public IMdTag DetermineCurrentTag()
        {
            if (CurrentPosition < sourceString.Length - 1 && mdInTag.ContainsKey(sourceString.Substring(CurrentPosition, 2)))
            {
                return mdInTag[sourceString.Substring(CurrentPosition, 2)];
            }
            if (mdInTag.ContainsKey(sourceString[CurrentPosition].ToString()))
            {
                return mdInTag[sourceString[CurrentPosition].ToString()];
            }
            return new EmptyTag();
        }

        public string GetSubstring()
        {
            var substring = sourceString.Substring(substringStartPosition, CurrentPosition - substringStartPosition);
            substringStartPosition = CurrentPosition;
            return substring;
        }


        public string WrapInnerTags()
        {
            var newString = sourceString;
            Dictionary<string, int> tagToPosition = new Dictionary<string, int>();
            var innerTags = currentMdTag.GetNestedTags;
            CurrentPosition += currentMdTag.NameTag.Length;

            for (int i = 0; i < innerTags.Count; i++)
            {
                newString = WrapTag(tagToPosition, newString, innerTags[i]);
            }
            
            CurrentPosition -= currentMdTag.NameTag.Length; 
            return newString;
        }


        public string WrapTag(Dictionary<string, int> tagToPosition, string newString, string innerTag)
        {
            for (int i = CurrentPosition; i < newString.Length+1-currentMdTag.NameTag.Length; i++)
            {
                if (tagToPosition.Count == 0 && currentMdTag.IsStartedPositionTag(newString,i))
                {
                    return newString;
                }
                if (newString.Substring(i,innerTag.Length) == innerTag && tagToPosition.Count == 0)
                {
                    tagToPosition.Add(innerTag, i);
                }
                else if (newString.Substring(i, innerTag.Length) == innerTag)
                {
                    var html = HtmlWrapper.WrapInTags(newString, innerTag, tagToPosition[innerTag], i);
                    tagToPosition.Remove(innerTag);
                    return WrapTag(tagToPosition, html, innerTag);
                }
            }
            return newString;
        }
    }
}