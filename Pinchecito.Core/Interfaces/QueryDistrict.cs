using MongoDB.Bson;

namespace Pinchecito.Core.Interfaces
{
    public class QueryDistrict
    {
        public ObjectId Id { get; set; }
        public string QueryCode { get; set; }
        public string DistrictName { get; set; }
        public List<Court> Courts { get; set; } = new();
    }
}