using language_app.ViewModels;
using language_app.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace language_app
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("//LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("//RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("//Lessons", typeof(Lessons));
            Routing.RegisterRoute("//StartCourse", typeof(StartCourse));
        }
    }
}
