using SQLite;

namespace Pinchecito.Core.Interfaces
{
    public class TrackedFile
    {
        public int Id { get; set; }
        public string CaseCaption { get; set; }
        public string CaseDescription { get; set; }        
        public bool IsParalized { get; set; }
        [Ignore]
        public CourtOrder LastOrder { get; set; }
    }
}