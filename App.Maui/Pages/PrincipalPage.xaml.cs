using App.Domain.DTOs;
using App.Maui.Abstractions;
using App.Maui.Helpers;
namespace App.Maui.Pages;
public partial class PrincipalPage : ContentPage
{
    public PrincipalPage()
	{
		InitializeComponent();
        SetUser();
    }

    private async void SetUser()
    {
        UserListView.ItemsSource = await ClientHttp.GetAll<UserModel>($"{Router.UrlUser}/getall");
    }
}