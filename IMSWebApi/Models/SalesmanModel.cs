using System.ComponentModel.DataAnnotations;

namespace IMSWebApi.Models
{
    public class TMSalesman
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string SlsCode { get; set; }
        public string SlsName { get; set; }
        public bool? Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Supervisi { get; set; }
        public string SubType { get; set; }
        public string Area { get; set; }
        public string InsertUser { get; set; }
    }

}
