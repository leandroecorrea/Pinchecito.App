using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinchecito.Core.Interfaces
{
    public interface ISessionCookieRepository
    {
        Task<Result<SessionCookie>> Save(SessionCookie sessionCookie);
    }
}
