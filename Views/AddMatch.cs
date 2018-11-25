using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace KorfballTimerTizen.Views
{
    public class AddMatch : ContentPage
    {
        public AddMatch()
        {
        }

        public AddMatch(Model.Match addMatch)
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                }
            };
        }
    }
}