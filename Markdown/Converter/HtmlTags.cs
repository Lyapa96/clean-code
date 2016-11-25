namespace Markdown.Converter
{
    public class HtmlTags
    {
        public string StartTag { get; private set; }
        public string EndTag { get; private set; }

        public HtmlTags(string startTag, string endTag)
        {
            StartTag = startTag;
            EndTag = endTag;
        }

        public void AddCssProperties(CssProperties properties)
        {
            if (StartTag != "")
                StartTag = StartTag.Insert(StartTag.Length - 1, properties.AllCssProperiesToString());
        }

        public void AddHref(string href, string basicUri)
        {
            if (IsRelativePath(href))
            {
                href = basicUri + href;
            }
            href = $" href=\"{href}\"";
            StartTag = StartTag.Insert(StartTag.Length - 1, href);
        }

        private bool IsRelativePath(string href)
        {
            return href[0] == '/';
        }
    }
}