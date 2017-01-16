﻿using System;
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
    public partial class AddMemberPage : ContentPage
    {
        AzureCloudService azureCloudService;
        public AddMemberPage()
        {
            InitializeComponent();

            azureCloudService = new AzureCloudService();
            AddBtn.Clicked += AddBtn_Clicked;
            AddImage.Clicked += AddImage_Clicked;
        }        

        private async void AddBtn_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
            await azureCloudService.AddPersonAsync(name.Text, picUrl.Text, title.Text, description.Text);
            UserDialogs.Instance.HideLoading();

            UserDialogs.Instance.ShowSuccess("Person Added");
            await Navigation.PopAsync(true);
        }

        private async void AddImage_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
            await CrossMedia.Current.Initialize();
            UserDialogs.Instance.HideLoading();

            var photo = await CrossMedia.Current.PickPhotoAsync();
            if (picUrl.Text == null)
            {
                return;
            }

            try
            {
                picUrl.Text = await azureCloudService.UploadImageAsync(photo);
            }
            catch (Exception)
            {
                UserDialogs.Instance.ShowError("Upload fail.");
            }
        }        
    }
}