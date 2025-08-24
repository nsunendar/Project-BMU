namespace IMSWebApp.Models
{
    public class SPParameters
    {
        public string? USERNAME { get; set; }
        public string? ACTIVEBRANCH { get; set; }
        public string? DATA { get; set; }
    }

    public class UserDetail
    {
        public int? ID { get; set; }
        public string? USERNAME { get; set; }
        public string? EMAIL { get; set; }
        public string? ROLEID { get; set; }
        public string? ROLENAME { get; set; }
        public string? COMPANYID { get; set; }
        public string? COMPANYNAME { get; set; }

    }

    public class UserBranch
    {
        public Int64 Id { get; set; }
        public string? BranchCode { get; set; }
        public string? BranchName { get; set; }
    }

    public class UserModul
    {
        public Int32 USERID { get; set; }
        public string USERNAME { get; set; }
        public string MDUL_CODE { get; set; }
        public string MDUL_NAME { get; set; }
    }

    public class UserProgram
    {
        public Int32 USERID { get; set; }
        public string USERNAME { get; set; }
        public string MDUL_CODE { get; set; }
        public string PROG_CODE { get; set; }
        public string PROG_NAME { get; set; }
        public string URL_LINK { get; set; }
    }

    public class UserLoginDashboard
    {
        public int? ID { get; set; }
        public string USERNAME { get; set; }
        public string FULLNAME { get; set; }
        public string EMAIL { get; set; }
        public string COMPANY { get; set; }
    }

}
