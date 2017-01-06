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
        EmotionServiceClient esc;

        public EmotionService()
        {
            esc = new EmotionServiceClient(Constant.EmotionApiKey);
        }

        public async Task<List<KeyValuePair<string, float>>> GetEmotionRank(Stream imageStream)
        {
            var emotionResult = await esc.RecognizeAsync(imageStream);
            return emotionResult[0].Scores.ToRankedList().ToList();
        }
    }
}
