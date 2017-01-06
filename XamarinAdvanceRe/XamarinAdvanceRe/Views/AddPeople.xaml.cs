using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinAdvanceRe.Services;
using Acr.UserDialogs;
using Plugin.Media;

namespace XamarinAdvanceRe.Views
{
    public partial class AddPeople : ContentPage
    {
        public AddPeople()
        {
            InitializeComponent();
            AddBtn.Clicked += AddBtn_Clicked;
            AddImage.Clicked += AddImage_Clicked;
        }        

        private async void AddBtn_Clicked(object sender, EventArgs e)
        {
            AzureCloundService acs = new AzureCloundService();

            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
            await acs.AddPerson(name.Text, picUrl.Text, title.Text, description.Text);
            UserDialogs.Instance.HideLoading();

            UserDialogs.Instance.ShowSuccess("Person Added");
            await Navigation.PopAsync(true);
        }

        private async void AddImage_Clicked(object sender, EventArgs e)
        {
            AzureCloundService acs = new AzureCloundService();
            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
            await CrossMedia.Current.Initialize();
            UserDialogs.Instance.HideLoading();

            var photo = await CrossMedia.Current.PickPhotoAsync();

            try
            {
                picUrl.Text = await acs.UploadImage(photo);
            }
            catch (Exception)
            {
                UserDialogs.Instance.ShowError("Upload fail.");
            }
        }        
    }
}
