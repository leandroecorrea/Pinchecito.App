using MongoDB.Bson;

namespace Pinchecito.Core.Interfaces
{
    public class CreationDistrict
    {        
        public ObjectId Id { get; set; }
        public string CreationCode { get; set; }
        public string CourtName { get; set; }
    }
}
