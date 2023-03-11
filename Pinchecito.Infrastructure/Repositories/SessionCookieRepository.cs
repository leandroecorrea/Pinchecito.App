using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.DataAccess;

namespace Pinchecito.Infrastructure.Repositories
{
    public class SessionCookieRepository : ISessionCookieRepository
    {
        private readonly PinchecitoDatabase _db;

        public SessionCookieRepository(PinchecitoDatabase db)
        {
            _db = db;
        }

        public async Task<Result<SessionCookie>> Save(SessionCookie sessionCookie)
        {
            var db = await _db.GetDatabase();
            await db.Table<SessionCookie>().DeleteAsync(x => true);
            await db.InsertAsync(sessionCookie);
            return new()
            {
                Value = sessionCookie,
                IsSuccess= true
            };
        }
    }
}
