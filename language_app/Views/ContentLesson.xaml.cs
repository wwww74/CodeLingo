using language_app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace language_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContentLesson : ContentPage
	{
		public int ID_lesson = 0;
		public int ID_part = 0;
        public int startItemPos = 0;
        public ObservableCollection<ContentCarousel> lessons { get; set; }
        public ContentLesson (int id_part, int id_lesson)
		{
			ID_part = id_part;
			ID_lesson = id_lesson;
			InitializeComponent();

            lessons = new ObservableCollection<ContentCarousel>
            {
                new ContentCarousel {H0 = "Добро пожаловать в С#!", H1="C# (произносится как See-Sharp) - один из самых популярных современных языков программирования.", H2="С# элегантен и мощен. Вы можете использовать его для создания видеоигр, веб-приложений, мобильных приложений, приложений баз данных и многого другого.", img="info.png", H3="Этот курс поможет вам быстро и самым простым способом написать свои собственные программы на C#, чтобывы могли решать реальные проблемы и задачи и создавать свои собственные приложения."},
                new ContentCarousel {H0 = "Программирование", H1="Суть большинства компьютерных программ - создание выводов. Приведём несколько примеров:", H2="- Уведомления 'Вы получили новое сообщение' ", H6="- Текст 'Game Over', который отображается на экране, когда вы играете в видеоигры", H7="- Остаток денег на вашем счёте, когда вы заходите в приложение для интернет-банкинга.", H8="Самый простой вывод - отображение сообщений на экране."},
                new ContentCarousel {H0_1="Уведомления и текст, отображаемые на экране, являются примерами выводом, создаваемых компьютерными программами.", False_btn=true, True_btn=true},
                new ContentCarousel {H0 = "Вывод", H1="С помощью выводов программисты проверяют, что компьютер следует заданным инструкциям, а также исправляют ошибки в коде.", H2="Следующая строка кода отображает сообщение на экране в качестве вывода.", frame_2=true},
                new ContentCarousel {H0_1 = "Выберите элемент, чтобы создать строку кода, которая выводит 'New message'.", H1="Console.WriteLine( ", frame_1=true, H1_1=");", Answer_btn=true},
                new ContentCarousel {H0 = "Резюме урока", H1="Отлично! Вы прошли ваш первый урок.", H2="Помните о следующих важных моментах:", H6="∘ Для создания выводов используется инструкция Console.WriteLine", H7="∘ За инструкцией Console.WriteLine должны следовать круглые скобки", H0_2="Идем дальше?", H9="На следующем уроке вы создадите код с несколькими строками и различными типами данных.", btn_stop = true}
            };

            MainCarousel.ItemsSource = lessons;

            startItemPos = MainCarousel.Position;
            MainCarousel.ScrollTo(lessons, position: ScrollToPosition.End);
		}

        private void Next_page_Clicked(object sender, EventArgs e) //click to the next item with carousel
        {
            progressBar.PercentageValue += 0.17f;
        }

        private void Button_Clicked_1(object sender, EventArgs e) //check answer
        {

        }

        private void False_btn_Clicked(object sender, EventArgs e) // answer false
        {

        }

        private void True_btn_Clicked(object sender, EventArgs e) // answer true
        {

        }

        private void Answer_btn_Clicked(object sender, EventArgs e) //page5 answer
        {

        }
        private async void Close_btn_Clicked(object sender, EventArgs e) //page5 answer
        {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private void MainCarousel_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            int previousItemPos = e.PreviousPosition;
            int currentItemPos = e.CurrentPosition;

            if (currentItemPos < previousItemPos)
            {
                Console.WriteLine("HUIHUIHUIHUIHUI POSITION:    " + currentItemPos + "   HUIHUIHUI START POS: " + startItemPos);
                progressBar.PercentageValue -= 0.2f;
            }
            else if (currentItemPos == startItemPos)
            {
                Console.WriteLine("HUIHUIHUIHUIHUI POSITION:    " + currentItemPos + "   HUIHUIHUI START POS: " + startItemPos);
                progressBar.PercentageValue = 0.0f;
            }
            else
            {
                Console.WriteLine("HUIHUIHUIHUIHUI POSITION:    " + currentItemPos + "   HUIHUIHUI START POS: " + startItemPos);
                progressBar.PercentageValue += 0.2f;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            DB db = new DB();

            if (db != null)
            {
                await db.UpdateStatusLesson(Preferences.Get("Username", string.Empty), ID_part, ID_lesson);
                await db.InsertNextLesson(Preferences.Get("Username", string.Empty), ID_part, ID_lesson + 1);
                await DisplayAlert("YAY", "Вы прошли урок, поздравляем!", "OK");
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
        }
    }
}