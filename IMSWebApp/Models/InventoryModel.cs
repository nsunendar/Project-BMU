namespace IMSWebApp.Models
{
    public class InventoryList
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public string? RelasiCode { get; set; }
        public string? CSINVName { get; set; }
        public bool InvStatus { get; set; }
        public bool Discontinue { get; set; }
        public string? VendorCode { get; set; }
        public string? VendorName { get; set; }
        public string? Barcode { get; set; }
        public string InvType { get; set; }
        public string InvTypeDesc { get; set; }
        public string InvSubType { get; set; }
        public string? InvSubTypeDesc { get; set; }
        public string? Brand { get; set; }
        public string? BrandDesc { get; set; }
        public string? SmallUnit { get; set; }
        public string? SmallUnitDesc { get; set; }
        public string? LargeUnit { get; set; }
        public string? LargeUnitDesc { get; set; }
        public decimal? Crt { get; set; }
        public decimal? Fra { get; set; }
        public decimal? Norm { get; set; }
        public int Process { get; set; }
        public decimal? NoMachine { get; set; }
        public decimal? People { get; set; }
        public decimal? CodeBOM { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? BuyPrice { get; set; }
        public string InsertUser { get; set; }
    }

    public class TMInvType
    {
        public string InvType { get; set; }
        public string InvTypeDesc { get; set; }
        public string InsertUser { get; set; }
    }

    public class TMInvSubType
    {
        public string InvType { get; set; }
        public string InvSubType { get; set; }
        public string InvSubTypeDesc { get; set; }
        public string InsertUser { get; set; }
    }
    public class TMBrand
    {
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string InsertUser { get; set; }
    }
}
