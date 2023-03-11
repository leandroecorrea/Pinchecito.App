using HtmlAgilityPack;
using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.HtmlParsers;
using System.IO.Compression;
using System.Net;

namespace Pinchecito.Infrastructure.Scrapers
{
    public class LoginScraper
    {
        private static string LOGIN_URL = "https://mev.scba.gov.ar/loguin.asp";
        private CookieContainer cookieContainer;
        private HttpClientHandler httpClientHandler;

        public LoginScraper()
        {
            cookieContainer = new CookieContainer();
            httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
            };
        }


        public async Task<Result<User>> NavigateLoginPage(LoginRequest loginRequest)
        {
            try
            {
                var user = await Authenticate(loginRequest);
                if (user == null)
                {
                    return new Result<User>()
                    {
                        Value = new EmptyUser(),
                        IsSuccess = false,
                        Errors = new List<Error>
                        {
                            new(){ Message = "El usuario no pudo ser obtenido de la web luego de una navegación exitosa" }
                        }
                    };
                }
                return new()
                {
                    Value = user,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Value = new EmptyUser(),
                    IsSuccess = false,
                    Errors = new List<Error>
                        {
                            new(){ Message = ex.Message }
                        }
                };
            }
        }

        private async Task<User> Authenticate(LoginRequest loginRequest)
        {
            var cookie = await GetCookie();
            var httpClient = new HttpClient(httpClientHandler);
            AddHeadersForLogin(httpClient);
            FormUrlEncodedContent content = CreateContentFor(loginRequest);
            var response = await httpClient.PostAsync("https://mev.scba.gov.ar/loguin.asp?familiadepto=", content);
            response.EnsureSuccessStatusCode();
            Stream loginPage = await GetLoginPageFrom(response);
            var user = await ParseUserFrom(loginPage);
            user.Cookie = cookie;
            user.LastRequest = DateTime.Now;
            return user;
        }

        private static async Task<User> ParseUserFrom(Stream loginPage)
        {
            using (var reader = new StreamReader(loginPage))
            {
                var loginContent = await reader.ReadToEndAsync();
                var html = new HtmlDocument();
                html.LoadHtml(loginContent);
                var loginNode = html.DocumentNode.SelectNodes("//p[@class='whiteleft']")
                                                 .Where(node => node.InnerText.Contains("UsuarioMEV"))
                                                 .First()
                                                 .SelectNodes("//b");
                var useraccount = loginNode.First().InnerText;
                var fullName = loginNode.Last().InnerText;
                return new User { Username = useraccount, Fullname = fullName };
            }
        }

        private static async Task<Stream> GetLoginPageFrom(HttpResponseMessage response)
        {
            var loginPage = await response.Content.ReadAsStreamAsync();
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                loginPage = new GZipStream(loginPage, CompressionMode.Decompress);
            }
            else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
            {
                loginPage = new DeflateStream(loginPage, CompressionMode.Decompress);
            }

            return loginPage;
        }

        private static FormUrlEncodedContent CreateContentFor(LoginRequest loginRequest)
        {
            return new FormUrlEncodedContent(new[]
            {
                     new KeyValuePair<string, string>("usuario", loginRequest.Username),
                     new KeyValuePair<string, string>("clave", loginRequest.Password),
                     new KeyValuePair<string, string>("DeptoRegistrado", loginRequest.DistrictCode)
                });
        }

        private static void AddHeadersForLogin(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Referer", "https://mev.scba.gov.ar/loguin.asp");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,es;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        }

        private async Task<SessionCookie> GetCookie()
        {
            var httpClient = new HttpClient(httpClientHandler);
            var httpResponse = await httpClient.GetAsync(LOGIN_URL);
            httpResponse.EnsureSuccessStatusCode();
            var cookieRetrieved = cookieContainer.GetCookies(httpResponse.RequestMessage.RequestUri).FirstOrDefault();
            var sessionCookie = new SessionCookie
            {
                Name = cookieRetrieved.Name,
                Value = cookieRetrieved.Value,
            };
            var cookieForRequests = new Cookie(cookieRetrieved.Name, cookieRetrieved.Value, path: "/", sessionCookie.COOKIE_DOMAIN());
            httpClientHandler.CookieContainer.Add(cookieForRequests);
            return sessionCookie;
        }
    }
}
