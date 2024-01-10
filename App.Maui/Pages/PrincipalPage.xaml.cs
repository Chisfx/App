using App.Domain.DTOs;
using App.Maui.Abstractions;
using App.Maui.Helpers;
namespace App.Maui.Pages;
/// <summary>
/// Represents the principal page of the application.
/// </summary>
public partial class PrincipalPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrincipalPage"/> class.
    /// </summary>
    public PrincipalPage()
    {
        InitializeComponent();
        SetUser();
    }

    /// <summary>
    /// Sets the user list view with data from the server.
    /// </summary>
    private async void SetUser()
    {
        UserListView.ItemsSource = await ClientHttp.GetAll<UserModel>($"{Router.UrlUser}/getall");
    }
}
