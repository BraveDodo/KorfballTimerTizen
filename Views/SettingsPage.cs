using KorfballTimerTizen.Helpers;
using KorfballTimerTizen.Model;
using KorfballTimerTizen.res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;

namespace KorfballTimerTizen.Views
{
    public class SettingsPage : CirclePage
    {
        #region pagedeclarations
        Grid LayoutRoot;
        StackLayout HeaderStack;
        StackLayout HeaderSubStack;
        Image Logo;
        Label NameTxt;
        Label PageTxt;
        Image EditImage;
        Label Vibrationtimetxt;
        Entry Vibrationtime;
        Switch indoor;
        Label indoorlabel;
        Switch checkBoxSP;
        Label checkBoxSPlabel;
        Switch checkAlarm;
        Label checkAlarmlabel;
        Switch checkEnd;
        Label checkEndlabel;
        Switch checkTimeOut;
        Label checkTimeOutlabel;
        Switch checkMatch;
        Label checkMatchlabel;
        Switch checkStats;
        Label checkStatslabel;
        Switch checkNotes;
        Label checkNoteslabel;
        Switch checkStandardNotes;
        Label checkStandardNoteslabel;
        Switch checkScreenOn;
        Label checkScreenOnlabel;
        Button TimeTxt;
        int gridrow;
        #endregion

        #region declarations
        private MatchHelper mh = new MatchHelper();
        private SettingHelper sh = new SettingHelper();
        public int id;
        public int mnd = DateTime.Now.Month;
        public string parameter;
        private TimerSettings setting;
        private RefSettings refsetting;
        private bool changed;
        #endregion

        
        public SettingsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            setting = sh.GetSetting();
            SetupMainPage();
            //Logo.Source = "res/KorfbalWit.png";
            refsetting = sh.GetRefSetting();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            #region setupPageFields
            NameTxt.Text = AppResources.NameTxt;
            PageTxt.Text = AppResources.Setting;
            checkBoxSPlabel.Text = AppResources.checkBoxSP;
            checkMatchlabel.Text = AppResources.checkMatch;
            checkAlarmlabel.Text = AppResources.checkAlarm;
            checkEndlabel.Text = AppResources.checkEnd;
            Vibrationtimetxt.Text = AppResources.Vibrate;
            checkStatslabel.Text = AppResources.checkStats;
            checkNoteslabel.Text = AppResources.subnumber;
            checkStandardNoteslabel.Text = AppResources.Namecard;
            if (checkTimeOut.IsToggled)
                checkTimeOutlabel.Text = AppResources.TimeOutOn;
            else
                checkTimeOutlabel.Text = AppResources.TimeOutOff;
            if (indoor.IsToggled)
                indoorlabel.Text = AppResources.Indoors;
            else
                indoorlabel.Text = AppResources.Outdoors;
            if (checkScreenOn.IsToggled)
                checkScreenOnlabel.Text = AppResources.ScreenOn;
            else
                checkScreenOnlabel.Text = AppResources.ScreenTime;
            Refereesetting();
            //Content = new ScrollView
            //{ Content = ContentPanel };

            #endregion

            if (setting.Indoor)
                id = 2;
            else
                id = 1;
            Vibrationtime.Text = setting.Vibrate.ToString();
        }

        #region voids
        private void AddTapGestureRecognizers()
        {
            var tapEditImage = new TapGestureRecognizer();
            tapEditImage.Tapped += (s, e) =>
            {
                Matches_Click(s, e);
            };
            EditImage.GestureRecognizers.Add(tapEditImage);
        }
        
        private void SetSetting()
        {
            setting.Indoor = indoor.IsToggled;
            if (Vibrationtime.Text != null)
                setting.Vibrate = int.Parse(Vibrationtime.Text);
        }

        private void Refereesetting()
        {
            //checkBoxSPlabel.IsVisible = true;
            //checkBoxSP.IsVisible = true;
            checkBoxSP.IsToggled = refsetting.StopAtChange;
            checkMatch.IsToggled = refsetting.SaveMatch;
            //checkAlarmlabel.IsVisible = true;
            //checkAlarm.IsVisible = true;
            checkAlarm.IsToggled = refsetting.Audio1Min;
            checkStats.IsToggled = refsetting.MatchStats;
            //checkEndlabel.IsVisible = true;
            //checkEnd.IsVisible = true;
            checkEnd.IsToggled = refsetting.AudioEnd;
            checkTimeOut.IsToggled = refsetting.TimeOut;
            //checkTimeOut.IsVisible = true;
            //checkTimeOutlabel.IsVisible = true;
            checkNotes.IsToggled = refsetting.SubNumber;
            checkStandardNotes.IsToggled = refsetting.CardName;
            checkScreenOn.IsToggled = setting.ScreenOn;
            TimeTxt.IsVisible = !checkScreenOn.IsToggled;
            //checkTimeOutlabel.IsVisible = !checkScreenOn.IsToggled;
            TimeTxt.Text = setting.ScreenTime.ToString(@"mm\:ss");
        }

        private void SetRefsetting()
        {
            refsetting.StopAtChange = checkBoxSP.IsToggled;
            refsetting.SaveMatch = checkMatch.IsToggled;
            refsetting.MatchStats = checkStats.IsToggled;
            refsetting.Audio1Min = checkAlarm.IsToggled;
            refsetting.AudioEnd = checkEnd.IsToggled;
            refsetting.TimeOut = checkTimeOut.IsToggled;
            refsetting.SubNumber = checkNotes.IsToggled;
            refsetting.CardName = checkStandardNotes.IsToggled;
            sh.UpdateRefSetting(refsetting);
        }

        private void UpdateRefSetting()
        {
            refsetting.StopAtChange = checkBoxSP.IsToggled;
            refsetting.SaveMatch = checkMatch.IsToggled;
            refsetting.MatchStats = true;
            refsetting.Audio1Min = (checkAlarm.IsToggled);
            refsetting.AudioEnd = checkEnd.IsToggled;
            refsetting.TimeOut = checkTimeOut.IsToggled;
            refsetting.SubNumber = checkNotes.IsToggled;
            refsetting.CardName = checkStandardNotes.IsToggled;
            sh.UpdateRefSetting(refsetting);
        }

        private void UpdateSettings()
        {
            UpdateRefSetting();
            SetSetting();
            sh.UpdateSetting(setting);
        }

        private async void EditButtonClick()
        {
            SetRefsetting();

            SetSetting();
            sh.UpdateSetting(setting);

            await Navigation.PushAsync(new MyMatches());
        }
        #endregion

        #region clickactions
        private async void Matches_Click(object sender, EventArgs e)
        {
            UpdateSettings();
            await Navigation.PushAsync(new MyMatches());
        }

        #endregion

        #region changeactions

        private async void Vibrationtime_LostFocus(object sender, EventArgs e)
        {
            if (Vibrationtime.Text.ToString() == "" || int.Parse(Vibrationtime.Text) < 0)
            {
                Vibrationtime.Text = setting.Vibrate.ToString();
            }
            else
            {
                if (int.Parse(Vibrationtime.Text) > 999)
                {
                    string msgtxt = "< 1000 ms";
                    string message = await DisplayActionSheet(msgtxt, null, null, AppResources.ok);
                    //message = await DependencyService.Get<IActionSheet>().UseActionSheet(this, msgtxt, "OK", "");
                    Vibrationtime.Text = setting.Vibrate.ToString();
                }
                else
                {
                    setting.Vibrate = int.Parse(Vibrationtime.Text);
                    App.Vibrate();
                }
            }
        }

        private void CheckTimeOut_Checked(object sender, ToggledEventArgs e)
        {
            if (checkTimeOut.IsToggled)
                checkTimeOutlabel.Text = AppResources.TimeOutOn;
            else
                checkTimeOutlabel.Text = AppResources.TimeOutOff;
        }

        private void Indoor_Toggled(object sender, ToggledEventArgs e)
        {
            if (indoor.IsToggled)
                indoorlabel.Text = AppResources.Indoors;
            else
                indoorlabel.Text = AppResources.Outdoors;
        }

        private void LayoutRoot_SizeChanged(object sender, EventArgs e)
        {
            if (setting.Round)
                Padding = new Thickness(Width * .1465 - 20, Width * .1465, Width * .1465, Width * .1465 - Width + Height);

        }

        private void CheckScreenOn_Toggled(object sender, ToggledEventArgs e)
        {
            TimeTxt.IsVisible = !checkScreenOn.IsToggled;
            if (checkScreenOn.IsToggled)
                checkScreenOnlabel.Text = AppResources.ScreenOn;
            else
                checkScreenOnlabel.Text = AppResources.ScreenTime;
            setting.ScreenOn = checkScreenOn.IsToggled;
            sh.UpdateSetting(setting);
        }

        private async void TimeTxt_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTime(setting.ScreenTime));
        }
        #endregion
        
        #region pageSetup
        private void SetupMainPage()
        {
            if (!changed)
            {
                changed = true;

                AddMainGrid();
                AddHeaderStack();
                AddVibration();
                AddSwitches();
                AddScreentime();
                AddTapGestureRecognizers();
            }
        }

        private void AddMainGrid()
        {
            LayoutRoot = new Grid() { BackgroundColor = Color.Black };
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //ContentPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = LayoutRoot
            };
            
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

        private void AddVibration()
        {
            gridrow = 1;
            Vibrationtime = new Entry
            {
                TextColor = Color.Black,
                BackgroundColor = Color.White,
                FontSize = 10,
                WidthRequest = 80,
                //HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                Keyboard = Keyboard.Numeric
            };
            Vibrationtime.Completed += Vibrationtime_LostFocus;
            LayoutRoot.Children.Add(Vibrationtime);
            Grid.SetRow(Vibrationtime, gridrow);
            Vibrationtimetxt = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(Vibrationtimetxt);
            Grid.SetColumn(Vibrationtimetxt, 1);
            Grid.SetRow(Vibrationtimetxt, gridrow);
            gridrow++;
        }

        private void AddSwitches()
        {
            indoor = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            indoor.Toggled += Indoor_Toggled;
            LayoutRoot.Children.Add(indoor);
            Grid.SetRow(indoor, gridrow);
            indoorlabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(indoorlabel);
            Grid.SetColumn(indoorlabel, 1);
            Grid.SetRow(indoorlabel, gridrow);
            gridrow++;

            checkBoxSP = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            LayoutRoot.Children.Add(checkBoxSP);
            Grid.SetRow(checkBoxSP, gridrow);
            checkBoxSPlabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkBoxSPlabel);
            Grid.SetColumn(checkBoxSPlabel, 1);
            Grid.SetRow(checkBoxSPlabel, gridrow);
            gridrow++;

            checkAlarm = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            LayoutRoot.Children.Add(checkAlarm);
            Grid.SetRow(checkAlarm, gridrow);
            checkAlarmlabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkAlarmlabel);
            Grid.SetColumn(checkAlarmlabel, 1);
            Grid.SetRow(checkAlarmlabel, gridrow);
            gridrow++;

            checkEnd = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            LayoutRoot.Children.Add(checkEnd);
            Grid.SetRow(checkEnd, gridrow);
            checkEndlabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkEndlabel);
            Grid.SetColumn(checkEndlabel, 1);
            Grid.SetRow(checkEndlabel, gridrow);
            gridrow++;

            checkTimeOut = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            checkTimeOut.Toggled += CheckTimeOut_Checked;
            LayoutRoot.Children.Add(checkTimeOut);
            Grid.SetRow(checkTimeOut, gridrow);
            checkTimeOutlabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkTimeOutlabel);
            Grid.SetColumn(checkTimeOutlabel, 1);
            Grid.SetRow(checkTimeOutlabel, gridrow);
            gridrow++;

            checkMatch = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            LayoutRoot.Children.Add(checkMatch);
            Grid.SetRow(checkMatch, gridrow);
            checkMatchlabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkMatchlabel);
            Grid.SetColumn(checkMatchlabel, 1);
            Grid.SetRow(checkMatchlabel, gridrow);
            gridrow++;

            checkStats = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            LayoutRoot.Children.Add(checkStats);
            Grid.SetRow(checkStats, gridrow);
            checkStatslabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkStatslabel);
            Grid.SetColumn(checkStatslabel, 1);
            Grid.SetRow(checkStatslabel, gridrow);
            gridrow++;

            checkNotes = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            LayoutRoot.Children.Add(checkNotes);
            Grid.SetRow(checkNotes, gridrow);
            checkNoteslabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkNoteslabel);
            Grid.SetColumn(checkNoteslabel, 1);
            Grid.SetRow(checkNoteslabel, gridrow);
            gridrow++;

            checkStandardNotes = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            LayoutRoot.Children.Add(checkStandardNotes);
            Grid.SetRow(checkStandardNotes, gridrow);
            checkStandardNoteslabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkStandardNoteslabel);
            Grid.SetColumn(checkStandardNoteslabel, 1);
            Grid.SetRow(checkStandardNoteslabel, gridrow);
            gridrow++;
        }

        private void AddScreentime()
        {

            checkScreenOn = new Switch
            {
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = 0.6
            };
            checkScreenOn.Toggled += CheckScreenOn_Toggled;
            LayoutRoot.Children.Add(checkScreenOn);
            Grid.SetRow(checkScreenOn, gridrow);
            checkScreenOnlabel = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            LayoutRoot.Children.Add(checkScreenOnlabel);
            Grid.SetColumn(checkScreenOnlabel, 1);
            Grid.SetRow(checkScreenOnlabel, gridrow);
            gridrow++;

            TimeTxt = new Button
            {
                TextColor = Color.White,
                BackgroundColor = Color.Black,
                FontSize = 6,
                HorizontalOptions = LayoutOptions.Center,
            };
            TimeTxt.Clicked += TimeTxt_Clicked;
            LayoutRoot.Children.Add(TimeTxt);
            //Grid.SetColumn(TimeTxt, 1);
            Grid.SetColumnSpan(TimeTxt, 2);
            Grid.SetRow(TimeTxt, gridrow);
            gridrow++;
        }
        #endregion Pagesetup
    }
}