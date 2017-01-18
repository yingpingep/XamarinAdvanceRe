using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using DrawFunction;
using XamarinAdvanceRe.Services;
using System.Net.Http;

namespace XamarinAdvanceRe.Views
{
    public partial class FaceTestPage : ContentPage
    {
        FaceService faceService;
        public FaceTestPage()
        {
            InitializeComponent();

            faceService = new FaceService();
            DectedBtn.Clicked += DectedBtn_Clicked;
            IdentifyBtn.Clicked += IdentifyBtn_Clicked;
        }        

        private async void DectedBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Draw draw = new Draw();

                var myData = await faceService.DetectFaceAsync(ImageLocation.Text);
                myData.imageuri = ImageLocation.Text;
                string imageBase64 = await draw.GetDrawedImageAsync(myData);
                DisplayImage.Source = ImageSource.FromStream(() => draw.GetStream(imageBase64));
                await faceService.GetPersonIdAsync(UserName.Text, ImageLocation.Text);
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.ToString(), "ok");
            }
        }

        private async void IdentifyBtn_Clicked(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            var loginUser = await faceService.GetUserDetailAsync(await httpClient.GetStreamAsync(ImageLocation.Text));
            Title = loginUser.Name.ToString();
        }
    }
}
