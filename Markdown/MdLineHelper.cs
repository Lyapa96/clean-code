using System.Collections.Generic;
using Markdown.MdLines;
using Markdown.Tags;

namespace Markdown
{
    public class MdLineHelper
    {
        private static readonly List<MdTag> HeaderTags = new List<MdTag>()
        {
            new DoubleSharpTag(),
            new SharpTag()
        };

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
            if (currentLine.Content == null)
            {
                var headerTag = TagHelper.DetermineCurrentTag(line, 0, HeaderTags);
                var headerLine = new HeaderLine(headerTag);
                headerLine.AddContent(line);
                mdLines.Add(headerLine);
            }
            else
            {
                mdLines.Add(currentLine);
                var headerTag = TagHelper.DetermineCurrentTag(line, 0, HeaderTags);
                var headerLine = new HeaderLine(headerTag);
                headerLine.AddContent(line);
                mdLines.Add(headerLine);
            }
            currentLine = new TextLine();
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
            foreach (var header in HeaderTags)
            {
                if (header.IsStartedPositionTagStart(line, 0))
                    return true;
            }
            return false;
        }

        public static bool IsCodeLine(string line, MdLine currentLine)
        {
            if (line.Length < 4) return false;
            if (currentLine is CodeLine && line.Substring(0, 4) == "    ") return true;
            return line.Substring(0, 4) == "    " && !IsEmptyLine(line);
        }

        public static bool IsEmptyLine(string line)
        {
            return line.Trim().Length == 0;
        }
    }
}