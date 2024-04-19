using language_app.Models;
using language_app.ViewModels;
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
    public partial class Achievement : ContentPage
    {
        public Achievement()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckConnectivity();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            main_grid.Children.Clear();
        }

        private async void CheckConnectivity()
        {
            var isConnected = CrossConnectivity.Current.IsConnected;

            try
            {
                if (isConnected)
                {
                    if (Preferences.Get("Log_in", false))
                    {
                        main_grid.Children.Clear();
                        Load_Connected_Form();
                    }
                    else
                    {
                        main_grid.Children.Clear();
                        Load_Logout_Form();
                    }
                }
                else
                {
                    main_grid.Children.Clear();
                    Load_Disconnected_Form();
                }
            }
            catch
            {
                await DisplayAlert("OOPSS..", "YOU DON'T CONNECTION ETHERNET", "OK");
            }
        }

        private void Load_Logout_Form()
        {
            Frame disconn_frame = new Frame
            {
                HasShadow = true,
                HeightRequest = 180,
                CornerRadius = 10,
                Margin = new Thickness(20, 20, 20, 0),
                Padding = 0,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex("#252525")
            };

            Grid dis_grid = new Grid();

            StackLayout dis_stack = new StackLayout();

            Label disconn_label_1 = new Label
            {
                Text = "Вы еще не вошли в профиль.",
                TextColor = Color.White,
                FontSize = 22,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 25, 0, 10)
            };

            Label disconn_label_2 = new Label
            {
                Text = "Войдите, чтобы увидеть содержимое",
                TextColor = Color.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Button btn_conn = new Button
            {
                Text = "Войти",
                BackgroundColor = Color.FromHex("#202020"),
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderColor = Color.FromHex("#33ff33"),
                BorderWidth = 0.5
            };

            btn_conn.Clicked += Btn_conn_Clicked1;

            dis_stack.Children.Add(disconn_label_1);
            dis_stack.Children.Add(disconn_label_2);
            dis_stack.Children.Add(btn_conn);

            dis_grid.Children.Add(dis_stack);

            disconn_frame.Content = dis_grid;

            main_grid.Children.Add(disconn_frame);

            Content = scrl;
        }

        private async void Btn_conn_Clicked1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private void Load_Disconnected_Form()
        {
            Frame disconn_frame = new Frame
            {
                HasShadow = true,
                HeightRequest = 180,
                CornerRadius = 10,
                Margin = new Thickness(20, 20, 20, 0),
                Padding = 0,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex("#252525")
            };

            Grid dis_grid = new Grid();

            StackLayout dis_stack = new StackLayout();

            Label disconn_label_1 = new Label
            {
                Text = "Вы не подключены к интернету.",
                TextColor = Color.White,
                FontSize = 22,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 25, 0, 10)
            };

            Label disconn_label_2 = new Label
            {
                Text = "Повторите попытку",
                TextColor = Color.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Button btn_conn = new Button
            {
                Text = "Повторить",
                BackgroundColor = Color.FromHex("#202020"),
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderColor = Color.FromHex("#33ff33"),
                BorderWidth = 0.5
            };

            btn_conn.Clicked += Btn_conn_Clicked;

            dis_stack.Children.Add(disconn_label_1);
            dis_stack.Children.Add(disconn_label_2);
            dis_stack.Children.Add(btn_conn);

            dis_grid.Children.Add(dis_stack);

            disconn_frame.Content = dis_grid;

            main_grid.Children.Add(disconn_frame);

            Content = scrl;
        }

        private void Btn_conn_Clicked(object sender, EventArgs e)
        {
            CheckConnectivity();
        }

        private async void Load_Connected_Form()
        {
            bool isLoad = false;

            while (!isLoad)
            {
                main_grid.IsVisible = false;
                indicator.IsVisible = true;

                int a_c = 0;
                int m_c = 0;

                DB database = new DB();

                if (database != null)
                {
                    long? active_course = await database.GetCountOpenAchiev(Preferences.Get("Username", string.Empty));
                    long? all_course = await database.GetCountAllAchiev();

                    Frame My_Course = new Frame
                    {
                        HasShadow = true,
                        CornerRadius = 10,
                        Margin = new Thickness(20, 20, 20, 0),
                        Padding = new Thickness(0, 0, 0, 20),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        BackgroundColor = Color.FromHex("#252525"),
                    };

                    Grid grid = new Grid();

                    StackLayout fr_stack = new StackLayout();

                    Label my_course_lbl = new Label
                    {
                        Text = "Мои достижения",
                        TextColor = Color.White,
                        FontSize = 30,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(0, 15, 0, 10)
                    };

                    Grid My_Grid_Course = new Grid
                    {
                        RowSpacing = 20
                    };

                    Grid.SetRow(My_Course, 0);

                    fr_stack.Children.Add(my_course_lbl);
                    fr_stack.Children.Add(My_Grid_Course);

                    grid.Children.Add(fr_stack);

                    My_Course.Content = grid;

                    main_grid.Children.Add(My_Course);

                    Frame All_Course = new Frame
                    {
                        HasShadow = true,
                        CornerRadius = 10,
                        Margin = new Thickness(20, 20, 20, 0),
                        Padding = new Thickness(0, 0, 0, 20),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        BackgroundColor = Color.FromHex("#252525")
                    };

                    Grid all_grid = new Grid();

                    StackLayout all_stack = new StackLayout();

                    Label all_course_lbl = new Label
                    {
                        Text = "Закрытые достижения",
                        TextColor = Color.White,
                        FontSize = 30,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(0, 15, 0, 10)
                    };

                    Grid All_Grid_Course = new Grid
                    {
                        RowSpacing = 20
                    };

                    Grid.SetRow(All_Course, 1);

                    all_stack.Children.Add(all_course_lbl);
                    all_stack.Children.Add(All_Grid_Course);

                    all_grid.Children.Add(all_stack);

                    All_Course.Content = all_grid;

                    main_grid.Children.Add(All_Course);

                    for (int i = 1; i <= all_course; i++)
                    {
                        DB db = new DB();
                        string username = Preferences.Get("Username", string.Empty);

                        var courses = await db.GetOpenAchievement(username, i);

                        if (courses != null)
                        {
                            Achiev new_course = new Achiev(courses.ID, courses.Name, courses.Image, courses.Description);
                            new_course.frame.Opacity = 0;

                            Grid.SetRow(new_course.frame, m_c);
                            My_Grid_Course.Children.Add(new_course.frame);
                            My_Grid_Course.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                            m_c++;

                            var animation = new Animation(v => new_course.frame.Opacity = v, 0, 1);
                            animation.Commit(this, "VisibleCourse", 16, 1000, Easing.SinInOut, (v, c) => new_course.frame.Opacity = 1, () => false);
                        }
                    }

                    if (active_course == 0)
                    {
                        Label label1 = new Label
                        {
                            Text = "Извините!",
                            TextColor = Color.White,
                            FontSize = 23,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        Label label2 = new Label
                        {
                            Text = "Но у вас нет еще ни одного достижения :(",
                            TextColor = Color.White,
                            FontSize = 17,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Margin = new Thickness(0, 50, 0, 0)
                        };
                        My_Grid_Course.Children.Add(label1);
                        My_Grid_Course.Children.Add(label2);
                    }

                    for (int i = 1; i <= all_course; i++)
                    {
                        DB db = new DB();
                        string username = Preferences.Get("Username", string.Empty);

                        var course = await db.GetCloseAchievement(username, i);

                        if (course != null)
                        {
                            Achiev new_course = new Achiev(course.ID, course.Name, course.Image, course.Description);

                            new_course.frame.Opacity = 0;

                            Grid.SetRow(new_course.frame, a_c);
                            All_Grid_Course.Children.Add(new_course.frame);
                            All_Grid_Course.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            a_c++;

                            var animation = new Animation(v => new_course.frame.Opacity = v, 0, 1);
                            animation.Commit(this, "VisibleCourse", 16, 1000, Easing.SinInOut, (v, c) => new_course.frame.Opacity = 1, () => false);
                        }
                    }

                    if (All_Grid_Course.RowDefinitions.Count == 0)
                    {
                        Label label3 = new Label
                        {
                            Text = "К сожалению!",
                            TextColor = Color.White,
                            FontSize = 23,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        Label label4 = new Label
                        {
                            Text = "Все доступные курсы закончились :(",
                            TextColor = Color.White,
                            FontSize = 17,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Margin = new Thickness(0, 50, 0, 0)
                        };

                        All_Grid_Course.Children.Add(label3);
                        All_Grid_Course.Children.Add(label4);
                    }
                }
                else
                    await DisplayAlert("Упс...", "Произошла ошибка, попробуйте позже!", "ОК");

                isLoad = true;
            }
            main_grid.IsVisible = true;
            indicator.IsVisible = false;

            Content = scrl;
        }
    }
}