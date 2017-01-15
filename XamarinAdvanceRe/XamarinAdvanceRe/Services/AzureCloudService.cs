using Acr.UserDialogs;
using Microsoft.ProjectOxford.Face;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinAdvanceRe.Data;

namespace XamarinAdvanceRe.Services
{
    class AzureCloudService
    {
        IMobileServiceTable<Users> userTable;
        MobileServiceClient mobileserviceclient;

        public AzureCloudService()
        {
            mobileserviceclient = new MobileServiceClient(Constant.ApplicationURL);
            userTable = mobileserviceclient.GetTable<Users>();
        }

        public MobileServiceClient CurrentClient
        {
            get { return mobileserviceclient; }
        }

        public IMobileServiceTable<Users> CurrentTable
        {
            get { return userTable; }
        }

        public async Task UpdateEmotionAsync(string id, string emotion)
        {
            try
            {
                // Find the person with FaceAPI's id from personTable
                IMobileServiceTableQuery<Users> query = userTable.Where(p => p.Personid == id);
                List<Users> queryResult = await query.ToListAsync();

                if (queryResult.Count > 0)
                {
                    queryResult[0].emotion = emotion;
                    await userTable.UpdateAsync(queryResult[0]);
                }
                else
                {
                    UserDialogs.Instance.Toast("Can't find your data.");
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Unable to update your emotion.");
            }
        }

        public async Task AddPersonAsync(string name, string picUrl = "", string title = "", string description = "")
        {
            FaceService faceservice = new FaceService();
            var id = await faceservice.GetPersonIdAsync(name, picUrl);

            Users data = new Users
            {
                Name = name,
                Title = title,
                Description = description,
                Personid = id,
                Image = picUrl,
                emotion = "Happiness"
            };

            await userTable.InsertAsync(data);
        }

        /*  
            upload to msp azure blob account
            server code: https://github.com/oscar60310/mspimg
        */

        public async Task<string> UploadImageAsync(MediaFile image)
        {
            var httpclient = new HttpClient();
            byte[] imgBytes = new byte[image.GetStream().Length];
            await image.GetStream().ReadAsync(imgBytes, 0, imgBytes.Length);

            var message = await httpclient.PostAsync(Constant.UploadImgAPI, new ByteArrayContent(imgBytes));
            return await message.Content.ReadAsStringAsync();            
        }
    }
}
