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
    class AzureCloundService
    {
        IMobileServiceTable<Users> userTable;
        MobileServiceClient msClient;

        public AzureCloundService()
        {
            msClient = new MobileServiceClient(Constant.ApplicationURL);
            userTable = msClient.GetTable<Users>();
        }

        public MobileServiceClient CurrentClient
        {
            get { return msClient; }
        }

        public IMobileServiceTable<Users> CurrentTable
        {
            get { return userTable; }
        }

        public async void UpdateEmotion(string id, string emotion)
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

        public async Task AddPerson(string name, string picUrl = "", string title = "", string description = "")
        {
            FaceService fs = new FaceService();
            var id = await fs.GetPersonId(name, picUrl);

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

        public async Task<string> UploadImage(MediaFile image)
        {
            var hClient = new HttpClient();
            byte[] imgBytes = new byte[image.GetStream().Length];
            await image.GetStream().ReadAsync(imgBytes, 0, imgBytes.Length);

            var message = await hClient.PostAsync("https://msp11.azurewebsites.net/image", new ByteArrayContent(imgBytes));
            return await message.Content.ReadAsStringAsync();            
        }
    }
}
