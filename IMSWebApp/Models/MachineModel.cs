namespace IMSWebApp.Models
{
    public class TMMachine
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string MachineCode { get; set; }
        public string MachineName { get; set; }
        public bool? Status { get; set; }
        public DateTime? BuyDate { get; set; }
        public DateTime? MaintDate { get; set; }
        public decimal? Usage { get; set; }
        public string InsertUser { get; set; }
    }
}
