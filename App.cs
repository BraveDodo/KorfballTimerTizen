using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;
using KorfballTimerTizen.Views;
using KorfballTimerTizen.Helpers;
using KorfballTimerTizen.Model;
using Plugin.DeviceInfo;
using SQLitePCL;
using System.IO;
using Plugin.Vibrate;
using System.Threading.Tasks;
using Tizen.System;

namespace KorfballTimerTizen
{
    public class App : Application
    {
        static CreateDatabaseItems db;
        public static bool sync = false;
        public static bool sendback = false;
        public static float scalefactor;
        public static string device;
        public static string device1;
        public static Feedback feedback;

        public App()
        {
            // The root page of your application
            
        }

        public static CreateDatabaseItems Database
        {
            get
            {
                if (db == null)
                {
                    db = new CreateDatabaseItems();
                }
                return db;
            }

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            MatchHelper mh = new MatchHelper();
            CreateDatabaseItems CreateDb = new CreateDatabaseItems();
            SettingHelper sh = new SettingHelper();
            TimerSettings setting = sh.GetSetting();
            CreateDb.CreateDb();
            device = CrossDeviceInfo.Current.Idiom.ToString();
            device1 = CrossDeviceInfo.Current.Model;

            setting = sh.GetSetting();
            try
            {
                sh.UpdateSetting(setting);
                if (setting.CountryId == 0)
                {
                    MainPage = new NavigationPage(new WearTypePage());
                }
                else
                {
                    TeamTypeNameHelper ttnh = new TeamTypeNameHelper();
                    IEnumerable<TeamTypeNames> teamtypenames = ttnh.GetAllTeamTypeNames();
                    if (teamtypenames.Count() == 0)
                        MainPage = new NavigationPage(new SetUpCountryPage());
                    else
                    {
                        ActiveMatch activematch = mh.GetActiveMatch();
                        Match match = mh.GetMyMatch(activematch.MatchId);
                        if (match == null)
                        {
                            activematch.MatchId = -1;
                            mh.UpdateActiveMatch(activematch);
                        }
                        if (activematch.MatchId == -1)
                            MainPage = new NavigationPage(new SettingsPage());
                        else
                            MainPage = new NavigationPage(new TimerPage());
                    }
                }
            }
            catch
            {
                MainPage = new NavigationPage(new WearTypePage());
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void Vibrate()
        {
            SettingHelper sh = new SettingHelper();
            TimerSettings setting = sh.GetSetting();
            feedback = new Feedback();
            //feedback.Play(FeedbackType.Vibration, "Tap");
            //await Task.Delay(setting.Vibrate);
            //feedback.Stop();


        }
    }
}
