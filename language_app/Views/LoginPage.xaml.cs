using language_app.Models;
using language_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace language_app.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e) // Tap login
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
                    if (Username_Entry.Text == null || Password_Entry.Text == null)
                        await DisplayAlert("Упс...", "Одно из полей пустое, заполните поля и повторите попытку", "ОК");
                    else
                    {
                        DB db = new DB();
                        if (db != null)
                        {
                            var user = await db.GetUser(Username_Entry.Text);
                            if (user != null)
                            {
                                if (user.Username == Username_Entry.Text && user.Password == Password_Entry.Text)
                                {
                                    await DisplayAlert("Умничка!", "Авторизация прошла успешно!", "ОК");
                                    Preferences.Set("Log_in", true);
                                    Preferences.Set("Username", Username_Entry.Text);
                                    Preferences.Set("UserPass", Password_Entry.Text);

                                    var stack = Shell.Current.Navigation.NavigationStack.ToArray();
                                    for (int i = stack.Length - 1; i > 0; i--)
                                    {
                                        Shell.Current.Navigation.RemovePage(stack[i]);
                                    }
                                    await Shell.Current.GoToAsync($"//{nameof(Profile)}");
                                }
                                else
                                {
                                    await DisplayAlert("Упс...", "Неверный логин или пароль :(", "ОК");
                                }
                            }
                            else
                                await DisplayAlert("Упс...", "Неверный логин или пароль :(", "ОК");
                        }
                        else
                            await DisplayAlert("Упс...", "Произошла ошибка, попробуйте позже!", "ОК");
                    }   
                }
                else
                    await DisplayAlert("Упс...", "Отсутствует подключение к интернету", "ОК");
            }

            catch
            {
                await DisplayAlert("Упс...", "Произошла ошибка, попробуйте позже", "ОК");
                await Shell.Current.GoToAsync($"//{nameof(Profile)}");
            }
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e) // Tap Register
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}