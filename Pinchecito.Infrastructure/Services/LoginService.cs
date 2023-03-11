using MongoDB.Driver;
using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.Scrapers;

namespace Pinchecito.Infrastructure.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionCookieRepository _sessionCookieRepository;
        private LoginScraper _scraper = new();
        private readonly MongoDatabase _mongoDB;



        public LoginService(IUserRepository userRepository,
                            ISessionCookieRepository sessionCookieRepository,
                            MongoDatabase mongoDB)
        {
            _userRepository = userRepository;
            _sessionCookieRepository = sessionCookieRepository;
            _mongoDB = mongoDB;
        }

        public async Task<Result<IEnumerable<CreationDistrict>>> GetCreationDistricts()
        {
            try
            {
                var districtsCollection = _mongoDB.GetDatabase().GetCollection<CreationDistrict>("creation_districts");
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

        public async Task<Result<User>> GetUser()
        {
            try
            {
                var user = await _userRepository.Get();
                if (user.IsSuccess && user.Value.Cookie is null)
                {
                    var newUserCredentials = await _scraper.NavigateLoginPage(new(user.Value.Username, user.Value.Password, user.Value.DistrictCode));
                    user = newUserCredentials;
                    await _sessionCookieRepository.Save(newUserCredentials.Value.Cookie);
                }
                return user;
            }
            catch (Exception ex)
            {
                return new()
                {
                    IsSuccess = false,
                    Errors = new List<Error>() { new() { Message = ex.Message } }
                };
            }
        }

        public async Task<Result<User>> Login(LoginRequest request)
        {
            var user = await _scraper.NavigateLoginPage(request);
            if (user.IsSuccess)
            {
                user.Value.Password = request.Password;
                user.Value.DistrictCode = request.DistrictCode;
                user = await _userRepository.Save(user.Value);
            }
            return user;
        }
    }
}