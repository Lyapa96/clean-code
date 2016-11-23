using System.Collections.Generic;
using Markdown.MdLines;


namespace Markdown
{
    public class ParserTextToMdLines
    {
        public readonly string[] Text;
        public List<MdLine> MdLines = new List<MdLine>();

        public ParserTextToMdLines(string[] text)
        {
            Text = text;
        }

        public List<MdLine> CreateMdLines()
        {
            MdLines = new List<MdLine>();
            MdLine currentMdLine = new TextLine();
            foreach (var line in Text)
            {
                if (MdLineHelper.IsCodeLine(line, currentMdLine))
                {
                    MdLineHelper.HandleCodeLine(ref currentMdLine, line, MdLines);
                    continue;
                }
                if (MdLineHelper.IsHeaderLine(line))
                {
                    MdLineHelper.HandleHeaderLine(ref currentMdLine, line, MdLines);
                    continue;
                }
                if (MdLineHelper.IsEmptyLine(line))
                {
                    MdLineHelper.HandleEmptyLine(ref currentMdLine, line, MdLines);
                    continue;
                }
                if (currentMdLine is CodeLine)
                {
                    MdLines.Add(currentMdLine);
                    currentMdLine = new TextLine(line);
                    continue;
                }
                currentMdLine.AddContent(line);
            }
            if (currentMdLine.Content != null)
            {
                MdLines.Add(currentMdLine);
            }
            return MdLines;
        }
    }
}