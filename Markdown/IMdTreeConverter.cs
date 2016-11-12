namespace Markdown
{
    public interface IMdTreeConverter<T>
    {
        T Convert(MdTree tree);
    }
}