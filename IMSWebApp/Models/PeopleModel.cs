using System.ComponentModel.DataAnnotations;

namespace IMSWebApp.Models
{
    public class TMPeople
    {
        public int Id { get; set; }
        public string BussCode { get; set; }
        public string PlantCode { get; set; }
        public string PeopleCode { get; set; }
        public string PeopleName { get; set; }
        public bool? Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? JoinDate { get; set; }
        public string PeopleJob { get; set; }
        public string PeopleGroup { get; set; }
    }

}
