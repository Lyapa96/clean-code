using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class MdTreeBuilder
    {
        private MdTag currentMdTag;
        private int currentPosition;
        private int substringStartPosition;

        private readonly string sourceString;

        private readonly List<MdTag> supportedMdTags = new List<MdTag>()
        {
            new DoubleUnderscoreTag(),
            new UnderscoreTag(),
            new SharpTag(),
        };

        public MdTreeBuilder(string sourceString)
        {
            this.sourceString = sourceString;
            currentMdTag = TagHelper.DetermineCurrentTag(sourceString, currentPosition, supportedMdTags);
        }


        public string GetHtmlText()
        {
            var htmlWrapper = new HtmlWrapper();
            var htmlText = new StringBuilder();
            while (currentPosition < sourceString.Length)
            {
                htmlText.Append(htmlWrapper.WrapInTags(GetMdNode()));
            }
            return htmlText.ToString();
        }

        public MdNode Build()
        {
            var mdNode = new MdNode(new EmptyTag());
            while (currentPosition < sourceString.Length)
            {
                mdNode.InnerMdNodes.Add(GetMdNode());
            }
            return mdNode;
        }

        public MdNode GetMdNode()
        {
            if (currentMdTag.GetInnerTags.Count != 0)
            {
                return GetMdNodeWithInnerNodes();
            }

            var offset = FindPositionTagEnd();
            currentPosition = offset;

            if (offset == sourceString.Length)
            {
                var emptyTag = new EmptyTag();
                return new MdNode(GetSubstringWithoutMdTags(emptyTag.TagName), emptyTag);
            }

            var previousTag = currentMdTag;
            UpdateCurrentTag();

            var context = GetSubstringWithoutMdTags(previousTag.TagName);
            return new MdNode(context, previousTag);
        }

        private MdNode GetMdNodeWithInnerNodes()
        {
            var start = currentPosition;
            var mdNodeWithInnerNodes = GetInnerNodes();
            substringStartPosition = currentPosition;
            if (IsTagСorrectlyClosed(start))
            {
                currentMdTag = TagHelper.DetermineCurrentTag(sourceString, currentPosition, supportedMdTags);
                return mdNodeWithInnerNodes;
            }
            return CreateRightMdNode(mdNodeWithInnerNodes);
        }

        private MdNode GetInnerNodes()
        {
            var mdNode = new MdNode(currentMdTag);
            var tags = currentMdTag.GetInnerTags;

            var tagToPosition = new Dictionary<string, int>();
            currentPosition += currentMdTag.TagName.Length;
            var innerTag = TagHelper.DetermineCurrentTag(sourceString, currentPosition, tags);
            tagToPosition.Add(innerTag.TagName, currentPosition);
            currentPosition += innerTag.TagName.Length;
            for (int i = currentPosition; i < sourceString.Length; i++)
            {
                if (tagToPosition.Count == 0 && currentMdTag.IsStartedPositionTagEnd(sourceString, i))
                {
                    currentPosition = i + currentMdTag.TagName.Length;
                    return mdNode;
                }
                if (tagToPosition.Count == 0)
                {
                    innerTag = TagHelper.DetermineCurrentTag(sourceString, currentPosition, tags);
                    tagToPosition.Add(innerTag.TagName, currentPosition);
                    i += innerTag.TagName.Length;
                }
                if (innerTag.IsStartedPositionTagEnd(sourceString, i))
                {
                    AddInnerMdNode(tagToPosition, innerTag, i, mdNode);
                    tagToPosition.Remove(innerTag.TagName);
                    
                }
            }
            if (tagToPosition.Count != 0)
            {
                AddNotClosedInnerTag(tagToPosition, innerTag, mdNode);
            }
            currentPosition = sourceString.Length;
            return mdNode;
        }

        private void AddNotClosedInnerTag(Dictionary<string, int> tagToPosition, MdTag innerTag, MdNode mdNode)
        {
            var lenght = sourceString.Length - tagToPosition[innerTag.TagName];
            mdNode.InnerMdNodes.Add(new MdNode(sourceString.Substring(tagToPosition[innerTag.TagName], lenght),
                new EmptyTag()));
        }

        private void AddInnerMdNode(Dictionary<string, int> tagToPosition, MdTag innerTag, int position, MdNode mdNode)
        {
            var start = tagToPosition[innerTag.TagName] + innerTag.TagName.Length;
            var lenghtTag = (innerTag.TagName.Length == 0) ? 1 : innerTag.TagName.Length;
            var lenght = position - start;
            if (innerTag is EmptyTag)
            {
                lenght++;
            }
            mdNode.InnerMdNodes.Add(new MdNode(sourceString.Substring(start, lenght), innerTag));
            currentPosition = position + lenghtTag;
        }

        private bool IsTagСorrectlyClosed(int start)
        {
            return start != currentPosition - currentMdTag.TagName.Length &&
                   currentMdTag.IsStartedPositionTagEnd(sourceString, currentPosition - currentMdTag.TagName.Length);
        }

        private MdNode CreateRightMdNode(MdNode mdNodeWithInnerNodes)
        {
            var innerNodes = mdNodeWithInnerNodes.InnerMdNodes;
            innerNodes.Reverse();
            innerNodes.Add(new MdNode(currentMdTag.TagName, new EmptyTag()));
            innerNodes.Reverse();
            return new MdNode(new EmptyTag()) {InnerMdNodes = innerNodes};
        }

        private int FindPositionTagEnd()
        {
            var offset = currentMdTag.FindTagEnd(sourceString, currentPosition);
            if (offset == currentPosition)
            {
                currentMdTag = new EmptyTag();
                offset = currentMdTag.FindTagEnd(sourceString, currentPosition);
            }
            return offset;
        }

        private void UpdateCurrentTag()
        {
            currentPosition++;
            if (currentPosition < sourceString.Length)
            {
                currentMdTag = TagHelper.DetermineCurrentTag(sourceString, currentPosition, supportedMdTags);
            }
        }

        private string GetSubstringWithoutMdTags(string oldTagName)
        {
            var substring = sourceString.Substring(substringStartPosition + oldTagName.Length,
                currentPosition - substringStartPosition - 2*oldTagName.Length);
            substringStartPosition = currentPosition;
            return substring;
        }
    }
}