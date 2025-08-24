namespace IMSWebApp.Models
{
    public class TMProcess
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string ProcCode { get; set; }
        public int? NoProcess { get; set; }
        public string CodeMachine { get; set; }
        public decimal? QtyUsage { get; set; }
        public decimal? TarProDay { get; set; }
        public decimal? TarProHours { get; set; }
        public DateTime? DateProcess { get; set; }
    }
}
