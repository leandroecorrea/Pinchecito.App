using HtmlAgilityPack;
using Pinchecito.Core.Interfaces;

namespace Pinchecito.Infrastructure.HtmlParsers
{
    public class FileOrdersPageParser
    {
        private HtmlDocumentBuilder _builder;

        public FileOrdersPageParser(HtmlDocumentBuilder builder)
        {
            _builder = builder;
        }

        public Result<CourtOrder> ParseLastOrder(string htmlFileOrdersPage)
        {
            var document = _builder.SetHtml(htmlFileOrdersPage)
                                   .Build();
            var tableTitle = document.DocumentNode.SelectNodes("//table[@class='pegada']//h3//tr//td//p//strong")
                                           .Where(x => x.InnerText.Trim() == "Pasos Procesales");
            if (ANodeIndicatesNoOrders(document) || OrdersTableIsNotValid(tableTitle))
                return new() { IsSuccess = false };
            var ordersTable = tableTitle.First();
            var lastOrderNode = document.DocumentNode.SelectNodes("//tr")
                                        .Where(x => x.InnerStartIndex > ordersTable.InnerStartIndex)
                                        .Skip(1)
                                        .FirstOrDefault();
            var lastOrderDateText = lastOrderNode.SelectNodes(".//td//p")
                                                 .FirstOrDefault()?
                                                 .InnerText.Trim();
            if (DateTime.TryParse(lastOrderDateText, out DateTime lastOrderDate))
            {
                return new()
                {
                    IsSuccess = true,
                    Value = new CourtOrder
                    {
                        Date = lastOrderDate,
                        OrderType = lastOrderNode.SelectNodes(".//td//a")
                                                 .FirstOrDefault()?
                                                 .InnerHtml ?? "No se pudo obtener el título del trámite"
                    }
                };
            }
            return new()
            {
                IsSuccess = false,
                Errors = new() { new Error() { Message = "No pudo obtenerse la fecha de la última resolución." } }
            };
        }

        private bool OrdersTableIsNotValid(IEnumerable<HtmlNode> tableTitle) =>
            !tableTitle.Any() || tableTitle.Count() != 1;

        private bool ANodeIndicatesNoOrders(HtmlDocument document) =>
                            document.DocumentNode.SelectNodes("//p")
                                                 .Select(x => x.InnerText)
                                                 .Where(x => x.ToLowerInvariant().Contains("sin pasos"))
                                                 .Any();
    }
}
