using HtmlAgilityPack;
using System.Net;

namespace Pinchecito.Infrastructure.HtmlParsers
{
    public class HtmlDocumentBuilder
    {
        private HtmlDocument htmlDocument = new();
        public HtmlDocumentBuilder SetHtml(string html)
        {            
            htmlDocument.OptionEmptyCollection = true;
            htmlDocument.OptionFixNestedTags = false;
            string subcriterionCritiqueDecode = WebUtility.HtmlDecode(html);
            htmlDocument.LoadHtml(subcriterionCritiqueDecode);
            return this;
        }
        public HtmlDocument Build() => htmlDocument;

    }
}
