using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinAdvanceRe.Data;
using XamarinAdvanceRe.Services;
using Acr.UserDialogs;

namespace XamarinAdvanceRe.Views
{
    public partial class ManageLayout : ContentPage
    {
        List<Users> users = new List<Users>();
        AzureCloundService acs = new AzureCloundService();
        Users selectedUser = null;
        public ManageLayout()
        {
            InitializeComponent();

            AddNew.WidthRequest = Device.OnPlatform(200, 250, 250);
            AddNew.HeightRequest = Device.OnPlatform(60, 80, 80);

            // l, u, r, d
            // i, a, w
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 0, 0);

            init();
            AddNew.Clicked += AddNew_Clicked;            
        }

        private async void AddNew_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPeople(), true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            init();
        }

        private async void init()
        {
            UserDialogs.Instance.ShowLoading("Loading People", MaskType.Black);
            users = await acs.CurrentClient.GetTable<Users>().ToListAsync();
            peopleList.ItemsSource = users;
            UserDialogs.Instance.HideLoading();
        }

        private async void DeletePerson(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            try
            {
                selectedUser = (Users)mi.BindingContext;
                await acs.CurrentTable.DeleteAsync(selectedUser);
                init();
            }
            catch (Exception)
            {
                UserDialogs.Instance.ShowError("Something went wrong, Try again.");
            }
        }
    }
}
