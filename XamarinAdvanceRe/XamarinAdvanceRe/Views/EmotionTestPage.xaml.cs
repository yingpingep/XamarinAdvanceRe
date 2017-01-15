using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using DrawFunction;
using XamarinAdvanceRe.Services;

namespace XamarinAdvanceRe.Views
{
    public partial class EmotionTestPage : ContentPage
    {
        EmotionService emotionService;
        public EmotionTestPage()
        {
            InitializeComponent();

            emotionService = new EmotionService();
            RecognizeBtn.Clicked += RecognizeBtn_Clicked;
        }

        private async void RecognizeBtn_Clicked(object sender, EventArgs e)
        {
            try
            {                
                Draw draw = new Draw();
                var myData = await emotionService.RecognizeEmotionAsync(ImageLocation.Text);
                myData.imageuri = ImageLocation.Text;
                string imageBase64 = await draw.GetDrawedImageAsync(myData);
                DisplayImage.Source = ImageSource.FromStream(() => draw.GetStream(imageBase64));
            }
            catch (Exception)
            {
                await DisplayAlert("ERROR", "Your Image URI is wrong.", "ok");
            }            

        }
    }
}
