using Claudiu_Cojocaru_Lab11.Models;

namespace Claudiu_Cojocaru_Lab11
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            slist.Date = DateTime.UtcNow;
            
            bool isNewItem = slist.ID == 0;
            
            await App.Database.SaveShopListAsync(slist, isNewItem);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            await App.Database.DeleteShopListAsync(slist);
            await Navigation.PopAsync();
        }
    }
}
