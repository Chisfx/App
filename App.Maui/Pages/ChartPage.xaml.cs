using App.Domain.DTOs;
using App.Maui.Abstractions;
using App.Maui.Helpers;
using Microcharts;
using SkiaSharp;

namespace App.Maui.Pages;

public partial class ChartPage : ContentPage
{
	public ChartPage()
	{
		InitializeComponent();
        LoadEntriesAsync();
    }

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

    private void OnButtonClicked(object sender, EventArgs e)
    {
        LoadEntriesAsync();
    }
}