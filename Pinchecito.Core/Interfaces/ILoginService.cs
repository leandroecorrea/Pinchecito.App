using System.Net;

namespace Pinchecito.Core.Interfaces
{
    public interface ILoginService
    {
        Task<Result<User>> GetUser();
        Task<Result<User>> Login(LoginRequest request);
        Task<Result<IEnumerable<CreationDistrict>>> GetCreationDistricts();
    }    
}