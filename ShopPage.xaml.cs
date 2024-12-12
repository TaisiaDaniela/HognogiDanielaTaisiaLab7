using HognogiDanielaTaisiaLab7.Models;
using Plugin.LocalNotification;
using HognogiDanielaTaisiaLab7.Data;

namespace HognogiDanielaTaisiaLab7;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        if (shop == null || shop.ID == 0)
        {
            await DisplayAlert("Error", "Shop not found or already deleted.", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the shop '{shop.ShopName}'?", "Yes", "No");

        if (confirm)
        {
            // ?tergere din baza de date
            await App.Database.DeleteShopAsync(shop);
            await DisplayAlert("Deleted", "The shop has been deleted successfully.", "OK");

            // Navigare înapoi la pagina anterioar?
            await Navigation.PopAsync();
        }
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        //var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };
        //var shoplocation = locations?.FirstOrDefault();
        var shoplocation = new Location(46.7492379, 23.5745597);//pentru Windows Machine 

        //var myLocation = await Geolocation.GetLocationAsync();
        var myLocation = new Location(46.7731796289, 23.6213886738);
        //pentru Windows Machine */
        var distance = myLocation.CalculateDistance(myLocation, DistanceUnits.Kilometers);
        if (distance < 5)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            await LocalNotificationCenter.Current.Show(request);
        }



        await Map.OpenAsync(shoplocation, options);
    }
}