using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.IO;
using DrawFunction;

namespace XamarinAdvanceRe.Services
{
    class EmotionService
    {
        EmotionServiceClient emotionServiceClient;

        public EmotionService()
        {
            emotionServiceClient = new EmotionServiceClient(Constant.EmotionApiKey);
        }

        public async Task<List<KeyValuePair<string, float>>> GetEmotionRankAsync(Stream imageStream)
        {
            Emotion[] emotionResult = await emotionServiceClient.RecognizeAsync(imageStream);
            return emotionResult[0].Scores.ToRankedList().ToList();
        }
    }
}