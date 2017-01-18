using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using System.IO;
using Microsoft.ProjectOxford.Face.Contract;
using DrawFunction;

namespace XamarinAdvanceRe.Services
{
    class FaceService
    {
        FaceServiceClient faceServiceClient;
        public FaceService()
        {
            faceServiceClient = new FaceServiceClient(Constant.FaceApiKey);

            // make sure the people group exist (200 if created successfully, 409 if existed)
            faceServiceClient.CreatePersonGroupAsync(Constant.DefaultPersonGroupId, Constant.DefaultPersonGroupName);
        }

        public async Task<MyDataType> DetectFaceAsync(string imageUrl)
        {
            FaceAttributeType[] fats = new FaceAttributeType[]
            {
                FaceAttributeType.Age
            };

            Face[] faceResult;
            try
            {
                faceResult = await faceServiceClient.DetectAsync(imageUrl, true, false, fats);
            }
            catch (FaceAPIException faceapiException)
            {
                throw new Exception("Your image size need smaller than 4MB OR You're out of the free quota.", faceapiException);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            MyDataType mydata = new MyDataType();
            foreach (var item in faceResult)
            {
                mydata.rects.Add(new Rect(item.FaceRectangle.Left, item.FaceRectangle.Top, item.FaceRectangle.Width));
                mydata.ages.Add(new Age(item.FaceAttributes.Age));
            }

            return mydata;
        }

        public async Task<string> GetPersonIdAsync(string name, string picUrl)
        {
            /*
             * Create person and get PersonId
             * -> add image
             * -> train
             * -> return PersonId
             */

            // Get the globally unique identifier(GUID)
            try
            {
                CreatePersonResult person = await faceServiceClient.CreatePersonAsync(Constant.DefaultPersonGroupId, name);
                Guid id = person.PersonId;

                // Binding id and picture
                await faceServiceClient.AddPersonFaceAsync(Constant.DefaultPersonGroupId, id, picUrl);
                await faceServiceClient.TrainPersonGroupAsync(Constant.DefaultPersonGroupId);

                return id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Detected more than one person.", ex);
            }
        }

        public async Task<Person> GetUserDetailAsync(Stream imageStream)
        {
            var faceResult = await faceServiceClient.DetectAsync(imageStream);
            if (faceResult.Length > 1)
            {
                throw new Exception(faceResult.Length + " faces detected.");
            }

            Guid[] ids = new Guid[1];
            ids[0] = faceResult[0].FaceId;
            var identyResult = (await faceServiceClient.IdentifyAsync(Constant.DefaultPersonGroupId, ids))[0].Candidates;

            if (identyResult.Length != 1)
            {
                throw new Exception("Login Failed.");
            }

            var id = identyResult[0].PersonId;
            return await faceServiceClient.GetPersonAsync(Constant.DefaultPersonGroupId, id);
        }
    }
}
