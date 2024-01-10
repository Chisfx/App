namespace App.Maui.Helpers
{
    public static class Router
    {
        public static string UrlBase => Microsoft.Maui.Controls.Application.Current.Resources["UrlBase"].ToString();
        public static string UrlUser => Microsoft.Maui.Controls.Application.Current.Resources["User"].ToString();
    }
}
