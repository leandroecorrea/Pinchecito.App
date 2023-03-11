using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.DataAccess;

namespace Pinchecito.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PinchecitoDatabase _db;
        private int maxSessionMinutes = 5;

        public UserRepository(PinchecitoDatabase db)
        {
            _db = db;
        }

        public async Task<Result<User>> Get()
        {
            var db = await _db.GetDatabase();
            var user = await db.Table<User>().FirstOrDefaultAsync();
            if (user is null)
            {
                return new()
                {
                    Value = new EmptyUser(),
                    Errors = new List<Error> { new Error { Message = "No se encontró usuario" } },
                    IsSuccess = false
                };
            }
            if (DateTime.Now.Subtract(user.LastRequest).TotalMinutes < maxSessionMinutes)
            {
                user.Cookie = await db.Table<SessionCookie>()
                                      .OrderByDescending(x => x.CreationDate)
                                      .FirstOrDefaultAsync();
            }
            user.TrackedFiles = await db.Table<TrackedFile>().ToListAsync();
            return new Result<User>
            {
                Value = user,
                Errors = new List<Error>(),
                IsSuccess = true
            };
        }

        public async Task<Result<User>> Save(User user)
        {
            user.LastRequest = DateTime.Now;
            var db = await _db.GetDatabase();
            await db.InsertAsync(user);
            await db.InsertAsync(user.Cookie);
            await db.InsertAllAsync(user.TrackedFiles);
            return new Result<User>
            {
                Value = user,
                IsSuccess = true
            };
        }        
    }
}
