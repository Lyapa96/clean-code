using System.Collections.Generic;
using System.Linq;
using Markdown.MdLines;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class ParserTextToMdLine_should
    {
        private static readonly TestCaseData[] TextCase =
        {
            new TestCaseData(new List<string>
            {
                "tra la la",
                "tra la la",
            }).Returns(new List<string>() {"tra la la tra la la"}),
            new TestCaseData(new List<string>
            {
                "aaa",
                "    ",
                "bbb",
            }).Returns(new List<string>() {"aaa", "bbb"}),
            new TestCaseData(new List<string>
            {
                "aaa",
                "    ",
                "    ",
                "bbb",
            }).Returns(new List<string>() {"aaa", "bbb"}),
            new TestCaseData(new List<string>
            {
                "   ",
                "aaa",
                "    ",
                "    ",
                "bbb",
                "    ",
            }).Returns(new List<string>() {"aaa", "bbb"}),
        };


        [TestCaseSource(nameof(TextCase))]
        public List<string> createTextLine(List<string> text)
        {
            var parser = new ParserTextToMdLines(text.ToArray());
            var mdLines = parser.CreateMdLines();
            return mdLines.Select(x => x.Content).ToList();
        }


        private static readonly TestCaseData[] HeaderCase =
        {
            new TestCaseData(new List<string>
            {
                "# aa",
                "bbb",
                "# cc",
            }).Returns(new List<string>() {"# aa", "bbb", "# cc"}),
            new TestCaseData(new List<string>
            {
                "## aa",
                "bbb",
                "# cc",
            }).Returns(new List<string>() {"## aa", "bbb", "# cc"}),
            new TestCaseData(new List<string>
            {
                "text",
                "text",
                "## cc",
            }).Returns(new List<string>() {"text text", "## cc"}),
        };

        [TestCaseSource(nameof(HeaderCase))]
        public List<string> createHeaderLine(List<string> text)
        {
            var parser = new ParserTextToMdLines(text.ToArray());
            var mdLines = parser.CreateMdLines();
            return mdLines.Select(x => x.Content).ToList();
        }


        private static readonly TestCaseData[] CodeLinesCase =
        {
            new TestCaseData(new List<string>
            {
                "    function{",
                "    var a = 1 + 2",
                "    }",
            }).Returns(new List<string>() {"    function{\r\nvar a = 1 + 2\r\n}"}),
            new TestCaseData(new List<string>
            {
                "    function{",
                "    ",
                "    var a = 1 + 2",
                "    ",
                "    }",
            }).Returns(new List<string>() {"    function{\r\n\r\nvar a = 1 + 2\r\n\r\n}"}),
            new TestCaseData(new List<string>
            {
                "    function{",
                "             ",
                "    var a = 1 + 2",
                "             ",
                "    }",
            }).Returns(new List<string>() {"    function{\r\n\r\nvar a = 1 + 2\r\n\r\n}"}),
            new TestCaseData(new List<string>
            {
                "    function{",
                "    var a = 1 + 2",
                "    }",
                "end."
            }).Returns(new List<string>() {"    function{\r\nvar a = 1 + 2\r\n}", "end."}),
            new TestCaseData(new List<string>
            {
                "    function{",
                "    var a = 1 + 2",
                "    }",
                "# end."
            }).Returns(new List<string>() {"    function{\r\nvar a = 1 + 2\r\n}", "# end."}),
            new TestCaseData(new List<string>
            {
                "## H2",
                "    function{",
                "    var a = 1 + 2",
                "    }",
                "# end."
            }).Returns(new List<string>() {"## H2", "    function{\r\nvar a = 1 + 2\r\n}", "# end."}),
            new TestCaseData(new List<string>
            {
                "text",
                "    function{",
                "    var a = 1 + 2",
                "    }",
                "text",
            }).Returns(new List<string>() {"text", "    function{\r\nvar a = 1 + 2\r\n}", "text"}),
        };

        [TestCaseSource(nameof(CodeLinesCase))]
        public List<string> createCodeLine(List<string> text)
        {
            var parser = new ParserTextToMdLines(text.ToArray());
            var mdLines = parser.CreateMdLines();
            return mdLines.Select(x => x.Content).ToList();
        }


        private static readonly TestCaseData[] OrderListCase =
        {
            new TestCaseData(new List<string>
            {
                "## H2",
                "1. 1",
                "1. 2",
                "1. 3",
                "    ",
            }).Returns(new List<string>() {"1. 1", "1. 2", "1. 3"}),
            new TestCaseData(new List<string>
            {
                "text",
                "1. 1",
                "1. 2",
                "1. 3",
                "text"
            }).Returns(new List<string>() {"1. 1", "1. 2", "1. 3"}),
        };

        [TestCaseSource(nameof(OrderListCase))]
        public List<string> createOrderListLine(List<string> text)
        {
            var parser = new ParserTextToMdLines(text.ToArray());
            var mdLines = parser.CreateMdLines();
            return (mdLines[1] as OrderedListsLine).GetListItems().Select(x => x.Content).ToList();
        }
    }
}