using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IMSWebApi.Models
{

    public class LoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public int? Batch { get; set; }
    }

    public class MenuModul
    {
        public Int64 Id { get; set; }
        public string? ModulCode { get; set; }
        public string? ModulName { get; set; }        
        public string? Images { get; set; }
        public string? Url { get; set; }
        public Int64 RightAuth { get; set; }
        public string? ImgLink { get; set; }
    }

    public class MenuProces
    {
        public string? UserId { get; set; }
        public string? ModulCode { get; set; }
        public string? ProcesCode { get; set; }
        public string? ProcesName { get; set; }
        public string? ImgLink { get; set; }
    }

    public class MenuProgram
    {
        public string? UserId { get; set; }
        public string? ModulCode { get; set; }
        public string? ProcesCode { get; set; }
        public string? ProgCode { get; set; }
        public string? ProgName { get; set; }
        public string? ImgLink { get; set; }
    }

    public class MenuItem
    {
        public string? UserId { get; set; }
        public string? BussCode { get; set; }
        public string? ProcCode { get; set; }
        public string? ProcName { get; set; }
        public Int64 ProgSeqn { get; set; }
        public Int64 ProgState { get; set; }
        public string? Parent { get; set; }
        public string? Url { get; set; }
        public string? ImgLink { get; set; }

    }

    public class MenuList
    {
        public string? ModulCode { get; set; }
        public string? ProcesCode { get; set; }
        public string? ProcesName { get; set; }
        public string? LinkProc { get; set; }
        public string? ImgLink { get; set; }
        public List<MenuListProgram> MenuPrograms { get; set; } 
    }

    public class MenuListProgram
    {
        public string? ProcesCode { get; set; }
        public string? ProgCode { get; set; }
        public string? ProgName { get; set; }
        public string? LinkUrl { get; set; }
        public string? ImgLink { get; set; }
    }

}
