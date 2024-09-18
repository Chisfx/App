namespace App.Maui.Helpers
{
    /// <summary>
    /// Represents a helper class for routing.
    /// </summary>
    public static class Router
    {
        /// <summary>
        /// Gets the base URL.
        /// </summary>
        #if DEBUG
        public static string UrlBase => DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7052" : Application.Current.Resources["UrlBase"].ToString();
        #else
        public static string UrlBase => Application.Current.Resources["UrlBase"].ToString();
        #endif

        /// <summary>
        /// Gets the user URL.
        /// </summary>
        public static string UrlUser => Application.Current.Resources["User"].ToString();
    }
}
