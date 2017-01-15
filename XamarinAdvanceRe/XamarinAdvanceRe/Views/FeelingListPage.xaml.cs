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
    public partial class FeelingListPage : ContentPage
    {
        public FeelingListPage(string name)
        {
            InitializeComponent();

            Title = "Hello " + name;
            UserDialogs.Instance.ShowLoading("Loading person", MaskType.Black);
            init();
            UserDialogs.Instance.HideLoading();
        }

        private async void init()
        {
            AzureCloudService azureCloudService = new AzureCloudService();
            List<MSP> users = await azureCloudService.CurrentClient.GetTable<MSP>().ToListAsync();
            foreach (var user in users)
            {
                TimeSpan last = DateTime.Now - user.updatedAt;
                string lastString;
                if (last.TotalMinutes < 60)
                {
                    lastString = ((int)last.TotalMinutes).ToString() + " mins ago.";
                }
                else
                {
                    lastString = user.updatedAt.ToString("M/d h:mm tt");
                }
                user.emotionImg = GetEmotionImg(user.emotion);
                user.emotion = " last onlne " + lastString;
            }
            FeelingShow.ItemsSource = users;
        }

        private string GetEmotionImg(string emotion)
        {
            string imgName;
            switch (emotion)
            {
                case "Anger":
                    imgName = "anger.png";
                    break;
                case "Contempt":
                    imgName = "contempt.png";
                    break;
                case "Disgust":
                    imgName = "disgust.png";
                    break;
                case "Fear":
                    imgName = "fear.png";
                    break;
                case "Happiness":
                    imgName = "happiness.png";
                    break;
                case "Neutral":
                    imgName = "neutral.png";
                    break;
                case "Sadness":
                    imgName = "sadness.png";
                    break;
                default:
                    // "Surprise"
                    imgName = "surprise.png";
                    break;
            }

            return imgName;
        }
    }
}
