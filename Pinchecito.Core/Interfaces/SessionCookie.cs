using SQLite;
using System.Net;

namespace Pinchecito.Core.Interfaces
{
    public class SessionCookie
    {        
        public string COOKIE_DOMAIN() => "mev.scba.gov.ar";
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime CreationDate { get; set; }

        
        public Cookie GetCookie()
        {
            return new(Name, Value, path:"/", COOKIE_DOMAIN());
        }
    }
}
