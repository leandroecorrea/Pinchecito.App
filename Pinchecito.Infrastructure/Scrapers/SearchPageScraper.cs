using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.HtmlParsers;
using System.IO.Compression;
using System.Net;

namespace Pinchecito.Infrastructure.Scrapers
{
    public class SearchPageScraper
    {
        private const string POS_LOGIN_URL = "https://mev.scba.gov.ar/POSLoguin.asp";
        private SearchResultPageParser parser;
        private static string SEARCH_URL = "https://mev.scba.gov.ar/busqueda.asp";
        private CookieContainer cookieContainer;
        private HttpClientHandler httpClientHandler;
        private HttpClient _httpClient;

        public SearchPageScraper(SearchResultPageParser parser)
        {
            this.parser = parser;
            cookieContainer = new CookieContainer();
            httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
            };
            _httpClient = new HttpClient(httpClientHandler, false);
        }

        public async Task<Result<IEnumerable<TrackableFile>>> SearchFilesAsync(GetTrackableFileByIdRequest request, Cookie cookie)
        {
            try
            {
                return await QueryWebpage(request, cookie);
            }
            catch (Exception ex)
            {
                return new()
                {
                    IsSuccess = false,
                    Errors = new() { new() { Message = ex.Message } }
                };
            }
        }

        private async Task<Result<IEnumerable<TrackableFile>>> QueryWebpage(GetTrackableFileByIdRequest request, Cookie cookie)
        {
            httpClientHandler.CookieContainer.Add(cookie);
            var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("TipoDto", "CC"),
                        new KeyValuePair<string, string>("DtoJudElegido", request.QueryDistrict),
                        new KeyValuePair<string, string>("Aceptar", "Aceptar")
                    });
            var response = await _httpClient.PostAsync(POS_LOGIN_URL, content);
            response.EnsureSuccessStatusCode();
            content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("OpcionBusqueda", "1"),
                new KeyValuePair<string, string>("busca", request.FileNumber),
                new KeyValuePair<string, string>("JuzgadoElegido", request.CourtId),
                new KeyValuePair<string, string>("radio", "xNc"),
                new KeyValuePair<string, string>("caratula", ""),
                new KeyValuePair<string, string>("Ncausa", request.FileNumber),
                new KeyValuePair<string, string>("Ninterno", ""),
                new KeyValuePair<string, string>("Set", ""),
                new KeyValuePair<string, string>("Desde", DateTime.Now.Subtract(TimeSpan.FromDays(2)).ToString("dd/MM/yyyy")),
                new KeyValuePair<string, string>("Hasta", DateTime.Now.ToString("dd/MM/yyyy")),
                new KeyValuePair<string, string>("SetNovedades", ""),
                new KeyValuePair<string, string>("TipoCausa", "Ac"),
                new KeyValuePair<string, string>("Buscar", "Buscar"),
            });
            response = await _httpClient.PostAsync(SEARCH_URL, content);
            response.EnsureSuccessStatusCode();
            var contentStream = await response.Content.ReadAsStreamAsync();
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                contentStream = new GZipStream(contentStream, CompressionMode.Decompress);
            }
            else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
            {
                contentStream = new DeflateStream(contentStream, CompressionMode.Decompress);
            }
            using (var reader = new StreamReader(contentStream))
            {
                var htmlContent = await reader.ReadToEndAsync();
                var parsedItems = parser.Parse(htmlContent);
                if (parsedItems.Any())
                {
                    return new()
                    {
                        Value = parsedItems,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new()
                    {
                        IsSuccess = false,
                        Errors = new() { new Error { Message = "No fue posible encontrar archivo con ese número." } }
                    };
                }
            }
        }
    }
}
