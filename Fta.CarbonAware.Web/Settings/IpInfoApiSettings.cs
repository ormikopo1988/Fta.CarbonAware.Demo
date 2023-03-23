namespace Fta.CarbonAware.Web.Settings
{
    public class IpInfoApiSettings
    {
        public const string IpInfoApiSectionKey = "IpInfoApi";

        public string BaseUrl { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
    }
}
