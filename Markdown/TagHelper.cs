namespace Markdown
{
    public class TagHelper
    {
        public static bool IsNotTagShielded(string text,int position)
        {
            return !text[position - 1].ToString().Equals(@"\");
        }
       
    }
}