using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pinchecito.Core.Interfaces
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        [Ignore]
        public SessionCookie Cookie { get; set; }
        public DateTime LastRequest { get; set; }
        [Ignore]
        public List<TrackedFile> TrackedFiles { get; set; } = new List<TrackedFile>();
        public string DistrictCode { get; set; }
        public string Password { get; set; }
    }
}
