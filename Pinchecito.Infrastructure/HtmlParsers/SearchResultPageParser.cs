using HtmlAgilityPack;
using Pinchecito.Core.Interfaces;

namespace Pinchecito.Infrastructure.HtmlParsers
{
    public class SearchResultPageParser
    {
        private HtmlDocumentBuilder _builder;

        public SearchResultPageParser(HtmlDocumentBuilder builder)
        {
            _builder = builder;
        }
        public List<TrackableFile> Parse(string htmlSearchResultsPage)
        {
            var htmlDocument = _builder.SetHtml(htmlSearchResultsPage)
                                        .Build();
            List<TrackableFile> trackableFiles = new();
            if (ValidateDocument(htmlDocument))
            {
                var table = htmlDocument.DocumentNode.SelectNodes("//tbody")
                                                     .Where(tr => tr.SelectNodes("//td//div[@class='AnchoFijoCaratula']").Any())
                                                     .FirstOrDefault()!;
                var tableRows = table.SelectNodes("//tbody//tr").ToList();
                for (int i = 0; i < tableRows.Count; i = i + 2)
                {
                    var currentNode = tableRows[i];
                    TrackableFile file = new();
                    file.CaseCaption = currentNode.SelectNodes(".//td//div//p//a")
                                                   .FirstOrDefault()?
                                                   .InnerText ?? string.Empty
                                                   .Trim();
                    currentNode = tableRows[i + 1];
                    var fileData = currentNode.SelectNodes(".//p").Take(4).ToList();
                    var status = fileData[0].InnerText;
                    file.Status = string.IsNullOrWhiteSpace(status) ? "SIN DATOS" : status.Trim();
                    file.ReceivedId = fileData[1].InnerText;
                    file.Id = fileData[2].InnerText;
                    file.InitialDate = DateTime.TryParse(fileData[3].InnerText, out DateTime initialDate) ? initialDate : DateTime.MinValue;
                    var lastUpdateNode = currentNode.SelectNodes(".//a").FirstOrDefault()?.InnerText;
                    if (string.IsNullOrWhiteSpace(lastUpdateNode) || lastUpdateNode.Split('-').Count() != 2)
                    {
                        trackableFiles.Add(file);
                        continue;
                    }
                    var lastUpdateString = lastUpdateNode.Split('-')[0];
                    var lastOrderTitle = lastUpdateNode.Split("-")[1];
                    file.LastUpdate = DateTime.TryParse(lastUpdateString, out DateTime lastUpdate) ? lastUpdate : DateTime.MinValue;
                    file.LastOrderTitle = lastOrderTitle;
                    trackableFiles.Add(file);
                }
            }
            return trackableFiles;
        }
        private static bool ValidateDocument(HtmlDocument htmlDocument)
        {
            var resultsTable = htmlDocument.DocumentNode.SelectNodes("//tbody").Where(tr => tr.SelectNodes("//td//div[@class='AnchoFijoCaratula']").Any()).FirstOrDefault();
            if (resultsTable is null) return false;
            if (resultsTable.SelectNodes("//tbody//tr").Count() % 2 != 0) return false;
            var tableRows = resultsTable.SelectNodes("//tbody//tr").ToList();
            for (int i = 0; i < tableRows.Count; i = i + 2)
            {
                var currentNode = tableRows[i];
                if (!currentNode.SelectNodes(".//td//div//p//a").Any()) return false;
                currentNode = tableRows[i + 1];
                if (currentNode.SelectNodes(".//p").Count() != 5) return false;
                if (!currentNode.SelectNodes(".//a").Any()) return false;
            }
            return true;
        }
    }
}
