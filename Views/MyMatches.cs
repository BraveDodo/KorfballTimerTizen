using KorfballTimerTizen.Helpers;
using KorfballTimerTizen.Model;
using KorfballTimerTizen.res;
using Plugin.Battery;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KorfballTimerTizen.Views
{
    public class MyMatches : ContentPage
    {
        #region pagedeclarations
        Grid LayoutRoot;
        StackLayout HeaderStack;
        Image Add;
        Image EditImage;
        Image PlayMatch;
        Image Setting;
        StackLayout HeaderSubStack;
        Label NameTxt;
        Label PageTxt;
        BoxView batteryBarBox;
        BoxView batteryBarempty;
        BoxView batteryBarPlus;
        BoxView batteryBar;
        Label BatteryLevel;
        ScrollView pagescroll;
        Grid StatGrid;
        Grid ProgramGrid;
        StackLayout stackLayout;
        Image Logo;
        Label Program;
        Label Done;
        ScrollView pagescroll1;
        ListView matchListBox;
        Grid doneGrid;
        StackLayout stackLayoutDone;
        Image LogoDone;
        Label DoneDone;
        Label DoneProg;
        Grid headerGridDone;
        Label Home;
        Label Guest;
        Label Score;
        ScrollView pagescroll2;
        ListView listBoxobj;
        #endregion

        #region declarations
        private IEnumerable<Match> matchlist;
        private ObservableCollection<Match> done = new ObservableCollection<Match>();
        private ObservableCollection<Match> program = new ObservableCollection<Match>();
        private MatchHelper mh = new MatchHelper();
        private TeamTypeHelper tth = new TeamTypeHelper();
        private SettingHelper sh = new SettingHelper();
        private TimerSettings setting;
        private Match mymatch;
        private Match activematch;
        private Match match;
        ActiveMatch myactivematch;
        private double scrollx;
        private double scrolly;
        byte tel;
        private bool changed;
        //private bool passed;
        private string home;
        private DateTime matchdate;
        private TimeSpan matchtime;
        private string message;
        private double width;
        private bool timerStart;
        private ObservableCollection<Match> matchlistdoneNew;
        private ObservableCollection<Match> matchlistNew;
        private ObservableCollection<Match> matchlistdone;

        private DateTime time;
        private bool MaySetSync;
        #endregion

        public MyMatches()
        {
            #region toolbar
            NavigationPage.SetHasNavigationBar(this, false);
            setting = sh.GetSetting();
            SetupMainPage();
            //Delete.Source = "Delete.png";
            //Add.Source = "Add.png";
            //EditImage.Source = "Edit.png";
            //PlayMatch.Source = "Timer.png";
            //Setting.Source = "Settings.png";
            //Up.Source = "Up.png";
            //Down.Source = "Down.png";
            Program.Text = AppResources.Programma;
            DoneProg.Text = AppResources.Programma;
            Done.Text = AppResources.Afgehandeld;
            DoneDone.Text = AppResources.Afgehandeld;
            //Logo.Source = "KorfbalWit.png";
            //LogoDone.Source = "KorfbalWit.png";
            #endregion
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetupMatchListBox();

            SetUpTextBoxes();

            GetbatteryLevel();

            scrollx = 0;
            scrolly = 0;

            timerStart = true;
            RunTimer();
        }

        #region timer
        public void RunTimer()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                if (timerStart)
                {
                    // returning true will fire task again in 250 milliseconds.
                    DispatcherTimer_Tick();
                    return true;
                }

                // No longer need to recur. Stops firing task
                return false;
            });

        }

        void DispatcherTimer_Tick()
        {
            time = DateTime.Now;
            if ((time.Second / 10) % 2 == 0)
                BatteryLevel.Text = CrossBattery.Current.RemainingChargePercent.ToString() + " %";
            else
                BatteryLevel.Text = time.ToString(@"HH\:mm");
            matchlistNew = mh.GetMatches(false);
            matchlistdoneNew = mh.GetMatches(true);
            if (matchlistNew.Count() != matchlist.Count() || matchlistdoneNew.Count() != matchlistdone.Count())
                SetupMatchListBox();


        }

        #endregion timer

        #region voids
        private void AddTapGestureRecognizers()
        {
            var tapEdit = new TapGestureRecognizer();
            tapEdit.Tapped += (s, e) =>
            {
                App.Vibrate();
                EditMatchClick();
            };
            EditImage.GestureRecognizers.Add(tapEdit);

            var tapAdd = new TapGestureRecognizer();
            tapAdd.Tapped += (s, e) =>
            {
                App.Vibrate();
                AddMatchClick();
            };
            Add.GestureRecognizers.Add(tapAdd);

            //var tapDeleteImage = new TapGestureRecognizer();
            //tapDeleteImage.Tapped += (s, e) =>
            //{
            //    App.Vibrate();
            //    DeleteMatchClick();
            //};
            //Delete.GestureRecognizers.Add(tapDeleteImage);

            var tapPlayMatch = new TapGestureRecognizer();
            tapPlayMatch.Tapped += (s, e) =>
            {
                App.Vibrate();
                PlayMatchClick();
            };
            PlayMatch.GestureRecognizers.Add(tapPlayMatch);

            var tapSettings = new TapGestureRecognizer();
            tapSettings.Tapped += (s, e) =>
            {
                App.Vibrate();
                SettingsClick();
            };
            Setting.GestureRecognizers.Add(tapSettings);

            var tapDone = new TapGestureRecognizer();
            tapDone.Tapped += (s, e) =>
            {
                App.Vibrate();
                DoneClicked();
            };
            Done.GestureRecognizers.Add(tapDone);

            var tapProgram = new TapGestureRecognizer();
            tapProgram.Tapped += (s, e) =>
            {
                App.Vibrate();
                ProgramClicked();
            };
            DoneProg.GestureRecognizers.Add(tapProgram);

            var tapSync = new TapGestureRecognizer();
            tapSync.Tapped += (s, e) =>
            {
                App.Vibrate();
            };
            
            //var tapUp = new TapGestureRecognizer();
            //tapUp.Tapped += (s, e) =>
            //{
            //    App.Vibrate();
            //    UpClicked();
            //};
            //Up.GestureRecognizers.Add(tapUp);

            //var tapDown = new TapGestureRecognizer();
            //tapDown.Tapped += (s, e) =>
            //{
            //    App.Vibrate();
            //    DownClicked();
            //};
            //Down.GestureRecognizers.Add(tapDown);

        }

        
        private void ActivateNextMatch()
        {
            Match nextmatch = mh.GetNextMatch();
            if (nextmatch == null)
            {
                nextmatch = mh.GetMyMatch(1);
                nextmatch.Done = false;
                nextmatch.HomeScore = 0;
                nextmatch.GuestScore = 0;
                nextmatch.Period = 1;
            }
            if (nextmatch != null)
            {
                nextmatch.Active = true;
                mh.UpdateMatch(nextmatch);
            }
        }

        private void GetbatteryLevel()
        {
            batteryBar.WidthRequest = CrossBattery.Current.RemainingChargePercent * 0.3;
            //batteryBarempty.WidthRequest = (100 - DependencyService.Get<IBattery>().RemainingChargePercent) * 0.6;
            BatteryLevel.Text = CrossBattery.Current.RemainingChargePercent.ToString() + " %";
            BatteryLevel.TranslationX = (100 - CrossBattery.Current.RemainingChargePercent) * 0.3;
            if (CrossBattery.Current.RemainingChargePercent > 50)
                batteryBar.BackgroundColor = Color.FromRgba(153, 255, 51, 255);
            else
            {
                tel = Convert.ToByte(50 - CrossBattery.Current.RemainingChargePercent);
                batteryBar.BackgroundColor = Color.FromRgba(Convert.ToByte(153 + 2 * tel), Convert.ToByte(255 - 5 * tel), Convert.ToByte(51 - tel), 255);
            }
        }

        private async void SetupMatchListBox()
        {
            matchlist = mh.GetMatches(false);
            matchListBox.ItemsSource = matchlist;
            //activematch = mh.GetMatch(false);
            //activematch.FontSize = activematch.FontSize / 2;
            //mh.UpdateMatch(activematch);
            if (mh.GetActiveMatches().Count > 0 && matchlist.Count() > 0)
            {
                activematch = mh.GetMatch(true);
                int tel = 0;
                do
                {
                    if (matchlist.ElementAt(tel).Active)
                    {
                        await Task.Delay(100);
                        if (matchlist.Count() > 0)
                            matchListBox.SelectedItem = matchlist.ElementAt(tel);
                    }
                    tel++;

                }
                while (tel < matchlist.Count());
            }
            matchlistdone = mh.GetMatches(true);
            listBoxobj.ItemsSource = matchlistdone;
        }

        private void SetUpTextBoxes()
        {
            NameTxt.Text = AppResources.NameTxt;
            Score.Text = AppResources.ScoreTB;
            Home.Text = AppResources.Thuis;
            Guest.Text = AppResources.Uit;
            if (Score.Text.Length > 7)
                Score.FontSize = 140 / Score.Text.Length;
            if (Program.Text.Length + Done.Text.Length > 17)
            {
                Program.FontSize = 240 / (Program.Text.Length + Done.Text.Length) * setting.ScreenWidth / 183;
                Done.FontSize = 240 / (Program.Text.Length + Done.Text.Length) * setting.ScreenWidth / 183;
                DoneDone.FontSize = 240 / (Program.Text.Length + Done.Text.Length) * setting.ScreenWidth / 183;
                DoneProg.FontSize = 240 / (Program.Text.Length + Done.Text.Length) * setting.ScreenWidth / 183;
            }
            matchListBox.TranslationY = -10;
            headerGridDone.TranslationY = -5;
            listBoxobj.TranslationY = -10;
        }

        private void DeleteMatchStats()
        {
            ObservableCollection<Model.MatchStat> matchstats = mh.GetMatchStats(mymatch.Id);
            foreach (Model.MatchStat matchstat in matchstats)
                mh.DeleteMatchStat(matchstat);
        }

        private void ActivateMatch()
        {
            match.Active = true;
            mh.UpdateMatch(match);
            activematch = mh.GetMatch(true);
        }

        private void ActivateActiveMatch()
        {
            ObservableCollection<MatchStat> matchstats = mh.GetMatchStats(match.Id);
            myactivematch = mh.GetActiveMatch();
            myactivematch = mh.GetActiveMatch();
            if (myactivematch == null)
            {
                myactivematch = new ActiveMatch();
                myactivematch = mh.GetActiveMatch();
            }
            if (myactivematch.MatchId == -1 || myactivematch.MatchId != match.Id)
            {
                TeamType teamtype = tth.GetTeamType(match.TeamTypeId);
                myactivematch.PlayTime = teamtype.PlayTime;
                myactivematch.MatchId = match.Id;
                myactivematch.PauseTimer = false;
                myactivematch.Reset = true;
                myactivematch.StartTimer = true;
                myactivematch.Notificate0 = true;
                myactivematch.Notificate1 = true;
                if (matchstats.Count() > 0)
                {
                    matchstats = mh.GetMatchSubs(match.Id, AppResources.homesubM, AppResources.homesubF);
                    myactivematch.WisselHome = (byte)mh.GetMatchSubs(match.Id, AppResources.homesubM, AppResources.homesubF).Count();
                    myactivematch.WisselGuest = (byte)mh.GetMatchSubs(match.Id, AppResources.guestsubM, AppResources.guestsubF).Count();
                    myactivematch.TimeOutGuest = 0;
                    myactivematch.TimeOutHome = 0;
                    myactivematch.GuestRed = (byte)mh.GetMatchCards(match.Id, AppResources.guestred).Count();
                    myactivematch.GuestYellow = (byte)mh.GetMatchCards(match.Id, AppResources.guestyellow).Count();
                    myactivematch.HomeRed = (byte)mh.GetMatchCards(match.Id, AppResources.homered).Count();
                    myactivematch.HomeYellow = (byte)mh.GetMatchCards(match.Id, AppResources.homeyellow).Count();
                }
                else
                {
                    myactivematch.WisselHome = 0;
                    myactivematch.WisselGuest = 0;
                    myactivematch.TimeOutGuest = 0;
                    myactivematch.TimeOutHome = 0;
                    myactivematch.GuestRed = 0;
                    myactivematch.GuestYellow = 0;
                    myactivematch.HomeRed = 0;
                    myactivematch.HomeYellow = 0;
                }
                mh.UpdateActiveMatch(myactivematch);
                
            }
        }

        #endregion voids


        private async void Pagescroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            await Task.Delay(50);
            if (Math.Abs(pagescroll.ScrollY - scrolly) < Math.Abs(pagescroll.ScrollX - scrollx))
            {
                if (Math.Abs(pagescroll.ScrollX - scrollx) < 25)
                {
                    if (pagescroll.ScrollX < 100)
                    {
                        PlayMatch.Source = ImageSource.FromFile("Timer.png");
                        await pagescroll.ScrollToAsync(-250, 0, false);
                    }
                    else
                    {
                        PlayMatch.Source = ImageSource.FromFile("AlignCenter.png");
                        await pagescroll.ScrollToAsync(250, 0, false);
                    }
                }
            }
            scrollx = pagescroll.ScrollX;
            scrolly = pagescroll.ScrollY;
            //SetSync();
        }

        private void Listbox_ItemSelected(object sender, EventArgs e)
        {
            //SetSync();
        }

        private void LayoutRoot_SizeChanged(object sender, EventArgs e)
        {
            if (setting.Round)
            {
                Padding = new Thickness(Width * .1465, Width * .1465, Width * .1465, Width * .1465 - Width + Height);
                LayoutRoot.WidthRequest = (Width * .707) * 2 + 5;
                StatGrid.WidthRequest = (Width * .707) * 2 + 5;
                width = Width * .707;
            }
            else
            {
                LayoutRoot.WidthRequest = Width * 2 + 5;
                StatGrid.WidthRequest = Width * 2 + 5;
                width = Width;
            }
            setting.ScreenWidth = Convert.ToInt32(width);
            sh.UpdateSetting(setting);

            Add.WidthRequest = setting.ScreenWidth / 5 - 5;
            EditImage.WidthRequest = setting.ScreenWidth / 5 - 5;
            PlayMatch.WidthRequest = setting.ScreenWidth / 5 - 5;
            Setting.WidthRequest = setting.ScreenWidth / 5 - 5;
            LayoutRoot.RowDefinitions.ElementAt(0).Height = setting.ScreenWidth / 5 - 5;
            //Up.WidthRequest =  setting.ScreenWidth / 8;
            //Down.WidthRequest =  setting.ScreenWidth / 8;
            NameTxt.FontSize = 14 * setting.ScreenWidth / 183;
        }

        #region clickactions
        private async void DoneClicked()
        {
            await pagescroll.ScrollToAsync(250, 0, false);
            PlayMatch.Source = ImageSource.FromFile("AlignCenter.png");
            //SetSync();
        }

        private async void ProgramClicked()
        {
            await pagescroll.ScrollToAsync(0, 0, false);
            PlayMatch.Source = ImageSource.FromFile("Timer.png");
            //SetSync();
        }

        //private async void DownClicked()
        //{
        //    scrolly = pagescroll2.ScrollY;
        //    await pagescroll2.ScrollToAsync(0, scrolly + 80, false);
        //}

        //private async void UpClicked()
        //{
        //    scrolly = pagescroll2.ScrollY;
        //    await pagescroll2.ScrollToAsync(0, scrolly - 80, false);
        //}

        //private async void DeleteMatchClick()
        //{
        //    if (pagescroll.ScrollX < 100)
        //        mymatch = (Match)matchListBox.SelectedItem;
        //    else
        //        mymatch = (Match)listBoxobj.SelectedItem;
        //    //if (await App.ShowQuestion(AppResources.deleteMatch, AppResources.yes, AppResources.no))
        //    if (await DependencyService.Get<IActionSheet>().UseActionSheet(this, AppResources.deleteMatch, AppResources.yes, AppResources.no, "") == AppResources.yes)
        //    {
        //        ObservableCollection<Model.MatchStat> matchstats = mh.GetMatchStats(mymatch.Id);
        //        foreach (Model.MatchStat matchstat in matchstats)
        //        {
        //            mh.DeleteMatchStat(matchstat);
        //            //DependencyService.Get<IWearData>().SendDeleteMatchStat(matchstat, mymatch);
        //        }
        //        DependencyService.Get<IWearData>().SendDeleteMatch(mymatch);
        //        mh.DeleteMatch(mymatch);
        //        program = mh.GetMatches(false);
        //        matchListBox.ItemsSource = program.ToList();
        //        ActivateNextMatch();
        //        SetupMatchListBox();
        //    }
        //}

        private async void EditMatchClick()
        {
            if (pagescroll.ScrollX < 100)
                mymatch = (Match)matchListBox.SelectedItem;
            else
                mymatch = (Match)listBoxobj.SelectedItem;
            if (mymatch != null)
                await Navigation.PushAsync(new AddMatch(mymatch));
            else
            { message = await DisplayActionSheet(AppResources.selectMatch, null, null, AppResources.ok); }
            //message = await DependencyService.Get<IActionSheet>().UseActionSheet(this, AppResources.selectMatch, "OK", "", ""); }
            //App.ShowMessage(AppResources.selectMatch);
        }

        private async void AddMatchClick()
        {
            await Navigation.PushAsync(new AddMatch());
            //message = await DependencyService.Get<IActionSheet>().UseActionSheet(this, AppResources.NoTeamAppointed, "OK", "", "");
        }

        private async void PlayMatchClick()
        {
            if (pagescroll.ScrollX < 100)
            {
                mymatch = (Match)matchListBox.SelectedItem;

                if (match == null)
                    match = activematch;
                else
                {
                    if (activematch == null)
                        ActivateMatch();
                    else
                    {
                        if (match.Id != activematch.Id)
                        {
                            //if (await App.ShowQuestion(AppResources.selectactive, AppResources.active, AppResources.selected))
                            if (await DisplayActionSheet(AppResources.selectactive, null, null, AppResources.active, AppResources.selected) == AppResources.active)
                                //if (await DependencyService.Get<IActionSheet>().UseActionSheet(this, AppResources.selectactive, AppResources.active, AppResources.selected, "") == AppResources.active)
                                match = activematch;
                            else
                            {
                                //deactivate other matches
                                ObservableCollection<Match> activeMatches = mh.GetActiveMatches();
                                foreach (Match activeMatch in activeMatches)
                                {
                                    mh.InactivateMatch(activeMatch);
                                }
                                //activate match
                                match.Active = true;
                                mh.UpdateMatch(match);
                            }
                        }
                    }
                }
                if (match != null)
                {
                    ActivateActiveMatch();
                    await Navigation.PushAsync(new TimerPage());

                }
            }
            else
            {
                mymatch = (Match)listBoxobj.SelectedItem;
                if (mymatch != null)
                {
                    //string parameter = mymatch.Id.ToString() + ";" + "MyMatches";
                    await Navigation.PushAsync(new MatchStats(mymatch.Id));
                }
                else
                    message = await DisplayActionSheet(AppResources.selectMatch, null, null, AppResources.ok);
                //message = await DependencyService.Get<IActionSheet>().UseActionSheet(this, AppResources.selectMatch, "OK", "", "");
            }
        }

        private async void SettingsClick()
        {
            await Navigation.PushAsync(new SettingsPage());
        }
        #endregion clickactons

        #region pageSetup
        private void SetupMainPage()
        {
            if (!changed)
            {
                changed = true;

                AddMainGrid();
                AddHeaderStack();
                //AddVibration();
                //AddSwitches();
                //AddScreentime();
                AddTapGestureRecognizers();
            }
        }

        private void AddMainGrid()
        {
            LayoutRoot = new Grid() { BackgroundColor = Color.Black };
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(15) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            
            LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;

            Content = LayoutRoot;
        }

        private void AddHeaderStack()
        {
            HeaderStack = new StackLayout() { Orientation = StackOrientation.Horizontal };

            Add = new Image
            {
                Source = ImageSource.FromFile("Add.png"),
            };
            HeaderStack.Children.Add(Add);

            EditImage = new Image
            {
                Source = ImageSource.FromFile("Edit.png"),
            };
            HeaderStack.Children.Add(EditImage);

            PlayMatch = new Image
            {
                Source = ImageSource.FromFile("Timer.png"),
            };
            HeaderStack.Children.Add(PlayMatch);

            Setting = new Image
            {
                Source = ImageSource.FromFile("Settings.png"),
            };
            HeaderStack.Children.Add(Setting);

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
        #endregion
    }
}
