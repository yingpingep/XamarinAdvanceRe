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
    public partial class EasyFace : ContentPage
    {
        FaceService faceservice;
        public EasyFace()
        {
            InitializeComponent();

            faceservice = new FaceService();
            DectedBtn.Clicked += DectedBtn_Clicked;
            IdentifyBtn.Clicked += IdentifyBtn_Clicked;
        }        

        private async void DectedBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Draw draw = new Draw();

                var mydata = await faceservice.DetectFaceAsync(ImageLocation.Text);
                mydata.imageuri = ImageLocation.Text;
                string imageBase64 = await draw.GetDrawedImageAsync(mydata);
                DisplayImage.Source = ImageSource.FromStream(() => draw.GetStream(imageBase64));   
            }
            catch (Exception)
            {
                await DisplayAlert("ERROR", "Your Image URI is wrong.", "ok");
            }
        }

        private async void IdentifyBtn_Clicked(object sender, EventArgs e)
        {
            HttpClient httpclient = new HttpClient();
            var loginuser = await faceservice.GetUserDetailAsync(await httpclient.GetStreamAsync(ImageLocation.Text));
            Title = loginuser.Name.ToString();
        }
    }
}
