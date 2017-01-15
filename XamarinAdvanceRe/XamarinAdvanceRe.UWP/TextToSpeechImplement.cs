using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XamarinAdvanceRe.Services;
using XamarinAdvanceRe.UWP;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplement))]
namespace XamarinAdvanceRe.UWP
{
    public class TextToSpeechImplement : ITextToSpeech
    {
        public async void Speak(string text)
        {
            var mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();
            var stream = await synth.SynthesizeTextToStreamAsync(text);

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}
