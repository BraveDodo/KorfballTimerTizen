using KorfballTimerTizen.Helpers;
using KorfballTimerTizen.Model;
using KorfballTimerTizen.res;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KorfballTimerTizen.Views
{
	public class EditTime : ContentPage
	{
        #region Pagedeclarations
        Grid LayoutRoot;
        StackLayout HeaderStack;
        StackLayout HeaderSubStack;
        Image Logo;
        Label NameTxt;
        Label PageTxt;
        Image EditImage;
        ListView listBoxobjM;
        ListView listBoxobjS;
        #endregion

        #region declarations
        ObservableCollection<Numbers> timelistM = new ObservableCollection<Numbers>();
        ObservableCollection<Numbers> timelistS = new ObservableCollection<Numbers>();
        TeamTypeHelper tth = new TeamTypeHelper();
        NumbersHelper nh = new NumbersHelper();
        SettingHelper sh = new SettingHelper();
        public TeamType teamtype;
        Numbers number;
        TimerSettings setting;
        string min;
        string sec;
        Match addMatch;
        bool eXtra;
        bool screentime;
        private string message;
        private bool changed;
        #endregion

        public EditTime (TimeSpan time)
		{
            NavigationPage.SetHasNavigationBar(this, false);
            setting = sh.GetSetting();
            screentime = true;
            SetupMainPage();

            SetupTimelist(time, 30);
        }

        public EditTime(TeamType tt, Match addmatch, bool extra)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            setting = sh.GetSetting();
            screentime = false;
            teamtype = tt;
            addMatch = addmatch;
            SetupMainPage();
            eXtra = extra;
            if (eXtra)
                SetupTimelist(teamtype.PlayTime1, 15);
            else
                SetupTimelist(teamtype.PlayTime, 30);


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AddTapGestureRecognizers();
            #region toolbar
            EditImage.Source = "Edit.png";
            NameTxt.Text = AppResources.NameTxt;
            PageTxt.Text = AppResources.TimeChoise;
            //NameTxt.FontSize = 14 * setting.ScreenWidth / 183;
            //if (PageTxt.Text.Length > 12)
            //    PageTxt.FontSize = 12 / PageTxt.Text.Length * setting.ScreenWidth / 183;
            //else
            //    PageTxt.FontSize = 12 * setting.ScreenWidth / 183;
            Logo.Source = "KorfbalWit.png";
            min = AppResources.min;
            sec = AppResources.sec;
            //Logo.HeightRequest = 35 * setting.ScreenWidth / 183;
            #endregion

        }


        #region voids
        private void AddTapGestureRecognizers()
        {
            var tapEditImage = new TapGestureRecognizer();
            tapEditImage.Tapped += (s, e) =>
            {
                App.Vibrate();
                EditButton_Click(s, e);
            };
            EditImage.GestureRecognizers.Add(tapEditImage);
        }

        private void SetupTimelist(TimeSpan time, int maxtime)
        {
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < maxtime + 1; i++)
                    AddNumber(i, j, maxtime, min, timelistM);
            }
            listBoxobjM.ItemsSource = timelistM.OrderBy(i => i.Id).ToList();
            timelistS.Clear();
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 60; i++)
                    AddNumber(i, j, 60, sec, timelistS);
            }
            listBoxobjS.ItemsSource = timelistS.OrderBy(i => i.Id).ToList();
            SetPlayTime(time, 30);
        }

        private async void SetPlayTime(System.TimeSpan playtime, byte maxtime)
        {
            for (int i = 0; i < 60; i++)
            {
                if (i == playtime.Minutes)
                {
                    listBoxobjM.SelectedItem = timelistM.ElementAt(i + 3 * maxtime + 2);
                    await Task.Delay(100);
                    listBoxobjM.ScrollTo(listBoxobjM.SelectedItem, ScrollToPosition.MakeVisible, false);
                    await Task.Delay(100);
                    listBoxobjM.SelectedItem = timelistM.ElementAt(i + 3 * maxtime + 3);
                }
                if (i == playtime.Seconds)
                {
                    listBoxobjS.SelectedItem = timelistS.ElementAt(i + 3 * 60 - 1);
                    await Task.Delay(100);
                    listBoxobjS.ScrollTo(listBoxobjS.SelectedItem, ScrollToPosition.MakeVisible, false);
                    await Task.Delay(100);
                    listBoxobjS.SelectedItem = timelistS.ElementAt(i + 3 * 60);
                }
            }
        }

        private void AddNumber(int i, int j, int factor, string description, ObservableCollection<Numbers> timelist)
        {
            number = new Numbers
            {
                Id = i + j * factor,
                I = i,
                J = j,
                Description = description
            };
            if (i < 10)
                number.Number = "0" + i.ToString();
            else
                number.Number = i.ToString();
            timelist.Add(number);
        }
        #endregion

        private async void EditButton_Click(object sender, EventArgs e)
        {
            Numbers min = (Numbers)listBoxobjM.SelectedItem;
            Numbers sec = (Numbers)listBoxobjS.SelectedItem;
            if (screentime)
            {
                setting.ScreenTime = TimeSpan.Parse("00:" + min.Number + ":" + sec.Number);
                sh.UpdateSetting(setting);
                await Navigation.PushAsync(new SettingsPage());
            }
            else
            {
                if (eXtra)
                    teamtype.PlayTime1 = TimeSpan.Parse("00:" + min.Number + ":" + sec.Number);
                else
                    teamtype.PlayTime = TimeSpan.Parse("00:" + min.Number + ":" + sec.Number);
                if (teamtype.PlayTime1 > teamtype.PlayTime)
                    message = await DisplayActionSheet(AppResources.ExtraTimeLarge, null, null, AppResources.ok);
                //message = await DependencyService.Get<IActionSheet>().UseActionSheet(this, "", AppResources.ExtraTimeLarge, "", "");
                else
                    tth.UpdateTeamType(teamtype);
                await Navigation.PushAsync(new AddMatch(addMatch));
            }
        }

        private void LayoutRoot_SizeChanged(object sender, EventArgs e)
        {
            if (setting.Round)
                Padding = new Thickness(Width * .1465, Width * .1465, Width * .1465, Width * .1465 - Width + Height);

        }

        private void SetupMainPage()
        {
            if (!changed)
            {
                changed = true;

                AddMainGrid();
                AddHeaderStack();
                AddMinutesView();
                AddSecondsViews();
            }
        }

        private void AddMainGrid()
        {
            LayoutRoot = new Grid() { BackgroundColor = Color.Maroon };
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
             LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            
            LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //ContentPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

            //Content = new ScrollView
            //{
            //    Orientation = ScrollOrientation.Vertical,
            Content = LayoutRoot;
            //};

        }

        private void AddHeaderStack()
        {
            HeaderStack = new StackLayout() { Orientation = StackOrientation.Horizontal };

            Logo = new Image
            {
                Source = ImageSource.FromFile("KorfbalWit.png"),
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center
            };
            if (setting.Round)
                Logo.Margin = new Thickness(20, 0, 0, 0);
            HeaderStack.Children.Add(Logo);

            HeaderSubStack = new StackLayout() { Orientation = StackOrientation.Vertical };
            HeaderStack.Children.Add(HeaderSubStack);
            NameTxt = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                HorizontalOptions = LayoutOptions.Start
            };
            HeaderSubStack.Children.Add(NameTxt);
            PageTxt = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                HorizontalOptions = LayoutOptions.Start
            };
            HeaderSubStack.Children.Add(PageTxt);

            EditImage = new Image
            {
                Source = ImageSource.FromFile("Edit.png"),
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center
            };
            HeaderStack.Children.Add(EditImage);
            Grid.SetColumnSpan(HeaderStack, 2);
            LayoutRoot.Children.Add(HeaderStack);
        }

        private void AddMinutesView()
        {
            listBoxobjM = new ListView()
            {
                RowHeight = 80,
                WidthRequest = 100,
                BackgroundColor=Color.Transparent,
                VerticalOptions=LayoutOptions.Start,
                HorizontalOptions=LayoutOptions.Center,
                ItemTemplate = new DataTemplate(typeof(MinutesViewCell)),
            };
            LayoutRoot.Children.Add(listBoxobjM);
            Grid.SetRow(listBoxobjM, 1);
        }

        public class MinutesViewCell : ViewCell
        {
            public MinutesViewCell()
            {
                //instantiate each of our views
                var ListGrid = new Grid();

                AddTimeScroll(ListGrid);


                ListGrid.HorizontalOptions = LayoutOptions.Fill;

                View = ListGrid;
            }

            private void AddTimeScroll(Grid listgrid)
            {
                var timeLabel = new Label()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.Black,
                    FontSize = 15,
                    WidthRequest=80,
                    HeightRequest=70,
                    BackgroundColor=Color.White,
                };
                timeLabel.SetBinding(Label.TextProperty, new Binding("Number"));
                listgrid.Children.Add(timeLabel);

                var descriptionLabel = new Label()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalTextAlignment=TextAlignment.Center,
                    VerticalTextAlignment=TextAlignment.Center,
                    TextColor = Color.Black,
                    FontSize = 7,
                    WidthRequest = 80,
                    //Margin = new Thickness(0, -10, 0, 0),
                    BackgroundColor = Color.White,
                };
                descriptionLabel.SetBinding(Label.TextProperty, new Binding("Description"));
                listgrid.Children.Add(descriptionLabel);

            }
        }
        
        private void AddSecondsViews()
        {
            listBoxobjS = new ListView()
            {
                RowHeight = 80,
                WidthRequest = 100,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                ItemTemplate = new DataTemplate(typeof(SecondsViewCell)),
            };
            LayoutRoot.Children.Add(listBoxobjS);
            Grid.SetRow(listBoxobjS, 1);
            Grid.SetColumn(listBoxobjS, 1);
        }

        public class SecondsViewCell : ViewCell
        {
            public SecondsViewCell()
            {
                //instantiate each of our views
                var ListGrid = new Grid();

                AddTimeScroll(ListGrid);


                ListGrid.HorizontalOptions = LayoutOptions.Fill;

                View = ListGrid;
            }

            private void AddTimeScroll(Grid listgrid)
            {
                var timestack = new StackLayout { Orientation = StackOrientation.Vertical };
                var timeLabel = new Label()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.Black,
                    FontSize = 15,
                    WidthRequest = 80,
                    HeightRequest = 70,
                    BackgroundColor = Color.White,
                };
                timeLabel.SetBinding(Label.TextProperty, new Binding("Number"));
                timestack.Children.Add(timeLabel);

                var descriptionLabel = new Label()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    TextColor = Color.Black,
                    FontSize = 6,
                    WidthRequest = 80,
                    Margin = new Thickness(0, -10, 0, 0),
                    BackgroundColor = Color.White,
                };
                descriptionLabel.SetBinding(Label.TextProperty, new Binding("Description"));
                timestack.Children.Add(descriptionLabel);

                listgrid.Children.Add(timestack);
            }
        }

    }
}