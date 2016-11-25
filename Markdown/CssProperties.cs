using System.Text;

namespace Markdown
{
    public class CssProperties
    {
        private string className;

        public string ClassName
        {
            get { return className != null ? $" class=\"{className}\"" : ""; }
            set { className = value; }
        }

        public string AllCssProperiesToString()
        {
            var type = this.GetType();
            var properties = type.GetProperties();
            var builder = new StringBuilder();
            foreach (var prop in properties)
            {
                builder.Append(prop.GetValue(this));
            }
            return builder.ToString();
        }
    }
}