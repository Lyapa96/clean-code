using System.Collections.Generic;
using System.Net.Mime;
using Markdown.MdLines;
using Markdown.Tags;

namespace Markdown
{
    public class ParserTextToMdLines
    {
        public readonly string[] Text;
        public List<MdLine> MdLines = new List<MdLine>();

        private readonly List<MdTag> headerTags = new List<MdTag>()
        {
            new DoubleSharpTag(),
            new SharpTag()
            
        };

        public ParserTextToMdLines(string[] text)
        {
            Text = text;
        }

        public List<MdLine> CreateMdLines()
        {
            MdLines = new List<MdLine>();
            var currentLine = new TextLine();
            foreach (var line in Text)
            {
                if (IsHeaderLine(line))
                {
                    if (currentLine.Content == null)
                    {
                        var headerTag = TagHelper.DetermineCurrentTag(line, 0, headerTags);
                        var headerLine = new HeaderLine(headerTag); 
                        headerLine.AddContent(line);                     
                        MdLines.Add(headerLine);
                    }
                    else
                    {
                        MdLines.Add(currentLine);
                        var headerTag = TagHelper.DetermineCurrentTag(line, 0, headerTags);
                        var headerLine = new HeaderLine(headerTag);
                        headerLine.AddContent(line);
                        MdLines.Add(headerLine);
                    }
                    currentLine = new TextLine();
                    continue;
                }
                if (IsEmptyLine(line))
                {
                    if (currentLine.Content != null)
                    {
                        MdLines.Add(currentLine);
                        currentLine = new TextLine();
                    }
                    continue;
                }
                currentLine.AddContent(line);
            }
            if (currentLine.Content != null)
            {
                MdLines.Add(currentLine);
            }
            return MdLines;
        }

        private bool IsHeaderLine(string line)
        {
            foreach (var header in headerTags)
            {
                if (header.IsStartedPositionTagStart(line, 0))
                    return true;
            }
            return false;
        }

        private static bool IsEmptyLine(string line)
        {
            return line.Trim().Length == 0;
        }
    }
}