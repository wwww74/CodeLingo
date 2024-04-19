using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace language_app.Models
{
    public partial class Lesson : ContentPage
    {
        public int ID { get; set; }
        public int ID_part { get; set; }
        public string Name { get; set; }
        public bool Type { get; set; }

        public Frame fr = new Frame();
        public Image img = new Image();

        public Lesson(int id, int id_part, string name, bool type, bool isActive)
        {
            ID = id;
            ID_part = id_part;
            Name = name;
            Type = type;

            fr.CornerRadius = 5;
            fr.BackgroundColor = Color.FromHex("#404040");

            Grid grid = new Grid();

            if (!isActive)
            {
                fr.Opacity = 0.5;
                img.Source = "locked.png";
                img.Scale = 0.5;
                img.HorizontalOptions = LayoutOptions.EndAndExpand;
                img.Margin = new Thickness(0, -30, -15, 0);
                grid.Children.Add(img);
            }
            else
                fr.Opacity = 1;

            Image image = new Image
            {
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Label type_label = new Label
            {
                TextColor = Color.FromHex("#b2b2b2"),
                FontSize = 14,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(50, -15, 0, 0)
            };

            if (!type)
            {
                type_label.Text = "Урок";
                image.Source = "book.png";
            }
            else
            {
                type_label.Text = "Практика";
                image.Source = "feedback.png";
            }

            Label name_label = new Label
            {
                Text = name,
                TextColor = Color.White,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(50, 5, 0, 0)
            };

            grid.Children.Add(image);
            grid.Children.Add(type_label);
            grid.Children.Add(name_label);

            fr.Content = grid;
        }
    }
}
