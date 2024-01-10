using App.Maui.Pages;
namespace App.Maui
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ChartPage();
        }
    }
}
