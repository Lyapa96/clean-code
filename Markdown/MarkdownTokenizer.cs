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

        private readonly List<IMdTag> supportedMdTags = new List<IMdTag>() { new DoubleUnderscoreTag(), new UnderscoreTag() };

        public MarkdownTokenizer(string sourceString)
        {
            this.sourceString = sourceString;
            currentMdTag = TagHelper.DetermineCurrentTag(sourceString,CurrentPosition,supportedMdTags);
        }


        public string ReadLine()
        {
            sourceString = WrapInnerTags();
            var offset = FindPositionTagEnd();

            CurrentPosition = offset;

            if (offset == sourceString.Length)
            {
                return HtmlWrapper.WrapInTags(GetSubstring(), new EmptyTag().TagName);
            }

            var tagName = currentMdTag.TagName;

            CurrentPosition++;
            if (CurrentPosition < sourceString.Length)
            {
                currentMdTag = TagHelper.DetermineCurrentTag(sourceString,CurrentPosition,supportedMdTags);
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
            CurrentPosition += currentMdTag.TagName.Length;

            for (int i = 0; i < innerTags.Count; i++)
            {
                newString = WrapTag(tagToPosition, newString, innerTags[i]);
            }
            
            CurrentPosition -= currentMdTag.TagName.Length; 
            return newString;
        }


        public string WrapTag(Dictionary<string, int> tagToPosition, string newString, IMdTag innerTag)
        {
            for (int i = CurrentPosition; i < newString.Length+1-currentMdTag.TagName.Length; i++)
            {
                if (tagToPosition.Count == 0 && currentMdTag.IsStartedPositionTagEnd(newString,i))
                {
                    return newString;
                }
                if (tagToPosition.Count == 0 && innerTag.IsStartedPositionTagStart(newString,i))
                {
                    tagToPosition.Add(innerTag.TagName, i);
                }
                else if (innerTag.IsStartedPositionTagEnd(newString,i))
                {
                    var html = HtmlWrapper.WrapInTags(newString, innerTag.TagName, tagToPosition[innerTag.TagName], i);
                    tagToPosition.Remove(innerTag.TagName);
                    return WrapTag(tagToPosition, html, innerTag);
                }
            }
            return newString;
        }
    }
}