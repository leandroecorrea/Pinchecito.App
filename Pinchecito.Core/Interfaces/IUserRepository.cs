namespace Pinchecito.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<User>> Save(User user);
        Task<Result<User>> Get();
    }
}