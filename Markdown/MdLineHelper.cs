using System.Collections.Generic;
using System.Linq;
using Markdown.MdLines;
using Markdown.Tags;

namespace Markdown
{
    public class MdLineHelper
    {
        private static readonly List<MdTag> headerTags = new List<MdTag>()
        {
            new DoubleSharpTag(),
            new SharpTag()
        };

        private static readonly ListItemTag listItemTag = new ListItemTag();

        public static void HandleCodeLine(ref MdLine currentLine, string line, List<MdLine> mdLines)
        {
            if (currentLine is CodeLine)
            {
                currentLine.AddContent(line);
            }
            else
            {
                if (currentLine.Content != null)
                {
                    mdLines.Add(currentLine);
                }
                currentLine = new CodeLine(line);
            }
        }

        public static void HandleHeaderLine(ref MdLine currentLine, string line, List<MdLine> mdLines)
        {
            if (currentLine.Content != null)
            {
                mdLines.Add(currentLine);
            }
            var headerTag = TagHelper.DetermineCurrentTag(line, 0, headerTags);
            var headerLine = new HeaderLine(headerTag);
            headerLine.AddContent(line);
            mdLines.Add(headerLine);
            currentLine = new TextLine();
        }

        public static void HandleOrderedListLine(ref MdLine currentLine, string line, List<MdLine> mdLines)
        {
            if (currentLine is OrderedListsLine)
            {
                currentLine.AddContent(line);
            }
            else
            {
                if (currentLine.Content != null)
                {
                    mdLines.Add(currentLine);
                    currentLine = new OrderedListsLine();
                    currentLine.AddContent(line);
                }
                else
                {
                    currentLine = new OrderedListsLine();
                    currentLine.AddContent(line);
                }
            }
        }

        public static void HandleEmptyLine(ref MdLine currentLine, string line, List<MdLine> mdLines)
        {
            if (currentLine.Content != null)
            {
                mdLines.Add(currentLine);
                currentLine = new TextLine();
            }
        }


        public static MdTag DetermineCurrentTag(MdLine mdLine)
        {
            if (mdLine is ListItemLine)
            {
                return new ListItemTag();
            }
            if (mdLine is TextLine)
            {
                return new ParagraphTag();
            }
            if (mdLine is HeaderLine)
            {
                return (mdLine as HeaderLine).HeaderTag;
            }
            if (mdLine is CodeLine)
            {
                return new CodeTag();
            }
            var startPosition = 0;
            return TagHelper.DetermineCurrentTag(mdLine.Content, startPosition, mdLine.SupportedMdTags);
        }

        public static bool IsHeaderLine(string line)
        {
            return headerTags.Any(header => header.IsStartedPositionTagStart(line, 0));
        }

        public static bool IsCodeLine(string line, MdLine currentLine)
        {
            var codeTag = new CodeTag();
            if (line.Length < codeTag.TagName.Length) return false;
            if (currentLine is CodeLine && TagHelper.IsSubstringEqualTag(line, 0, codeTag.TagName)) return true;
            return TagHelper.IsSubstringEqualTag(line, 0, codeTag.TagName) && !IsEmptyLine(line);
        }

        public static bool IsEmptyLine(string line)
        {
            return line.Trim().Length == 0;
        }

        public static bool IsOrderedListLine(string line)
        {
            return listItemTag.IsStartedPositionTagStart(line, 0);
        }
    }
}