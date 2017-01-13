﻿using System;
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
        FaceServiceClient fsc;
        public FaceService()
        {
            fsc = new FaceServiceClient(Constant.FaceApiKey);

            // make sure the people group exist (200 if created successfully, 409 if existed)
            fsc.CreatePersonGroupAsync(Constant.DefaultPersonGroupId, Constant.DefaultPersonGroupId);
        }

        // TODO : 新增的部分
        public async Task<MyDataType> DetectFace(string imageUrl)
        {
            FaceAttributeType[] fats = new FaceAttributeType[]
            {
                FaceAttributeType.Age
            };

            Face[] faceResult;
            try
            {
                // TODO: 使用 FaceServiceClient 提供的 DetectAsync 方法取得已辨識的臉部資訊
                // Step 1: FaceServiceClient.DetectAsync(string, ...)                
                faceResult = await fsc.DetectAsync(imageUrl, true, false, fats);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // TODO: 建立 MyDataType 型別的物件，整理 FaceServiceClient 傳回的資訊，方便後續使用
            // Step 1: new MyDataType();
            // Step 2: foreach (faces)
            // Step 3: rects.Add(new Rect(int, int, int))
            // Step 4: ages.Add(new Age(FaceAttributes.Age))            
            MyDataType mydata = new MyDataType();
            foreach (var item in faceResult)
            {
                mydata.rects.Add(new Rect(item.FaceRectangle.Left, item.FaceRectangle.Top, item.FaceRectangle.Width));
                mydata.ages.Add(new Age(item.FaceAttributes.Age));
            }

            return mydata;
        }

        public async Task<string> GetPersonId(string name, string picUrl)
        {
            /*
             * Create person and get PersonId
             * -> add image
             * -> train
             * -> return PersonId
             */

            // Get the globally unique identifier(GUID)
            CreatePersonResult person = await fsc.CreatePersonAsync(Constant.DefaultPersonGroupId, name);
            Guid id = person.PersonId;

            // Binding id and picture
            await fsc.AddPersonFaceAsync(Constant.DefaultPersonGroupId, id, picUrl);
            await fsc.TrainPersonGroupAsync(Constant.DefaultPersonGroupId);

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
            var identyResult = (await fsc.IdentifyAsync(Constant.DefaultPersonGroupId, ids))[0].Candidates;

            if (identyResult.Length != 1)
            {
                throw new Exception("Login Failed.");
            }

            var id = identyResult[0].PersonId;
            return await fsc.GetPersonAsync(Constant.DefaultPersonGroupId, id);
        }
        // TODO: Add EasyProject here.
    }
}
