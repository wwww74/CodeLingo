using language_app.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace language_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartCourse : ContentPage
    {
        public int id_course = 0;
        public string name_course = "";
        public string username = "";
        public StartCourse(int id_crs, string name_crs)
        {
            InitializeComponent();

            username = Preferences.Get("Username", string.Empty);
            id_course = id_crs;
            name_course = name_crs;

            CheckConnectivity();
        }

        private void CheckConnectivity()
        {
            var isConnected = CrossConnectivity.Current.IsConnected;

            stack.Children.Clear();

            if (isConnected)
                LoadForm_Conn();
            else
                LoadForm_Disconn();
        }

        private void LoadForm_Disconn()
        {
            Label lbl = new Label
            {
                Text = "Отсутствует подключение к интернету!",
                TextColor = Color.White,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(10, 10, 10, 0)
            };

            Frame frame = new Frame
            {
                HasShadow = true,
                CornerRadius = 10,
                BackgroundColor = Color.FromHex("#373737"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped; ;
            frame.GestureRecognizers.Add(tap);

            Label lbl2 = new Label
            {
                Text = "Повторить",
                TextColor = Color.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            frame.Content = lbl2;

            stack.Children.Add(lbl);
            stack.Children.Add(frame);

            Content = fr;
        }

        private void Tap_Tapped(object sender, EventArgs e)
        {
            CheckConnectivity();
        }

        private void LoadForm_Conn()
        {
            Label lbl = new Label
            {
                Text = "Вы уверены,",
                TextColor = Color.White,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(10, 10, 10, 0)
            };

            Label lbl1 = new Label
            {
                Text = "Что хотите начать этот курс?",
                TextColor = Color.White,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(10, 0, 10, 10)
            };

            Frame frame = new Frame
            {
                HasShadow = true,
                CornerRadius = 10,
                BackgroundColor = Color.FromHex("#373737"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += TapGestureRecognizer_Tapped;
            frame.GestureRecognizers.Add(tap);

            Label lbl2 = new Label
            {
                Text = "НАЧАТЬ",
                TextColor = Color.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            frame.Content = lbl2;

            stack.Children.Add(lbl);
            stack.Children.Add(lbl1);
            stack.Children.Add(frame);

            Content = fr;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DB db = new DB();
            int? min_part = await db.CheckStartPart(id_course);
            int? min_lesson = await db.CheckStartLesson(min_part);

            if (db != null )
            {
                await db.InsertCourse(username, id_course);
                await db.AddFirstPart(username, id_course, min_part);
                await db.AddFirstLesson(username, min_part, min_lesson);
                await Navigation.PushAsync(new Lessons(id_course, name_course));
            }
            else
            {
                await DisplayAlert("Ooppss..", "Произошла ошибка подключения :(", "OK");
            }

        }
    }
}