
namespace Markdown
{
    public interface IMdTag
    {
        string NameTag { get; }
        bool IsEndTag(string line,int position);
    }
}