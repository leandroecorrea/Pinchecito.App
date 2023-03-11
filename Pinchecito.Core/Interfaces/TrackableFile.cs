namespace Pinchecito.Core.Interfaces
{
    public class TrackableFile
    {
        public string Id { get; set; }
        public string CaseCaption { get; set; }
        public string CaseInternalCode { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateMessage => LastUpdate == DateTime.MinValue ? "No disponible" : LastUpdate.ToString("dd/MM/yyyy");
        public string InitialDateMessage => InitialDate == DateTime.MinValue ? "No disponible" : InitialDate.ToString("dd/MM/yyyy");
        public string FileURL { get; set; }
        public bool IsParalized { get; set; }
        public string Status { get; set; }
        public string ReceivedId { get; set; }
        public DateTime InitialDate { get; set; } = default;
        public string LastOrderTitle { get; set; }
    }
}