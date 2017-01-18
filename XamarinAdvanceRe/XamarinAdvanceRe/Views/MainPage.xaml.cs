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

namespace XamarinAdvanceRe.Views
{
    public partial class MainPage : ContentPage
    {        
        public MainPage()
        {
            InitializeComponent();

            CoverImageButtom.Source = ImageSource.FromFile("buttom.png");            
            CoverImageMiddle.Source = ImageSource.FromFile("middle.png");            
            CoverImage.Source = ImageSource.FromFile("pic.png");

            ManageBtn.Image = "man.png";
            LoginBtn.Image = "camera.png";

            LoginBtn.Clicked += LoginBtn_Clicked;
            ManageBtn.Clicked += ManageBtn_Clicked;
        }

        private async void ManageBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManagePage(), true);
            // await Navigation.PushAsync(new FaceTestPage(), true);
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {          
            UserDialogs.Instance.ShowLoading("Login ...");
            FaceService faceService = new FaceService();
            EmotionService emotionService = new EmotionService();
            AzureCloudService azureclientservice = new AzureCloudService();
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
                    var userDetail = await faceService.GetUserDetailAsync(photo.GetStream());
                    UserDialogs.Instance.ShowLoading("Hi " + userDetail.Name + "\nDetecting emotion ...");
                    DependencyService.Get<ITextToSpeech>().Speak("Hi " + userDetail.Name + " welcome");
                    var emotionRank = await emotionService.GetEmotionRankAsync(photo.GetStream());
                    await azureclientservice.UpdateEmotionAsync(userDetail.PersonId.ToString(), emotionRank[0].Key);
                    await Navigation.PushAsync(new FeelingListPage(userDetail.Name), true);
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.ShowError(ex.Message);
                }
            }
        }
    }
}
