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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            CheckConnectivity();
        }

        private async void CheckConnectivity()
        {
            var isConnected = CrossConnectivity.Current.IsConnected;

            try
            {
                if (isConnected)
                {
                    if (Username_Entry.Text != null && Password_Entry.Text != null)
                    {
                        DB db = new DB();
                        if (db != null)
                        {
                            var user = await db.GetUser(Username_Entry.Text);
                            if (user == null)
                            {
                                if (Username_Entry.Text.Length < 3 || Password_Entry.Text.Length < 3 || Password_Entry.Text.Length > 10)
                                {
                                    await DisplayAlert("Упс...", "Поле 'Логин' должно содержать минимум 3 символа, а поле 'Пароль' минимум 3 и максимум 10 символов!", "ОК");
                                }
                                else
                                {
                                    try
                                    {
                                        await db.Add_User(new User { Username = Username_Entry.Text, Password = Password_Entry.Text });
                                        await DisplayAlert("Умничка!", "Регистрация прошла успешно! Вы можете авторизироваться!", "ОК");
                                        await db.AddAchiev(Username_Entry.Text, 1);
                                        await DisplayAlert("Умничка!", "У вас новое достижение!!!", "ОК");
                                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                                    }
                                    catch
                                    {
                                        await DisplayAlert("Упс...", "Произошла ошибка при регистрации, повторите попытку позже :(", "ОК");
                                    }
                                }
                            }
                            else
                                await DisplayAlert("Упс...", "К сожалению, введенное вами имя уже занято!", "ОК");
                        }
                        else
                            await DisplayAlert("Упс...", "Произошла ошибка, попробуйте позже!", "ОК");
                    }
                    else
                        await DisplayAlert("Упс...", "Пожалуйста, заполните все поля!", "ОК");
                }
                else
                    await DisplayAlert("Упс...", "Отсутствует подключение к интернету", "ОК");
            }
            catch
            {
                await DisplayAlert("Упс...", "Произошла ошибка на сервере, попробуйте позже", "ОК");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}