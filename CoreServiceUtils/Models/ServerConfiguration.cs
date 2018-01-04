namespace CoreServiceUtils.Models
{
    public enum ConnectionType
    {
        NetTcp,
        BasicHttp,
        BasicHttps
    }

    public class ServerConfiguration
    {
        public string Server { get; set; }

        public ConnectionType Type { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
