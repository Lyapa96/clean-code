using System.Collections.Generic;
using System.Text;
using Markdown.Tags;

namespace Markdown.MdLines
{
    public class CodeLine : MdLine
    {
        private int codeTagLenght = 4;

        public CodeLine(string content) : base(content)
        {
        }

        public override List<MdTag> SupportedMdTags => new List<MdTag>();

        public override void AddContent(string newLine)
        {
            newLine = (newLine.Length == codeTagLenght || newLine.Trim().Length == 0)
                ? ""
                : newLine.Substring(codeTagLenght);
            var builder = new StringBuilder();
            if (Content != null)
            {
                builder.AppendLine(Content);
            }
            builder.Append(newLine);

            Content = builder.ToString();
        }
    }
}