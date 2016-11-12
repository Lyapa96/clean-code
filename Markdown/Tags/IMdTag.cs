
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace Markdown
{
    public interface IMdTag
    {
        string TagName { get; }

        int FindTagEnd(string line,int position);

        bool IsStartedPositionTagStart(string line, int position);
        bool IsStartedPositionTagEnd(string line, int position);
        
        List<IMdTag> GetInnerTags { get; }        
    }
}