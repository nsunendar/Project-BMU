namespace IMSWebApp.Services
{
    public interface IGlobalService
    {
        string CurrentBranch { get; set; }
    }
    public class GlobalService : IGlobalService
    {
        public string CurrentBranch { get; set; } = "0";
    }
}
