using KorfballTimerTizen.Helpers;
using KorfballTimerTizen.Model;
using KorfballTimerTizen.res;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;

namespace KorfballTimerTizen.Views
{
    public class SetUpCountryPage : CirclePage
    {
        private Grid LayoutRoot;
        private StackLayout HeaderStack;
        private StackLayout HeaderSubStack;
        private Image Logo;
        private Label NameTxt;
        private Label PageTxt;
        private Picker Countrypicker;
        private StackLayout CountryStack;
        private Switch countrie;
        private Label SelectName;
        private bool changed;
        #region declarations
        SQLiteConnection dbConn = SQLiteHelper.GetConnection();
        private MatchHelper mh = new MatchHelper();
        private SettingHelper sh = new SettingHelper();
        private CountryHelper ch = new CountryHelper();
        private TeamTypeNameHelper ttnh = new TeamTypeNameHelper();
        private FieldTypeHelper fsh = new FieldTypeHelper();
        private ObservableCollection<IKFCountries> countries = new ObservableCollection<IKFCountries>();
        private TimerSettings setting;
        private IKFCountries country;
        private TeamTypeNames team;
        public int id;
        public int mnd = DateTime.Now.Month;
        string name;
        string description;
        string msgtxt;
        #endregion

        public SetUpCountryPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            setting = sh.GetSetting();
            SetupMainPage();
        }

        #region voids
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //LayoutRoot.BackgroundColor = Color.FromRgba(0, 0, 0, 255);
            setting = sh.GetSetting();
            NameTxt.Text = AppResources.NameTxt;
            PageTxt.Text = AppResources.selectcountry;
            if (countrie.IsToggled)
                SelectName.Text = AppResources.AllCountries;
            else
                SelectName.Text = AppResources.Selectcountries;
            name = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString();
            countries = ch.CountryListOnHyph(name);
            if (countries.Count() == 0)
                countries = ch.CountryList();
            Countrypicker.Items.Clear();
            Countrypicker.Items.Add(AppResources.selectcountry);
            foreach (IKFCountries country in countries)
            {
                if (country.Description != "" || country.Description != null)
                    Countrypicker.Items.Add(country.Description);
            }
            Countrypicker.SelectedIndex = 0;
            //var itemList = dbConn.Table<IKFCountries>();
            //List<string> dbList = new List<string>();
            //dbList.Add(AppResources.NameTxt);
            //foreach (var item in countries)
            //{
            //    dbList.Add("Row " + item.Id + ": " + item.Description);
            //}
            //Countrypicker.ItemsSource = dbList;
        }


        public async void FillStandardTeams()
        {
            await Task.Delay(200);
            msgtxt = await DisplayActionSheet(AppResources.NoTeamAppointed, null, null, AppResources.ok);
            //msgtxt = await DependencyService.Get<IActionSheet>().UseActionSheet(this, AppResources.NoTeamAppointed, AppResources.ok, "");
            dbConn.Insert(new TeamTypeNames(AppResources.seniors));
            dbConn.Insert(new TeamTypeNames(AppResources.Beachkorfbal));
            dbConn.Insert(new TeamTypeNames(AppResources.Beslissingswedstrijd));
            dbConn.Insert(new TeamTypeNames(AppResources.Practice));

            team = ttnh.GetTeamTypeName(AppResources.seniors);
            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, true, TimeSpan.Parse("00:00:00"), false));

            team = ttnh.GetTeamTypeName(AppResources.Beachkorfbal);
            dbConn.Insert(new TeamType(team.Description, 5, 3.2f, 18, 9, TimeSpan.Parse("00:06:00"), 2, false, TimeSpan.Parse("00:00:00"), false));

            team = ttnh.GetTeamTypeName(AppResources.Beslissingswedstrijd);
            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, false, TimeSpan.Parse("00:10:00"), false));
            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, true, TimeSpan.Parse("00:10:00"), false));

            team = ttnh.GetTeamTypeName(AppResources.Practice);
            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:15:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:15:00"), 2, true, TimeSpan.Parse("00:00:00"), false));
        }

        public void FillCountryTeams()
        {

            string name = CultureInfo.CurrentCulture.Name.ToString();
            setting = sh.GetSetting();
            if (setting.CountryId != 0)
            {
                country = ch.GetCountry(setting.CountryId);
                switch (country.Description)
                {
                    case "Nederland":
                        {
                            dbConn.Insert(new TeamTypeNames("Senioren"));
                            dbConn.Insert(new TeamTypeNames("Midweek"));
                            dbConn.Insert(new TeamTypeNames("Junioren"));
                            dbConn.Insert(new TeamTypeNames("Aspiranten B"));
                            dbConn.Insert(new TeamTypeNames("Aspiranten B 4-tal"));
                            dbConn.Insert(new TeamTypeNames("Aspiranten C"));
                            dbConn.Insert(new TeamTypeNames("Aspiranten C 4-tal"));
                            dbConn.Insert(new TeamTypeNames("Pupillen D Hoofdklasse"));
                            dbConn.Insert(new TeamTypeNames("Pupillen D 8-tal"));
                            dbConn.Insert(new TeamTypeNames("Pupillen D 4-tal"));
                            dbConn.Insert(new TeamTypeNames("Pupillen E"));
                            dbConn.Insert(new TeamTypeNames("Pupillen F"));
                            dbConn.Insert(new TeamTypeNames("G-korfbal"));
                            dbConn.Insert(new TeamTypeNames("Beachkorfbal"));
                            dbConn.Insert(new TeamTypeNames("Beslissings wedstrijd"));
                            dbConn.Insert(new TeamTypeNames("Oefenwedstrijd"));

                            team = ttnh.GetTeamTypeName("Senioren");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:35:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, true, TimeSpan.Parse("00:00:00"), false));
                            team = ttnh.GetTeamTypeName("Midweek");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:35:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, true, TimeSpan.Parse("00:00:00"), false));
                            team = ttnh.GetTeamTypeName("G-korfbal");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("Junioren");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:35:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, true, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("Aspiranten B");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:25:00"), 2, true, TimeSpan.Parse("00:00:00"), false));
                            team = ttnh.GetTeamTypeName("Aspiranten B 4-tal");
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("Aspiranten C");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:25:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:25:00"), 2, true, TimeSpan.Parse("00:00:00"), false));
                            team = ttnh.GetTeamTypeName("Aspiranten C 4-tal");
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("Pupillen D Hoofdklasse");
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 40, 20, TimeSpan.Parse("00:25:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 40, 20, TimeSpan.Parse("00:25:00"), 2, true, TimeSpan.Parse("00:00:00"), false));
                            team = ttnh.GetTeamTypeName("Pupillen D 8-tal");
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 40, 20, TimeSpan.Parse("00:12:30"), 4, true, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 40, 20, TimeSpan.Parse("00:12:30"), 4, false, TimeSpan.Parse("00:00:00"), false));
                            team = ttnh.GetTeamTypeName("Pupillen D 4-tal");
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("Pupillen E");
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), true));
                            dbConn.Insert(new TeamType(team.Description, 4, 3.0f, 24, 12, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), true));
                            team = ttnh.GetTeamTypeName("Pupillen F");
                            dbConn.Insert(new TeamType(team.Description, 3, 2.5f, 24, 12, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), true));
                            dbConn.Insert(new TeamType(team.Description, 3, 2.5f, 24, 12, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), true));

                            team = ttnh.GetTeamTypeName("Beachkorfbal");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.2f, 18, 9, TimeSpan.Parse("00:06:00"), 2, false, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("Beslissings wedstrijd");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, false, TimeSpan.Parse("00:10:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, true, TimeSpan.Parse("00:10:00"), false));

                            team = ttnh.GetTeamTypeName("Oefenwedstrijd");
                            dbConn.Insert(new TeamType(team.Description, 3, 2.5f, 24, 12, TimeSpan.Parse("00:15:00"), 2, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 3, 2.5f, 24, 12, TimeSpan.Parse("00:15:00"), 2, true, TimeSpan.Parse("00:00:00"), false));
                            break;
                        }
                    case "Česká republika":
                        {
                            dbConn.Insert(new TeamTypeNames("Senioři"));
                            dbConn.Insert(new TeamTypeNames("plážový korfbal"));
                            dbConn.Insert(new TeamTypeNames("rozhodující zápas"));
                            dbConn.Insert(new TeamTypeNames("praxe zápas"));

                            team = ttnh.GetTeamTypeName("Senioři");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), false));

                            //team = ttnh.GetTeamTypeName("shot-clock");
                            //dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:25:00"), 2, true, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("plážový korfbal");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.2f, 18, 9, TimeSpan.Parse("00:06:00"), 2, false, TimeSpan.Parse("00:00:00"), false));

                            team = ttnh.GetTeamTypeName("rozhodující zápas");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:10:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:10:00"), false));

                            team = ttnh.GetTeamTypeName("praxe zápas");
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, false, TimeSpan.Parse("00:00:00"), false));
                            dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:10:00"), 4, true, TimeSpan.Parse("00:00:00"), false));
                            break;
                        }
                    case "Aruba":
                    case "België":
                    case "Argentina":
                    case "Colombia":
                    case "Costa Rica":
                    case "Dominican Republic":
                    case "Spain":
                    case "Armenia":
                    case "Australia":
                    case "Botswana":
                    case "Curacao":
                    case "Cyprus":
                    case "England":
                    case "Ghana":
                    case "Ireland":
                    case "Ivory Coast":
                    case "Malaysia":
                    case "Malawi":
                    case "New Zealand":
                    case "Pakistan":
                    case "Philippines":
                    case "Scotland":
                    case "Singapore":
                    case "South Africa":
                    case "USA":
                    case "Wales":
                    case "Zambia":
                    case "Zimbabwe":
                    case "Belgique":
                    case "Cameroon":
                    case "Canada":
                    case "France":
                    case "Luxembourg":
                    case "Morocco":
                    case "Switzerland":
                    case "Germany":
                    case "Brazil":
                    case "Portugal":
                    case "China (PRC)":
                    case "Hong Kong":
                    case "Taiwan (ROC)":
                    case "Bosnia and Herzegovina":
                    case "Belarus":
                    case "Bulgaria":
                    case "Finland":
                    case "Georgia":
                    case "Greece":
                    case "Hungaria":
                    case "India":
                    case "Indonesia":
                    case "Croatia":
                    case "Italy":
                    case "Japan":
                    case "Macau":
                    case "Mongolia":
                    case "Nepal":
                    case "Poland":
                    case "Romania":
                    case "Russia":
                    case "Serbia":
                    case "Slovakia":
                    case "South Korea":
                    case "Sweden":
                    case "Turkey":
                    case "Ukrain":
                    default:
                        {
                            FillStandardTeams();
                            //dbConn.Insert(new TeamTypeNames("Midweek"));
                            //team = ttnh.GetTeamTypeName("Midweek");
                            //dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:35:00"), 2, false, false, TimeSpan.Parse("00:00:00")));
                            //dbConn.Insert(new TeamType(team.Description, 5, 3.5f, 40, 20, TimeSpan.Parse("00:30:00"), 2, true, false, TimeSpan.Parse("00:00:00")));
                            break;
                        }

                }
            }

        }

        private void FillItems()
        {
            using (var dbstart = SQLiteHelper.GetConnection())
            {
                mh.InsertActiveMatch(new ActiveMatch(-1));
                fsh.InsertFieldType(new FieldType(false));
                fsh.InsertFieldType(new FieldType(true));
                mh.InsertMatch(new Match(DateTime.Now, TimeSpan.Parse("12:30:00"), AppResources.Thuis, AppResources.Uit, 0, 0, 2, 1, false, false, 16));
                FillCountryTeams();
            }
        }

        #endregion

        private void SetupMainPage()
        {
            if (!changed)
            {
                changed = true;
                
                AddMainGrid();

                AddHeaderStack();
                AddCountryPicker();
                AddCountryStack();
                //AddTapGestureRecognizers();
            }
        }

        private void AddMainGrid()
        {
            LayoutRoot = new Grid() { BackgroundColor = Color.Maroon };
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1,GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
            Content = LayoutRoot;
        }

        //private void LayoutRoot_SizeChanged(object sender, EventArgs e)
        //{
            
        //}

        private void AddHeaderStack()
        {
            HeaderStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
            };
            if (setting.Round)
                HeaderStack.Margin = new Thickness(setting.ScreenWidth / 5, setting.ScreenWidth / 5, 0, 0);

            Logo = new Image
            {
                Source = ImageSource.FromFile("KorfbalWit.png"),
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                //Margin=new Thickness(50,0,0,0)
            };
            HeaderStack.Children.Add(Logo);
            HeaderSubStack = new StackLayout() { Orientation = StackOrientation.Vertical };
            HeaderStack.Children.Add(HeaderSubStack);

            NameTxt = new Label
            {
                TextColor = Color.White,
                FontSize = 8,
                HorizontalOptions=LayoutOptions.Center
            };
            HeaderSubStack.Children.Add(NameTxt);
            PageTxt = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                HorizontalOptions = LayoutOptions.Center
            };
            HeaderSubStack.Children.Add(PageTxt);
            LayoutRoot.Children.Add(HeaderStack);
        }

        private void AddCountryPicker()
        {
            Countrypicker = new Picker
            {
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.DarkBlue,
            };
            LayoutRoot.Children.Add(Countrypicker);
            if (setting.Round)
                Countrypicker.WidthRequest = setting.ScreenWidth/2;
            Countrypicker.SelectedIndexChanged += Countrypicker_SelectedIndexChanged;
            Grid.SetRow(Countrypicker, 2);
        }

        private async void Countrypicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Countrypicker.SelectedIndex != -1)
            {
                description = Countrypicker.Items.ElementAt(Countrypicker.SelectedIndex).ToString();
                country = ch.GetCountryByName(description);
                if (country != null && country.Id != 0)
                {
                    //if (await App.ShowQuestion(AppResources.selectedcountry, AppResources.yes, AppResources.no))
                    if (await DisplayActionSheet(description + ": " + AppResources.selectedcountry, null, null, AppResources.yes, AppResources.no) == AppResources.yes)
                    //if (await DependencyService.Get<IActionSheet>().UseActionSheet(this, AppResources.selectedcountry + " " + description, AppResources.yes, AppResources.no, "") == AppResources.yes)
                    {
                        setting = sh.GetSetting();
                        setting.CountryId = country.Id;
                        setting.CountryHyph = country.Hyph;
                        sh.UpdateSetting(setting);
                        FillItems();
                        await Navigation.PushAsync(new SettingsPage());
                    }
                    else
                        Countrypicker.SelectedIndex = 0;
                }

            }
        }

        private void AddCountryStack()
        {
            CountryStack = new StackLayout() { Orientation = StackOrientation.Horizontal };
            countrie = new Switch
            {
                BackgroundColor = Color.Blue,
                Scale = .75,
                VerticalOptions = LayoutOptions.Center,
            };
            countrie.Toggled += Country_Toggled;
            CountryStack.Children.Add(countrie);
            SelectName = new Label
            {
                TextColor = Color.White,
                FontSize = 6,
                VerticalOptions = LayoutOptions.Center,
            };
            CountryStack.Children.Add(SelectName);
            LayoutRoot.Children.Add(CountryStack);
            Grid.SetRow(CountryStack, 1);
        }

        private void Country_Toggled(object sender, ToggledEventArgs e)
        {
            if (countrie.IsToggled)
            {
                countries = ch.CountryList();
                SelectName.Text = AppResources.AllCountries;
                CountryHelper cth = new CountryHelper();
                cth.InsertCountry(new IKFCountries("Colombia", "es"));
            }
            else
            {
                countries = ch.CountryListOnHyph(name);
                SelectName.Text = AppResources.Selectcountries;
                if (countries.Count() == 0)
                    countries = ch.CountryList();
            }
            Countrypicker.Items.Clear();
            Countrypicker.Items.Add(AppResources.selectcountry);
            foreach (IKFCountries country in countries)
            {
                if (country.Description != "" && country.Description != null)
                    Countrypicker.Items.Add(country.Description);
            }
            Countrypicker.SelectedIndex = 0;
        }
    }
}