using CommonMark;

namespace Storage.Common.Extensions
{
    public static class MarkdownExtensionscs
    {
        public static string ToHtml(this string markdown)
        {
            return CommonMarkConverter.Convert(markdown);
        }
    }
}