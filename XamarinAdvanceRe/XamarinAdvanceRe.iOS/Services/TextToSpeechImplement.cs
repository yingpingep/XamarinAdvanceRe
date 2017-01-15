using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using XamarinAdvanceRe.Services;
using AVFoundation;
using XamarinAdvanceRe.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplement))]
namespace XamarinAdvanceRe.iOS.Services
{
    public class TextToSpeechImplement : ITextToSpeech
    {
        public void Speak(string text)
        {
            var speechSynthesizer = new AVSpeechSynthesizer();
            var speechUtterance = new AVSpeechUtterance(text)
            {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
                Voice = AVSpeechSynthesisVoice.FromLanguage(null),
                Volume = 0.5f,
                PitchMultiplier = 1.0f
            };

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }
    }
}