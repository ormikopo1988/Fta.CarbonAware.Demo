namespace Fta.CarbonAware.Web.Settings
{
    public class CarbonAwareApiSettings
    {
        public const string CarbonAwareApiSectionKey = "CarbonAwareApi";

        public string BaseUrl { get; set; } = default!;
        public string Location { get; set; } = default!;
        public int WindowSize { get; set; } = 30;
    }
}
