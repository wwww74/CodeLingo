using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace language_app.Models
{
    public partial class Course : ContentPage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public long? Actual_lesson { get; set; }
        public long? General_lesson { get; set; }
        public bool Status { get; set; }

        public Frame frame = new Frame();

        public Course(int id, string new_img, string new_name, long? actual_count, long? general_count, bool status)
        {
            Id = id;
            Image = new_img;
            Name = new_name;
            Actual_lesson = actual_count;
            General_lesson = general_count;
            Status = status;

            frame.HasShadow = true;
            frame.CornerRadius = 10;
            frame.Margin = new Thickness(20, 0, 20, 0);
            frame.HeightRequest = 50;
            frame.HorizontalOptions = LayoutOptions.FillAndExpand;
            frame.BackgroundColor = Color.FromRgb(55, 55, 55);

            Grid grid = new Grid();

            Image img = new Image
            {
                Source = new_img,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = Color.FromRgb(55, 55, 55)
            };

            Label name_course = new Label
            {
                Text = new_name,
                TextColor = Color.White,
                FontSize = 19,
                Margin = new Thickness(105, -10, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Label progress_course = new Label
            {
                Text = "Прогресс:",
                TextColor = Color.White,
                FontSize = 14,
                Margin = new Thickness(0, 20, 35, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Label actual_less_count = new Label
            {
                Text = actual_count.ToString(),
                TextColor = Color.White,
                FontSize = 14,
                Margin = new Thickness(180, 20, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Label label_slesh = new Label
            {
                Text = "/ ",
                TextColor = Color.White,
                FontSize = 14,
                Margin = new Thickness(195, 20, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Label general_less_count = new Label
            {
                Text = general_count.ToString(),
                TextColor = Color.White,
                FontSize = 14,
                Margin = new Thickness(210, 20, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            ProgressBar progress = new ProgressBar
            {
                ScaleX = 0.4,
                Margin = new Thickness(20, 45, 0, 0),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Progress = Convert.ToDouble(actual_count) / Convert.ToDouble(general_count),
                ProgressColor = Color.FromRgb(51, 255, 51)
            };

            grid.Children.Add(img);
            grid.Children.Add(name_course);

            if (status == true)
            {
                grid.Children.Add(progress_course);
                grid.Children.Add(actual_less_count);
                grid.Children.Add(label_slesh);
                grid.Children.Add(general_less_count);
                grid.Children.Add(progress);
            }
            else
            {
                name_course.VerticalOptions = LayoutOptions.CenterAndExpand; 
            }

            frame.Content = grid;
        }
    }
}
