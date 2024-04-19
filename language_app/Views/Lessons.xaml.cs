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
    public partial class Lessons : ContentPage
    {
        public int id = 0;
        public string name = "";
        public string username = "";
        public bool? check = false;

        public ImageButton fav_btn = new ImageButton();
        public Lessons(int id_lesson, string name_course)
        {
            InitializeComponent();

            id = id_lesson;
            name = name_course;
            username = Preferences.Get("Username", string.Empty);

            CheckConnectivity();
        }

        private async void CheckConnectivity()
        {
            Label title = new Label
            {
                Text = name,
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(0, 15, 0, 0)
            };

            DB dB = new DB();

            if (dB != null)
            {
                check = await dB.CheckFavCourse(Preferences.Get("Username", string.Empty), id);
            }

            if ((bool)check)
                fav_btn.Source = "add_fav.png";
            else
                fav_btn.Source = "non_fav.png";

            fav_btn.BackgroundColor = Color.FromHex("#151515");
            fav_btn.Scale = 0.8;
            fav_btn.Margin = new Thickness(10, 12, 0, 0);

            fav_btn.Clicked += add_favourites_Clicked;

            Grid.SetRow(title, 0);
            Grid.SetRow(fav_btn, 0);
            Grid.SetColumn(title, 0);
            Grid.SetColumn(fav_btn, 4);

            lTitleView.Children.Add(title);
            lTitleView.Children.Add(fav_btn);

            var isConnected = CrossConnectivity.Current.IsConnected;

            if (isConnected)
                LoadForm_Connected();
            else
                LoadForm_Disconnected();
        }

        private void LoadForm_Disconnected()
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

            dis_stack.Children.Add(disconn_label_1);
            dis_stack.Children.Add(disconn_label_2);
            dis_stack.Children.Add(btn_conn);

            dis_grid.Children.Add(dis_stack);

            disconn_frame.Content = dis_grid;

            main_grid.Children.Add(disconn_frame);

            Content = scrl;
        }

        private async void LoadForm_Connected()
        {
            bool isLoad = false;
            
            while (!isLoad)
            {
                Part_Content.IsVisible = false;
                indicator.IsVisible = true;

                int part_count = 0;
                int less_count = 0;

                DB db = new DB();

                if (db != null)
                {
                    int? count = await db.GetCountAllParts(id);
                    int min_part = await db.GetCountPartWithCourseID(id);

                    for (int i = min_part; i <= count; i++)
                    {
                        var parts = await db.GetActivePart(username, id, i);
                        int? min_lesson = await db.GetCountLessonWithPartID(i);
                        long? all_lesson = await db.GetCountAllLesson(i);
                        if (parts != null)
                        {
                            if (parts.ID == i)
                            {
                                Part new_part = new Part(parts.ID, parts.ID_course, parts.Name, true);

                                for (int? j = min_lesson; j <= all_lesson; j++)
                                {
                                    var lesson = await db.GetActiveLesson(username, i, j);
                                    if (lesson != null)
                                    {
                                        if (lesson.ID == j)
                                        {
                                            Lesson new_less = new Lesson(lesson.ID, lesson.ID_part, lesson.Name, lesson.Type, true);

                                            Grid.SetRow(new_less.fr, less_count);

                                            TapGestureRecognizer tap = new TapGestureRecognizer();
                                            tap.Command = new Command(async () => await Navigation.PushAsync(new ContentLesson(i, lesson.ID)));
                                            new_less.fr.Content.GestureRecognizers.Add(tap);

                                            new_part.grid_fr.Children.Add(new_less.fr);
                                            new_part.grid_fr.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                                            less_count++;
                                        }
                                    }
                                }
                                for (int? j = min_lesson; j <= all_lesson; j++)
                                {
                                    var lesson = await db.GetInactiveLesson(username, i, j);
                                    if (lesson != null)
                                    {
                                        if (lesson.ID == j)
                                        {
                                            Lesson new_less = new Lesson(lesson.ID, lesson.ID_part, lesson.Name, lesson.Type, false);

                                            Grid.SetRow(new_less.fr, less_count);

                                            TapGestureRecognizer tap = new TapGestureRecognizer();
                                            tap.Command = new Command(async () => await DisplayAlert("Заблокировано", "Завершите предыдущие уроки, чтобы разблокировать это.", "OK"));
                                            new_less.fr.Content.GestureRecognizers.Add(tap);

                                            new_part.grid_fr.Children.Add(new_less.fr);
                                            new_part.grid_fr.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                                            less_count++;
                                        }
                                    }
                                }
                                less_count = 0;
                                new_part.frame.IsVisible = false;

                                new_part.arr_btn.Command = new Command(async () => { if (!new_part.isOpen) { new_part.isOpen = true; new_part.frame.IsVisible = true; await new_part.arr_btn.RotateTo(180, 500); new_part.arr_btn.Rotation = 180; } else { new_part.isOpen = false; new_part.frame.IsVisible = false; await new_part.arr_btn.RotateTo(360, 500); new_part.arr_btn.Rotation = 0; } });

                                Grid.SetRow(new_part.stack, part_count);

                                Part_Grid.Children.Add(new_part.stack);
                                Part_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                                part_count++;

                                //var animation = new Animation(v => new_part.stack.Opacity = v, 0, 1);
                                //animation.Commit(this, "VisibleCourse", 16, 1000, Easing.SinInOut, (v, c) => new_part.stack.Opacity = 1, () => false);
                            }
                        }
                    }

                    for (int i = min_part; i <= count; i++)
                    {
                        int? min_lesson = await db.GetCountLessonWithPartID(i);
                        long? all_lesson = await db.GetCountAllLesson(i);
                        var parts = await db.GetInactivePart(username, id, i);
                        if (parts != null)
                        {
                            if (parts.ID == i)
                            {
                                Part new_part = new Part(parts.ID, parts.ID_course, parts.Name, false);

                                for (int? j = min_lesson; j <= all_lesson; j++)
                                {
                                    var lesson = await db.GetActiveLesson(username, i, j);
                                    if (lesson != null)
                                    {
                                        if (lesson.ID == j)
                                        {
                                            Lesson new_less = new Lesson(lesson.ID, lesson.ID_part, lesson.Name, lesson.Type, true);

                                            Grid.SetRow(new_less.fr, less_count);

                                            TapGestureRecognizer tap = new TapGestureRecognizer();
                                            tap.Command = new Command(async () => await Navigation.PushAsync(new ContentLesson(i, lesson.ID)));
                                            new_less.fr.Content.GestureRecognizers.Add(tap);

                                            new_part.grid_fr.Children.Add(new_less.fr);
                                            new_part.grid_fr.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                                            less_count++;
                                        }
                                    }
                                }
                                for (int? j = min_lesson; j <= all_lesson; j++)
                                {
                                    var lesson = await db.GetInactiveLesson(username, i, j);
                                    if (lesson != null)
                                    {
                                        if (lesson.ID == j)
                                        {
                                            Lesson new_less = new Lesson(lesson.ID, lesson.ID_part, lesson.Name, lesson.Type, false);

                                            Grid.SetRow(new_less.fr, less_count);

                                            TapGestureRecognizer tap = new TapGestureRecognizer();
                                            tap.Command = new Command(async () => await DisplayAlert("Заблокировано", "Завершите предыдущие уроки, чтобы разблокировать это.", "OK"));
                                            new_less.fr.Content.GestureRecognizers.Add(tap);

                                            new_part.grid_fr.Children.Add(new_less.fr);
                                            new_part.grid_fr.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                                            less_count++;
                                        }
                                    }
                                }
                                less_count = 0;

                                new_part.frame.IsVisible = false;

                                new_part.arr_btn.Command = new Command(async () => { if (!new_part.isOpen) { new_part.isOpen = true; new_part.frame.IsVisible = true; await new_part.arr_btn.RotateTo(180, 500); new_part.arr_btn.Rotation = 180; } else { new_part.isOpen = false; new_part.frame.IsVisible = false; await new_part.arr_btn.RotateTo(360, 500); new_part.arr_btn.Rotation = 0; } });

                                Grid.SetRow(new_part.stack, part_count);

                                Part_Grid.Children.Add(new_part.stack);
                                Part_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                                part_count++;

                                //var animation = new Animation(v => new_part.stack.Opacity = v, 0, 1);
                                //animation.Commit(this, "VisibleCourse", 16, 1000, Easing.SinInOut, (v, c) => new_part.stack.Opacity = 1, () => false);
                            }
                        }
                    }
                }
                else
                    await DisplayAlert("Oppss..", "Произошла ошибка, попробуйте позже", "OK");
                isLoad = true;
            }

            Part_Content.IsVisible = true;
            indicator.IsVisible = false;

            Content = scrl;
        }
        private async void add_favourites_Clicked(object sender, EventArgs e) // add to favourites
        {
            DB dB = new DB();

            var status = await dB.CheckAddedFav(username, id);
            if (status == false)
            {
                try
                {
                    fav_btn.Source = "add_fav.png";
                    await dB.AddFavouriteCourse(username, id);
                    await DisplayAlert("Успешно", "Выбранный курс успешно добавлен в избранное!", "OK");
                }
                catch
                {
                    await DisplayAlert("Oppss..", "Произошла ошибка, попробуйте позже", "OK");
                }
            }
            else
            {
                try
                {
                    fav_btn.Source = "non_fav.png";
                    await dB.RemoveFavouriteCourse(username, id);
                    await DisplayAlert("Успешно", "Выбранный курс успешно удален из избранного!", "OK");
                }
                catch
                {
                    await DisplayAlert("Oppss..", "Произошла ошибка, попробуйте позже", "OK");
                }
            }
        }
    }
}