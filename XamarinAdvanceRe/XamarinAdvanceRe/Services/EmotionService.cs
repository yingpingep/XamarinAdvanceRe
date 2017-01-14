using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.IO;

namespace XamarinAdvanceRe.Services
{
    class EmotionService
    {
        EmotionServiceClient emotionserviceclient;

        public EmotionService()
        {
            emotionserviceclient = new EmotionServiceClient(Constant.EmotionApiKey);
        }

        public async Task<List<KeyValuePair<string, float>>> GetEmotionRankAsync(Stream imageStream)
        {
            var emotionResult = await emotionserviceclient.RecognizeAsync(imageStream);
            return emotionResult[0].Scores.ToRankedList().ToList();
        }
    }
}
