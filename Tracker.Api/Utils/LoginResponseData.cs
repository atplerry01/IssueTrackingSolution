namespace Tracker.Api.Utils
{
    public class LoginResponseData
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string client_id { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string roles { get; set; }
        public bool isAdmin { get; set; }
    }
}