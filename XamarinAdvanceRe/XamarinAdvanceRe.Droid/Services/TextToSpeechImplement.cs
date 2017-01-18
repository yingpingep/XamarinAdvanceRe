using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using XamarinAdvanceRe.Services;
using Android.Speech.Tts;
using XamarinAdvanceRe.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplement))]
namespace XamarinAdvanceRe.Droid.Services
{
    public class TextToSpeechImplement : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;
                
        public void Speak(string text)
        {
            var context = Xamarin.Forms.Forms.Context;
            toSpeak = text;

            if (speaker == null)
            {
                speaker = new TextToSpeech(context, this);
            }
            else
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null);
            }
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null);
            }
        }
    }
}