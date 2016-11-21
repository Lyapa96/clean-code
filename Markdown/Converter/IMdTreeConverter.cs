namespace Markdown.Converter
{
    public interface IMdTreeConverter<T>
    {
        T Convert(MdTree tree);
    }
}