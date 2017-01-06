using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using System.IO;
using Microsoft.ProjectOxford.Face.Contract;

namespace XamarinAdvanceRe.Services
{
    class FaceService
    {
        FaceServiceClient fsc;
        public FaceService()
        {
            fsc = new FaceServiceClient(Constant.FaceApiKey);
        }        

        public async Task<string> GetPersonId(string name, string picUrl)
        {
            /*
             * Create person and get PersonId
             * -> add image
             * -> train
             * -> return PersonId
             */

            var id = (await fsc.CreatePersonAsync(Constant.DefaultGroupName, name)).PersonId;

            // Binding id and picture
            await fsc.AddPersonFaceAsync(Constant.DefaultGroupName, id, picUrl);
            await fsc.TrainPersonGroupAsync(Constant.DefaultGroupName);

            return id.ToString();
        }
        
        public async Task<Person> GetUserDetail(Stream imageStream)
        {
            var faceResult = await fsc.DetectAsync(imageStream);
            if (faceResult.Length > 1)
            {
                throw new Exception(faceResult.Length + " faces detected.");
            }

            Guid[] ids = new Guid[1];
            ids[0] = faceResult[0].FaceId;
            var identyResult = (await fsc.IdentifyAsync(Constant.DefaultGroupName, ids))[0].Candidates;

            if (identyResult.Length != 1)
            {
                throw new Exception("Login Failed.");
            }

            var id = identyResult[0].PersonId;
            return await fsc.GetPersonAsync(Constant.DefaultGroupName, id);
        }
        // TODO: Add EasyProject here.
    }
}
