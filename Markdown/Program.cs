using System.IO;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                var pathToMdFile = args[0];
                var text = File.ReadAllLines(pathToMdFile);

                var html = Md.Render(text);
                File.WriteAllText("out.html", html);
            }
        }
    }
}