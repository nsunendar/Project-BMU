using System.Numerics;
using System.Xml.Linq;

namespace IMSWebApp.Models
{
    public class CustomerList
    {
        public Int64 Id { get; set; }
        public string? BussCode { get; set; }
        public string? PlantCode { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string? CustAddress { get; set; }
        public string City { get; set; }
        public bool Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OwnerName { get; set; }
        public bool PKP { get; set; }
        public string TaxName { get; set; }
        public string TaxAddress { get; set; }
        public string TaxCity { get; set; }
        public string NPWP { get; set; }
        public string? PriceCode { get; set; }
        public DateTime? JoinDate { get; set; }
        public string CustType { get; set; }
        public string? CsTypeDesc { get; set; }
        public string CustSubType { get; set; }
        public string? CsSubTypeDesc { get; set; }
        public string Area { get; set; }
        public string? CsAreaDesc { get; set; }
        public string Salesman { get; set; }
        public string? SlsName { get; set; }
        public Int64 Term { get; set; }
        public string InsertUser { get; set; }
    }


}
