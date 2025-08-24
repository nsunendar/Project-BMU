namespace IMSWebApi.Models
{
    public class SPParameters
    {
        public string? USERNAME { get; set; }
        public string? ACTIVEBRANCH { get; set; }
        public string? DATA { get; set; }
    }

    public class UserBranch
    {
        public Int64 Id { get; set; }
        public string? BranchCode { get; set; }
        public string? BranchName { get; set; }
    }



}
