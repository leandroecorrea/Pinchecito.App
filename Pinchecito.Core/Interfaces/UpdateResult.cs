namespace Pinchecito.Core.Interfaces
{
    public class UpdateResult
    {
        public bool IsAnUpdate { get; set; }
        public CourtOrder Update { get; set; }
    }
}
