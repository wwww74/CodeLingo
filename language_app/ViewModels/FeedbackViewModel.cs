using language_app.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace language_app.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        public ICommand OpenAcc_Max { get; }
        public ICommand OpenAcc_Andrew { get; }
        public FeedbackViewModel()
        {
            Title = "Обратная связь";
            OpenAcc_Max = new Command(async () => await Browser.OpenAsync("https://t.me/wwww74"));
            OpenAcc_Andrew = new Command(async () => await Browser.OpenAsync("https://t.me/SqrtWave"));
        }
    }
}
