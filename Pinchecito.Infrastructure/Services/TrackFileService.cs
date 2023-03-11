using MongoDB.Driver;
using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.Scrapers;

namespace Pinchecito.Infrastructure.Services
{
    public class TrackFileService : ITrackFileService
    {
        private readonly MongoDatabase _mongoDB;
        private readonly SearchPageScraper _scraper;
        private readonly ILoginService _loginService;

        public TrackFileService(MongoDatabase mongoDB, SearchPageScraper scraper, ILoginService loginService)
        {
            _mongoDB = mongoDB;
            _scraper = scraper;
            _loginService = loginService;
        }

        public async Task<Result<IEnumerable<QueryDistrict>>> GetDistrictsForTracking()
        {
            try
            {
                var districtsCollection = _mongoDB.GetDatabase().GetCollection<QueryDistrict>("query_districts");
                var districts = await districtsCollection.Find(_ => true)
                                                         .ToListAsync();
                return new()
                {
                    Value = districts,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Errors = new List<Error>() { new() { Message = ex.Message } },
                    IsSuccess = false
                };
            }

        }

        public async Task<Result<IEnumerable<TrackableFile>>> GetTrackablesFor(GetTrackableFileByIdRequest request)
        {
            var user = await _loginService.GetUser();
            if(user.IsSuccess)
            {
                return await _scraper.SearchFilesAsync(request, user.Value.Cookie.GetCookie());               
            }
            else
            {
                return new()
                {
                    IsSuccess = false,
                    Errors = user.Errors
                                 .Append(new(){ Message = "No fue posible obtener el usuario"})
                                 .ToList()
                };
            }
        }

        public Task<Result<TrackedFile>> TrackFile(GetTrackedFilesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}