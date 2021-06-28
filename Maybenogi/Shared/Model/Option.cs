namespace Maybenogi.Shared.Model
{
    public class Option
    {
        public EBrowserType BrowserType { get; set; } = EBrowserType.Chrome;
        public int BrowserWidth { get; set; } = 400;
        public int BrowserHeight { get; set; } = 600;
        public bool Headless { get; set; } = false;
        public string ChromeCachePath { get; set; } = "C:\\Users\\Public\\chrome";
        public string FirefoxCachePath { get; set; } = "C:\\Users\\Public\\firefox";
    }
}