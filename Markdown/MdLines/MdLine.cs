using System.Collections.Generic;
using System.Text;
using Markdown.Tags;

namespace Markdown.MdLines
{
    public abstract class MdLine
    {
        public string Content { get; protected set; }
        public abstract List<MdTag> SupportedMdTags { get; }

        protected MdLine(string content)
        {
            Content = content;
        }

        protected MdLine()
        {
        }

        public virtual void AddContent(string newLine)
        {
            var builder = new StringBuilder();
            if (Content != null)
            {
                builder.Append(Content);
                builder.Append(" ");
            }
            builder.Append(newLine);

            Content = builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return Content.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Content.GetHashCode();
        }
    }
}