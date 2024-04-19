using language_app.Models;
using language_app.ViewModels;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace language_app.Views
{
    public partial class Profile : ContentPage
    {
        bool isState = false;
        bool isEdit = false;

        public string name = "";
        public string mail = "";
        public string number = "";

        public Label name_label = new Label();
        public Label mail_label = new Label();
        public Label number_label = new Label();
        public Label personal_name = new Label();
        public Label personal_mail = new Label();
        public Label personal_number = new Label();
        public Grid Personal_Info_Grid = new Grid();
        public Profile()
        {
            InitializeComponent();
            CheckConnectivity();
        }

        private async void CheckConnectivity()
        {
            var isConnected = CrossConnectivity.Current.IsConnected;

            try
            {
                if (isConnected)
                {
                    Personal_Body_Grid.Children.Clear();
                    LoadForm_Conn();
                }
                else
                {
                    Personal_Body_Grid.Children.Clear();
                    LoadForm_Disconn();
                }
            }

            catch
            {
                await DisplayAlert("Oooppsss..", "YOU DON'T CONNECTION ETHERNET", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Personal_Body_Grid.Children.Clear();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            CheckConnectivity();
        }

        private async void LoadForm_Conn()
        {
            Ellipse ell = new Ellipse
            {
                Fill = Color.FromHex("#151515"),
                WidthRequest = 120,
                HeightRequest = 120,
                Margin = new Thickness(0, -80, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            Ellipse ellipse = new Ellipse
            {
                Fill = Color.FromHex("#252525"),
                WidthRequest = 110,
                HeightRequest = 110,
                Margin = new Thickness(0, -75, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            Grid.SetRow(ell, 0);
            Grid.SetRow(ellipse, 0);

            Personal_Body_Grid.Children.Add(ell);
            Personal_Body_Grid.Children.Add(ellipse);

            if (Preferences.Get("Log_in", false))
            {
                close_acc.IsVisible = true;

                DB db = new DB();
                if (db != null)
                {
                    var user = await db.GetUser(Preferences.Get("Username", string.Empty));
                    name = user.First_Name;
                    mail = user.Email;
                    number = user.Phone;

                    if (user != null)
                    {
                        Frame Personal_Info = new Frame
                        {
                            HasShadow = true,
                            CornerRadius = 10,
                            Margin = new Thickness(0, 20, 0, 0),
                            BackgroundColor = Color.FromRgb(32, 32, 32)
                        };

                        Label personal_info_label = new Label
                        {
                            Text = "Личная информация",
                            TextColor = Color.White,
                            FontSize = 20,
                        };

                        ImageButton img_btn = new ImageButton
                        {
                            Source = "edit.png",
                            Scale = 0.8,
                            BackgroundColor = Color.FromRgb(32, 32, 32),
                            Margin = new Thickness(0, -5, 0, 0),
                            HorizontalOptions = LayoutOptions.EndAndExpand
                        };

                        img_btn.Clicked += Img_btn_Clicked;

                        Xamarin.Forms.Shapes.Line line = new Xamarin.Forms.Shapes.Line
                        {
                            X1 = 0,
                            X2 = 400,
                            Stroke = Color.White,
                            Margin = new Thickness(0, 30, 0, 0)
                        };

                        name_label.Text = "Имя: ";
                        name_label.TextColor = Color.White;
                        name_label.FontSize = 17;
                        name_label.Margin = new Thickness(0, 10, 0, 0);

                        mail_label.Text = "Почта: ";
                        mail_label.TextColor = Color.White;
                        mail_label.FontSize = 17;
                        mail_label.Margin = new Thickness(0, 10, 0, 0);

                        number_label.Text = "Телефон: ";
                        number_label.TextColor = Color.White;
                        number_label.FontSize = 17;
                        number_label.Margin = new Thickness(0, 10, 0, 0);

                        Image user_img = new Image
                        {
                            Source = "user_profile.png",
                            WidthRequest = 80,
                            HeightRequest = 80,
                            Margin = new Thickness(0, -65, 0, 0),
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.StartAndExpand
                        };

                        Label main_name = new Label
                        {
                            TextColor = Color.White,
                            FontSize = 25,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Margin = new Thickness(0, 50, 0, 0)
                        };

                        if (user.First_Name == null)
                            main_name.Text = "Username";
                        else
                            main_name.Text = user.First_Name;

                        personal_name.Text = user.First_Name;
                        personal_name.TextColor = Color.White;
                        personal_name.FontSize = 17;
                        personal_name.Margin = new Thickness(45, 10, 0, 0);

                        personal_mail.Text = user.Email;
                        personal_mail.TextColor = Color.White;
                        personal_mail.FontSize = 17;
                        personal_mail.Margin = new Thickness(60, 10, 0, 0);

                        personal_number.Text = user.Phone;
                        personal_number.TextColor = Color.White;
                        personal_number.FontSize = 17;
                        personal_number.Margin = new Thickness(75, 10, 0, 0);

                        Frame Personal_Achievements = new Frame
                        {
                            HasShadow = true,
                            HeightRequest = 150,
                            CornerRadius = 10,
                            Margin = new Thickness(0, 20, 0, 0),
                            BackgroundColor = Color.FromRgb(32, 32, 32)
                        };

                        Grid Personal_Achiv_Grid = new Grid();

                        Label achiv_label = new Label
                        {
                            Text = "Достижения",
                            TextColor = Color.White,
                            FontSize = 20
                        };

                        Xamarin.Forms.Shapes.Line line_achiv = new Xamarin.Forms.Shapes.Line
                        {
                            X1 = 0,
                            X2 = 400,
                            Stroke = Color.White,
                            Margin = new Thickness(0, 30, 0, 0)
                        };

                        Grid.SetRow(personal_info_label, 0);
                        Grid.SetRow(img_btn, 0);
                        Grid.SetRow(line, 0);
                        Grid.SetRow(name_label, 1);
                        Grid.SetRow(mail_label, 2);
                        Grid.SetRow(number_label, 3);
                        Grid.SetRow(Personal_Info, 1);
                        Grid.SetRow(user_img, 0);
                        Grid.SetRow(main_name, 0);
                        Grid.SetRow(personal_name, 1);
                        Grid.SetRow(personal_mail, 2);
                        Grid.SetRow(personal_number, 3);
                        Grid.SetRow(achiv_label, 0);
                        Grid.SetRow(line_achiv, 0);
                        Grid.SetRow(Personal_Achievements, 2);
                        Grid.SetRow(Frame_name, 1);
                        Grid.SetRow(Frame_mail, 2);
                        Grid.SetRow(Frame_number, 3);
                        Grid.SetRow(exit_btn, 4);
                        Grid.SetRow(save_btn, 4);

                        Personal_Info_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        Personal_Info_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        Personal_Info_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        Personal_Achiv_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        Personal_Achiv_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                        Personal_Info_Grid.Children.Add(Frame_name);
                        Personal_Info_Grid.Children.Add(Frame_mail);
                        Personal_Info_Grid.Children.Add(Frame_number);
                        Personal_Info_Grid.Children.Add(exit_btn);
                        Personal_Info_Grid.Children.Add(save_btn);
                        Personal_Info_Grid.Children.Add(personal_info_label);
                        Personal_Info_Grid.Children.Add(img_btn);
                        Personal_Info_Grid.Children.Add(line);
                        Personal_Info_Grid.Children.Add(name_label);
                        Personal_Info_Grid.Children.Add(mail_label);
                        Personal_Info_Grid.Children.Add(number_label);
                        Personal_Info_Grid.Children.Add(personal_name);
                        Personal_Info_Grid.Children.Add(personal_mail);
                        Personal_Info_Grid.Children.Add(personal_number);
                        Personal_Achiv_Grid.Children.Add(achiv_label);
                        Personal_Achiv_Grid.Children.Add(line_achiv);

                        Personal_Info.Content = Personal_Info_Grid;
                        Personal_Achievements.Content = Personal_Achiv_Grid;

                        Personal_Body_Grid.Children.Add(user_img);
                        Personal_Body_Grid.Children.Add(main_name);
                        Personal_Body_Grid.Children.Add(Personal_Info);
                        //Personal_Body_Grid.Children.Add(Personal_Achievements);

                        Content = ScrollV;
                    }
                    else
                        await DisplayAlert("Упс...", "Произошла ошибка, попробуйте позже!", "ОК");
                }
                else
                    await DisplayAlert("Упс...", "Произошла ошибка, попробуйте позже!", "ОК");
            }
            else
            {
                close_acc.IsVisible = false;

                Image user_img = new Image
                {
                    Source = "user_profile.png",
                    WidthRequest = 80,
                    HeightRequest = 80,
                    Margin = new Thickness(0, -65, 0, 0),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand
                };

                Frame Log_in = new Frame
                {
                    HasShadow = true,
                    CornerRadius = 10,
                    Margin = new Thickness(0, 20, 0, 0),
                    BackgroundColor = Color.FromRgb(32, 32, 32)
                };

                Grid Login_Grid = new Grid();

                Label error_login = new Label
                {
                    Text = "Вы еще не вошли в аккаунт!",
                    TextColor = Color.White,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };

                Button btn_login = new Button
                {
                    Text = "Войти",
                    BackgroundColor = Color.FromHex("#202020"),
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    BorderColor = Color.FromHex("#33ff33"),
                    BorderWidth = 0.3
                };

                btn_login.Clicked += Btn_login_Clicked;

                Grid.SetRow(error_login, 0);
                Grid.SetRow(btn_login, 1);
                Grid.SetRow(Log_in, 1);

                Login_Grid.Children.Add(error_login);
                Login_Grid.Children.Add(btn_login);

                Login_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Login_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Log_in.Content = Login_Grid;

                Personal_Body_Grid.Children.Add(user_img);
                Personal_Body_Grid.Children.Add(Log_in);

                Content = ScrollV;
            }
        }

        private async void LoadForm_Disconn()
        {
            close_acc.IsVisible = false;

            Ellipse ell = new Ellipse
            {
                Fill = Color.FromHex("#151515"),
                WidthRequest = 120,
                HeightRequest = 120,
                Margin = new Thickness(0, -80, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            Ellipse ellipse = new Ellipse
            {
                Fill = Color.FromHex("#252525"),
                WidthRequest = 110,
                HeightRequest = 110,
                Margin = new Thickness(0, -75, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            Grid.SetRow(ell, 0);
            Grid.SetRow(ellipse, 0);

            Personal_Body_Grid.Children.Add(ell);
            Personal_Body_Grid.Children.Add(ellipse);

            Image user_img = new Image
            {
                Source = "user_profile.png",
                WidthRequest = 80,
                HeightRequest = 80,
                Margin = new Thickness(0, -65, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            Frame Log_in = new Frame
            {
                HasShadow = true,
                CornerRadius = 10,
                Margin = new Thickness(0, 20, 0, 0),
                BackgroundColor = Color.FromRgb(32, 32, 32)
            };

            Grid Login_Grid = new Grid();

            Label error_login = new Label
            {
                Text = "Отсутствует подключение к интернету!",
                TextColor = Color.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            Button btn_login = new Button
            {
                Text = "Повторить",
                BackgroundColor = Color.FromHex("#202020"),
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderColor = Color.FromHex("#33ff33"),
                BorderWidth = 0.3
            };

            btn_login.Clicked += Btn_login_Clicked1;

            Grid.SetRow(error_login, 0);
            Grid.SetRow(btn_login, 1);
            Grid.SetRow(Log_in, 1);

            Login_Grid.Children.Add(error_login);
            Login_Grid.Children.Add(btn_login);

            Login_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Login_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Log_in.Content = Login_Grid;

            Personal_Body_Grid.Children.Add(user_img);
            Personal_Body_Grid.Children.Add(Log_in);

            Content = ScrollV;
        }

        private void Btn_login_Clicked1(object sender, EventArgs e)
        {
            CheckConnectivity();
        }

        private async void Btn_login_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void Img_btn_Clicked(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                name_label.IsVisible = false;
                mail_label.IsVisible = false;
                number_label.IsVisible = false;
                personal_name.IsVisible = false;
                personal_mail.IsVisible = false;
                personal_number.IsVisible = false;

                Frame_name.IsVisible = true;
                Frame_mail.IsVisible = true;
                Frame_number.IsVisible = true;
                exit_btn.IsVisible = true;
                save_btn.IsVisible = true;

                Entry_name.Text = personal_name.Text;
                Entry_mail.Text = personal_mail.Text;
                Entry_number.Text = personal_number.Text;
                Personal_Info_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Preferences.Get("Log_in", true))
            {
                Preferences.Clear();
                Preferences.Set("Log_in", false);
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await DisplayAlert("Упс..", "Кажется, вы уже вышли из профиля!", "ОК");
            }
        }

        private void exit_btn_Clicked(object sender, EventArgs e)
        {
            isEdit = false;

            name_label.IsVisible = true;
            mail_label.IsVisible = true;
            number_label.IsVisible = true;
            personal_name.IsVisible = true;
            personal_mail.IsVisible = true;
            personal_number.IsVisible = true;

            Frame_name.IsVisible = false;
            Frame_mail.IsVisible = false;
            Frame_number.IsVisible = false;
            exit_btn.IsVisible = false;
            save_btn.IsVisible = false;

            personal_name.Text = name;
            personal_mail.Text = mail;
            personal_number.Text = number;
        }

        private async void save_btn_Clicked(object sender, EventArgs e)
        {
            DB db = new DB();
            if (db != null)
            {
                await db.UpdateUserInfo(Entry_name.Text, Entry_mail.Text, Entry_number.Text, Preferences.Get("Username", string.Empty));
                await DisplayAlert("Умничка", "Данные успешно сохранены", "OK");

                isEdit = false;

                name_label.IsVisible = true;
                mail_label.IsVisible = true;
                number_label.IsVisible = true;
                personal_name.IsVisible = true;
                personal_mail.IsVisible = true;
                personal_number.IsVisible = true;

                Frame_name.IsVisible = false;
                Frame_mail.IsVisible = false;
                Frame_number.IsVisible = false;
                exit_btn.IsVisible = false;
                save_btn.IsVisible = false;
                Personal_Info_Grid.RowDefinitions.RemoveAt(4);

                CheckConnectivity();
            }
            else
                await DisplayAlert("Ошибка", "Ошибка подключения к серверу, попробуйте позже", "OK");
        }
    }
}