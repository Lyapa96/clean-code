namespace Markdown
{
    public class HtmlTags
    {
        public readonly string StartTag;
        public readonly string EndTag;

        public HtmlTags(string startTag, string endTag)
        {
            StartTag = startTag;
            EndTag = endTag;
        }
    }
}