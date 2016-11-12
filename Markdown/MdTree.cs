namespace Markdown
{
    public class MdTree
    {
        public readonly MdNode Root;

        public MdTree(MdNode mdNode)
        {
            Root = mdNode;
        }
    }
}