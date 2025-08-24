using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSWebApp.Models
{
    public class BOMInventory
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string BOMCode { get; set; }
        public string FGCode { get; set; }
        public string FGName { get; set; }
        public string BOMDescription { get; set; }
    }

    public class BOMMaterial
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string BOMCode { get; set; }
        public string FGCode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public bool? Status { get; set; }
        public decimal? QtyUsage { get; set; }
        public int UnitCode { get; set; }
        public string UnitDesc { get; set; }
        public string RawCode2 { get; set; }
    }

}
