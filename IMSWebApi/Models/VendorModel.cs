namespace IMSWebApi.Models
{
    public class TMVendor
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string City { get; set; }
        public bool? Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OwnerName { get; set; }
        public bool? PKP { get; set; }
        public string TaxName { get; set; }
        public string TaxAddress { get; set; }
        public string TaxCity { get; set; }
        public string NPWP { get; set; }
        public string VendorType { get; set; }
        public string VndTypeDesc { get; set; }
        public int? Term { get; set; }
        public int? LeadTime { get; set; }
        public string PriceCode { get; set; }

        public string InsertUser { get; set; }
    }
}
