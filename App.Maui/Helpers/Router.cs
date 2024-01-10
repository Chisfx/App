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
        public static string UrlBase => Microsoft.Maui.Controls.Application.Current.Resources["UrlBase"].ToString();

        /// <summary>
        /// Gets the user URL.
        /// </summary>
        public static string UrlUser => Microsoft.Maui.Controls.Application.Current.Resources["User"].ToString();
    }
}
