using KorfballTimerTizen.Helpers;
using KorfballTimerTizen.Model;
using KorfballTimerTizen.res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace KorfballTimerTizen.Views
{
    public class WearTypePage : ContentPage
    {
        private Grid LayoutRoot;
        Label Choise;
        Label ChoiseRound;
        Label ChoiseSquare;
        Image WatchRound;
        Image WatchSquare;

        #region declarations
        private SettingHelper sh = new SettingHelper();
        private TimerSettings setting;
        private bool changed;
        #endregion

        public WearTypePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.Maroon;
            SetupMainPage();
            setting = sh.GetSetting();
        }

        private void AddTapGestureRecognizers()
        {
            var tapRound = new TapGestureRecognizer();
            tapRound.Tapped += (s, e) =>
            {
                App.Vibrate();
                SetSquare(s, e);
            };
            WatchSquare.GestureRecognizers.Add(tapRound);
            ChoiseRound.GestureRecognizers.Add(tapRound);

            var tapSquare = new TapGestureRecognizer();
            tapSquare.Tapped += (s, e) =>
            {
                App.Vibrate();
                SetRound(s, e);
            };
            WatchRound.GestureRecognizers.Add(tapSquare);
            ChoiseSquare.GestureRecognizers.Add(tapSquare);
        }

        private void SetRound(object s, EventArgs e)
        {
            setting.Round = true;
            setting.ScreenWidth = Convert.ToInt32(Width * .707);
            GotoNewPage();

        }

        private void SetSquare(object s, EventArgs e)
        {
            setting.Round = false;
            setting.ScreenWidth = Convert.ToInt32(Width);
            GotoNewPage();
        }

        private async void GotoNewPage()
        {
            sh.UpdateSetting(setting);
            if (setting.CountryId == 0)
            {
                await Navigation.PushAsync(new SetUpCountryPage());
            }
            else
            {
                MatchHelper mh = new MatchHelper();
                ActiveMatch activematch = mh.GetActiveMatch();
                Match match = mh.GetMyMatch(activematch.MatchId);
                if (match == null)
                {
                    activematch.MatchId = -1;
                    mh.UpdateActiveMatch(activematch);
                }
                if (activematch.MatchId == -1)
                    await Navigation.PushAsync(new MyMatches());
                else
                    await Navigation.PushAsync(new TimerPage());

            }
        }

        private void SetupMainPage()
        {
            if (!changed)
            {
                changed = true;

                AddMainGrid();

                AddLabels();
                AddImages();
                AddTapGestureRecognizers();
            }
        }

        private void AddMainGrid()
        {
            LayoutRoot = new Grid() { BackgroundColor = Color.Maroon };
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
            Content = LayoutRoot;
        }

        private void LayoutRoot_SizeChanged(object sender, EventArgs e)
        {
            Padding = new Thickness(Width * .1465, Width * .1465, Width * .1465, Width * .1465);
        }

        private void AddLabels()
        {
            Choise = new Label
            {
                TextColor = Color.White,
                Text = AppResources.WatchType,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            LayoutRoot.Children.Add(Choise);
            Grid.SetColumnSpan(Choise, 2);

            ChoiseRound = new Label
            {
                TextColor = Color.White,
                FontSize = 10,
                Text= AppResources.Round,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            LayoutRoot.Children.Add(ChoiseRound);
            Grid.SetRow(ChoiseRound, 1);

            ChoiseSquare = new Label
            {
                TextColor = Color.White,
                FontSize = 10,
                Text = AppResources.Square,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            LayoutRoot.Children.Add(ChoiseSquare);
            Grid.SetRow(ChoiseSquare, 1);
            Grid.SetColumn(ChoiseSquare, 1);
        }

        private void AddImages()
        {

            WatchRound = new Image
            {
                Source = ImageSource.FromFile("watch.png"),
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                //Margin=new Thickness(50,0,0,0)
            };
            LayoutRoot.Children.Add(WatchRound);
            Grid.SetRow(WatchRound, 2);

            WatchSquare = new Image
            {
                Source = ImageSource.FromFile("watchsquare.png"),
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                //Margin=new Thickness(50,0,0,0)
            };
            LayoutRoot.Children.Add(WatchSquare);
            Grid.SetRow(WatchSquare, 2);
            Grid.SetColumn(WatchSquare, 1);

        }

    }
}