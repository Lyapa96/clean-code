
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace Markdown
{
    public interface IMdTag
    {
        string NameTag { get; }

        int FindTagEnd(string line,int position);

        bool IsStartedPositionTag(string line, int position);
        List<string> GetNestedTags { get; }
    }
}