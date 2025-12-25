namespace IMSWebApi.Models
{
    public class TMWarehouse
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string WhCode { get; set; }
        public string WhName { get; set; }
        public string WhAddress { get; set; }
        public string City { get; set; }
        public bool? Status { get; set; }
        public decimal? M3Size { get; set; }
        public decimal? CRTSize { get; set; }
        public bool? Stock { get; set; }
        public string InsertUser { get; set; }

    }

}
