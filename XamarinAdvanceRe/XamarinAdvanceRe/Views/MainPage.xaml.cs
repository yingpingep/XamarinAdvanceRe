using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using XamarinAdvanceRe.Services;
using Microsoft.ProjectOxford.Common;

namespace XamarinAdvanceRe.Views
{
    public partial class MainPage : ContentPage
    {        
        public MainPage()
        {
            InitializeComponent();

            CoverImage.Source = ImageSource.FromUri(new Uri("https://scontent-tpe1-1.xx.fbcdn.net/v/t1.0-9/14492398_1288636481167411_4214539296251359956_n.png?oh=e0ec2d72b1e79a2f481a53d7149deda0&oe=586449C4"));
            LoginBtn.Clicked += LoginBtn_Clicked;
            ManageBtn.Clicked += ManageBtn_Clicked;
        }

        private async void ManageBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageLayout(), true);
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Login ...");
            FaceService fs = new FaceService();
            EmotionService es = new EmotionService();
            AzureCloundService acs = new AzureCloundService();
            await CrossMedia.Current.Initialize();
            UserDialogs.Instance.HideLoading();


            MediaFile photo;


            #region Camera
            /*
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "MSP",
                    Name = Guid.NewGuid().ToString() + ".jpg"
                });
            }
            else
            {
                photo = await CrossMedia.Current.PickPhotoAsync();
            }
            */
            #endregion

            // Easy to test
            photo = await CrossMedia.Current.PickPhotoAsync();

            if (photo != null)
            {
                CoverImage.Source = ImageSource.FromFile(photo.Path);
                
                long size = photo.GetStream().Length;
                if (size > Constant.ImageSize)
                {
                    UserDialogs.Instance.ShowError("Image size is too large.");
                    return;
                }
                
                try
                {
                    var userDetail = await fs.GetUserDetail(photo.GetStream());
                    UserDialogs.Instance.ShowLoading("Hi " + userDetail.Name + "\nDetecting emotion ...");
                    var emotionRank = await es.GetEmotionRank(photo.GetStream());
                    acs.UpdateEmotion(userDetail.PersonId.ToString(), emotionRank[0].Key);
                    await Navigation.PushAsync(new FeelingList(userDetail.Name), true);
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.ShowError(ex.Message);
                }
            }
        }
    }
}
