using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace language_app.Models
{
    public class Achiev
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image {  get; set; }
        public string Description { get; set; }

        public Frame frame = new Frame();

        public Achiev(int id, string name, string image, string description)
        {
            ID = id;
            Name = name;
            Image = image;
            Description = description;

            frame.HasShadow = true;
            frame.CornerRadius = 10;
            frame.Margin = new Thickness(20, 0, 20, 0);
            frame.HeightRequest = 50;
            frame.HorizontalOptions = LayoutOptions.FillAndExpand;
            frame.BackgroundColor = Color.FromRgb(55, 55, 55);

            Grid grid = new Grid();

            Image img = new Image
            {
                Source = image,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex("#373737")
            };

            Label name_course = new Label
            {
                Text = name,
                TextColor = Color.White,
                FontSize = 22,
                Margin = new Thickness(0, -8, 50, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            Line line = new Line
            {
                X1 = 70,
                X2 = 400,
                Stroke = Color.White,
                Margin = new Thickness(0, 25, 0, 0)
            };

            Label desc_course = new Label
            {
                Text = description,
                TextColor = Color.White,
                FontSize = 15,
                Margin = new Thickness(15, 0, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            grid.Children.Add(img);
            grid.Children.Add(name_course);
            grid.Children.Add(line);
            grid.Children.Add(desc_course);

            frame.Content = grid;
        }
    }
}
