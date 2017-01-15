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
    public partial class ManagePage : ContentPage
    {
        List<MSP> users = new List<MSP>();
        AzureCloudService azurecloudservice = new AzureCloudService();
        MSP selectedUser = null;
        public ManagePage()
        {
            InitializeComponent();

            // l, u, r, d
            // i, a, w
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 0, 0);

            ToolbarItems.Add(new ToolbarItem("Add Person", "add.png", async () =>
            {
                await Navigation.PushAsync(new AddMemberPage(), true);
            }));

            init();          
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            init();
        }

        private async void init()
        {
            UserDialogs.Instance.ShowLoading("Loading People", MaskType.Black);
            users = await azurecloudservice.CurrentClient.GetTable<MSP>().ToListAsync();          
            MemberList.ItemsSource = users;
            UserDialogs.Instance.HideLoading();
        }

        private async void DeleteMemberAsync(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            try
            {
                selectedUser = (MSP)mi.BindingContext;
                await azurecloudservice.CurrentTable.DeleteAsync(selectedUser);
                init();
            }
            catch (Exception)
            {
                UserDialogs.Instance.ShowError("Something went wrong, Try again.");
            }
        }
    }
}
