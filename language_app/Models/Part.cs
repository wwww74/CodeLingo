using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace language_app.Models
{
    public partial class Part : ContentPage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool isOpen { get; set; }

        public StackLayout stack = new StackLayout();
        public ImageButton arr_btn = new ImageButton();
        public Frame frame = new Frame();
        public Grid grid_fr = new Grid();
        public Part(int id, int id_course, string new_name, bool status)
        {
            ID = id;
            Name = new_name;
            Status = status;
            isOpen = false;

            Grid grid = new Grid();

            Image img = new Image
            {
                Scale = 0.7,
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };

            if (status == true)
                img.Source = "unlocked.png";
            else
                img.Source = "locked.png";

            Label name_part = new Label
            {
                Text = new_name,
                TextColor = Color.White,
                FontSize = 22,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(50, 0, 0, 0)
            };

            arr_btn.Source = "arrow.png";
            arr_btn.Scale = 0.7;
            arr_btn.HorizontalOptions = LayoutOptions.EndAndExpand;
            arr_btn.VerticalOptions = LayoutOptions.CenterAndExpand;
            arr_btn.BackgroundColor = Color.FromHex("#252525");

            Line underline = new Line
            {
                X1 = 0,
                X2 = 400,
                Stroke = Color.White,
            };

            frame.IsVisible = true;
            frame.Margin = new Thickness(0, 5, 0, 0);
            frame.CornerRadius = 5;
            frame.BackgroundColor = Color.FromHex("#252525");
            frame.HasShadow = false;
            frame.Padding = 0;

            frame.Content = grid_fr;

            Grid.SetRow(img, 0);
            Grid.SetRow(name_part, 0);
            Grid.SetRow(arr_btn, 0);
            Grid.SetRow(underline, 1);
            Grid.SetRow(frame, 2);

            grid.Children.Add(img);
            grid.Children.Add(name_part);
            grid.Children.Add(arr_btn);
            grid.Children.Add(underline);
            grid.Children.Add(frame);

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            stack.Children.Add(grid);
        }
    }
}