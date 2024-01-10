using App.Domain.DTOs;
using App.Maui.Abstractions;
using App.Maui.Helpers;
using Microcharts;
using SkiaSharp;

namespace App.Maui.Pages;

/// <summary>
/// Represents the ChartPage class.
/// </summary>
public partial class ChartPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChartPage"/> class.
    /// </summary>
    public ChartPage()
    {
        InitializeComponent();
        LoadEntriesAsync();
    }

    /// <summary>
    /// Loads the entries asynchronously.
    /// </summary>
    private async void LoadEntriesAsync()
    {
        try
        {
            var listAge = await ClientHttp.GetAll<GroupAgeModel>($"{Router.UrlUser}/GetGroupAgeTop/{5}");
            var listHost = await ClientHttp.GetAll<GroupHostModel>($"{Router.UrlUser}/GetGroupHostTop/{5}");

            if (listAge != null)
            {
                List<ChartEntry> entryListAge = listAge.Select(x => new ChartEntry(x.Count)
                {
                    Label = x.Age.ToString(),
                    ValueLabel = $"{x.Count}%",
                    Color = SKColor.Parse(x.Color)
                }).ToList();


                donutChart.Chart = new DonutChart()
                {
                    LabelTextSize = 40f,
                    Entries = entryListAge
                };
            }

            if (listHost != null)
            {
                List<ChartEntry> entryListHost = listHost.Select(x => new ChartEntry(x.Count)
                {
                    Label = x.Host.ToString(),
                    ValueLabel = $"{x.Count}%",
                    Color = SKColor.Parse(x.Color)
                }).ToList();

                pieChart.Chart = new PieChart()
                {
                    LabelTextSize = 40f,
                    Entries = entryListHost
                };
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Handles the button clicked event.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="e">The event arguments.</param>
    private void OnButtonClicked(object sender, EventArgs e)
    {
        LoadEntriesAsync();
    }
}
