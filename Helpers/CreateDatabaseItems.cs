using KorfballTimerTizen.Model;
using KorfballTimerTizen.res;
//using KorfballTimerTizen.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class CreateDatabaseItems
    {

        ObservableCollection<TeamType> teams = new ObservableCollection<TeamType>();
        TeamTypeHelper tth = new TeamTypeHelper();
        SettingHelper sh = new SettingHelper();
        TeamTypeNames team = new TeamTypeNames();
        MatchHelper mh = new MatchHelper();
        CountryHelper cth = new CountryHelper();
        TimerSettings setting;
        IEnumerable<TimerSettings> settinglist;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tasky.DL.Taskdbstart"/> Taskdbstart. 
        /// if the dbstart doesn't exist, it will create the dbstart and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        /// 
        public CreateDatabaseItems()
        {
            CreateTables();
        }

        public void CreateDb()
        {
            try
            {
                settinglist = sh.GetItems();
                if (settinglist.Count() == 0)
                {
                    //DropTables();
                    CreateTables();

                    FillItems();
                }
                else
                {
                    AdjustDb();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "no such table: Settings")
                {
                    CreateTables();
                    FillItems();
                }
            }

        }

        public void AdjustDb()
        {
            using (var dbstart = SQLiteHelper.GetConnection())
            {
                teams = tth.GetTeamTypesByTime(TimeSpan.Parse("00:35:00"));
                foreach (TeamType team in teams)
                {
                    team.PlayTime = TimeSpan.Parse("00:30:00");
                    tth.UpdateTeamType(team);
                }
                //setting = sh.GetSetting();
                //try
                //{
                //    sh.UpdateSetting(setting);
                //}
                //catch (Exception ex)
                //{
                //    if (ex.Message == "no such column: ScreenWidth")
                //    {
                //        dbstart.DropTable<Settings>();
                //        dbstart.CreateTable<Settings>();
                //        Settings newsetting = new Settings();
                //        newsetting = setting;
                //        sh.InsertSetting(newsetting);
                //    }
                //}
                //if (!sh.TableExists("Clubs"))
                //    dbstart.CreateTable<Clubs>();

                //dbstart.CreateTable<PlayerChange>();
                //dbstart.Execute("ALTER TABLE MatchStat ADD COLUMN PlayerName String");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN VerticalHome Boolean");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN HorizontalGuest Boolean");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN VerticalGuest Boolean");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN TwoColorsHome Boolean");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN TwoColorsGuest Boolean");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN HomeColor1 String");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN GuestColor1 String");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN SubNumber Boolean");
                //dbstart.Execute("ALTER TABLE Match ADD COLUMN CardName Boolean");
                //dbstart.Execute("ALTER TABLE Settings DROP COLUMN Round");
                //        teamtypes = tth.GetTeamTypes();

                //        foreach(TeamType tt in teamtypes)
                //        {
                //            if(tt.Description == "Pupillen E" || tt.Description == "Pupillen F")
                //                tt.Strafworp = true;
                //            else
                //                tt.Strafworp = false;
                //            tth.UpdateTeamType(tt);
                //        }
                //    }
                //}
            }
            //AdjustClubs();
            //AdjustMatches();
            //AdjustMatchStats();

            setting = sh.GetSetting();
            if (setting.CountryHyph != null)
            {
                if (setting.CountryHyph != CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString() && setting.CountryId != 0)
                    ResetTablesToNewLanguage();
            }
        }

        //private void AdjustMatchStats()
        //{
        //    using (var dbstart = SQLiteHelper.GetConnection())
        //    {
        //        dbstart.CreateTable<MatchStat>();
        //        MatchStat matchstat = mh.GetAMatchStat();
        //        try
        //        {
        //            matchstat.Sync = matchstat.Sync;
        //            mh.UpdateMatchStat(matchstat);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.Message == "no such column: Sync")
        //            {
        //                dbstart.Execute("ALTER TABLE MatchStat ADD COLUMN Sync Boolean");
        //                matchstat.Sync = false;
        //                mh.UpdateMatchStat(matchstat);
        //            }
        //        }
        //    }
        //}

        //private void AdjustMatches()
        //{
        //    using (var dbstart = SQLiteHelper.GetConnection())
        //    {
        //        Match match = mh.GetMatch(true);
        //        if(match == null)
        //            match = mh.GetMatch(false);
        //        try
        //        {
        //            match.TwoBandsHome = match.TwoBandsHome;
        //            mh.UpdateMatch(match);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.Message == "no such column: TwoBandsHome" || ex.Message == "no such column: TwoBandsGuest")
        //            {
        //                dbstart.Execute("ALTER TABLE Match ADD COLUMN TwoBandsHome Boolean");
        //                dbstart.Execute("ALTER TABLE Match ADD COLUMN TwoBandsGuest Boolean");
        //                match.TwoBandsHome = false;
        //                match.TwoBandsGuest = false;
        //                mh.UpdateMatch(match);
        //            }
        //        }
        //    }
        //}

        //private void AdjustClubs()
        //{
        //    using (var dbstart = SQLiteHelper.GetConnection())
        //    {
        //        Clubs club = mh.GetClub();
        //        try
        //        {
        //            club.TwoBandsClub = club.TwoBandsClub;
        //            mh.UpdateClub(club);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.Message == "no such column: TwoBandsClub")
        //            {
        //                dbstart.Execute("ALTER TABLE Clubs ADD COLUMN TwoBandsClub Boolean");
        //                club.TwoBandsClub = false;
        //                mh.UpdateClub(club);
        //            }

        //        }
        //    }

        //}

        public void DropTables()
        {
            using (SQLiteConnection dbstart = SQLiteHelper.GetConnection())
            {
                dbstart.DropTable<TeamType>();
                dbstart.DropTable<TeamTypeNames>();
                dbstart.DropTable<Ball>();
                dbstart.DropTable<Pole>();
                dbstart.DropTable<TimerSettings>();
                dbstart.DropTable<RefSettings>();
                dbstart.DropTable<Numbers>();
                dbstart.DropTable<FieldType>();
                dbstart.DropTable<Match>();
                dbstart.DropTable<ActiveMatch>();
                dbstart.DropTable<MatchStat>();
                dbstart.DropTable<IKFCountries>();
                dbstart.DropTable<PlayerChange>();
                dbstart.DropTable<Clubs>();
            }
        }

        public void CreateTables()
        {
            using (var dbstart = SQLiteHelper.GetConnection())
            {
                dbstart.CreateTable<TeamType>();
                dbstart.CreateTable<TeamTypeNames>();
                dbstart.CreateTable<Ball>();
                dbstart.CreateTable<Pole>();
                dbstart.CreateTable<TimerSettings>();
                dbstart.CreateTable<RefSettings>();
                dbstart.CreateTable<Numbers>();
                dbstart.CreateTable<FieldType>();
                dbstart.CreateTable<Match>();
                dbstart.CreateTable<ActiveMatch>();
                dbstart.CreateTable<MatchStat>();
                dbstart.CreateTable<IKFCountries>();
                dbstart.CreateTable<PlayerChange>();
                dbstart.CreateTable<Clubs>();
            }
        }

        //public void SetWear()
        //{
        //    using (var dbstart = SQLiteHelper.GetConnection())
        //    {
        //        dbstart.Execute("ALTER TABLE Settings ADD COLUMN Round Boolean");
        //    }
        //}

        private void FillItems()
        {
            using (var dbstart = SQLiteHelper.GetConnection())
            {
                InsertSettings();
                InsertIKFCountries();
                
            }
        }

        private void InsertSettings()
        {
            sh.InsertSetting(new TimerSettings(false, true, false, 0, 200, true, TimeSpan.Parse("00:03:00")));
            sh.InsertRefSetting(new RefSettings(true, true, true, true, false, false, false, false));
        }

        private void InsertIKFCountries()
        {
            cth.InsertCountry(new IKFCountries("Nederland", "nl"));
            cth.InsertCountry(new IKFCountries("Aruba", "nl"));
            cth.InsertCountry(new IKFCountries("België", "nl"));
            cth.InsertCountry(new IKFCountries("Argentina", "es"));
            cth.InsertCountry(new IKFCountries("Colombia", "es"));
            cth.InsertCountry(new IKFCountries("Costa Rica", "es"));
            cth.InsertCountry(new IKFCountries("Dominican Republic", "es"));
            cth.InsertCountry(new IKFCountries("Spain", "es"));
            cth.InsertCountry(new IKFCountries("Armenia", "hy"));
            cth.InsertCountry(new IKFCountries("Australia", "en"));
            cth.InsertCountry(new IKFCountries("Aruba", "en"));
            cth.InsertCountry(new IKFCountries("Botswana", "en"));
            cth.InsertCountry(new IKFCountries("Canada", "en"));
            cth.InsertCountry(new IKFCountries("Curacao", "en"));
            cth.InsertCountry(new IKFCountries("Cyprus", "en"));
            cth.InsertCountry(new IKFCountries("England", "en"));
            cth.InsertCountry(new IKFCountries("Ghana", "en"));
            cth.InsertCountry(new IKFCountries("Ireland", "en"));
            cth.InsertCountry(new IKFCountries("Ivory Coast", "en"));
            cth.InsertCountry(new IKFCountries("Malaysia", "en"));
            cth.InsertCountry(new IKFCountries("Malawi", "en"));
            cth.InsertCountry(new IKFCountries("New Zealand", "en"));
            cth.InsertCountry(new IKFCountries("Pakistan", "en"));
            cth.InsertCountry(new IKFCountries("Philippines", "en"));
            cth.InsertCountry(new IKFCountries("Scotland", "en"));
            cth.InsertCountry(new IKFCountries("Singapore", "en"));
            cth.InsertCountry(new IKFCountries("South Africa", "en"));
            cth.InsertCountry(new IKFCountries("USA", "en"));
            cth.InsertCountry(new IKFCountries("Wales", "en"));
            cth.InsertCountry(new IKFCountries("Wales", "cy"));
            cth.InsertCountry(new IKFCountries("Zambia", "en"));
            cth.InsertCountry(new IKFCountries("Zimbabwe", "en"));
            cth.InsertCountry(new IKFCountries("Česká republika", "cs"));
            cth.InsertCountry(new IKFCountries("Belgique", "fr"));
            cth.InsertCountry(new IKFCountries("Cameroon", "fr"));
            cth.InsertCountry(new IKFCountries("Canada", "fr"));
            cth.InsertCountry(new IKFCountries("France", "fr"));
            cth.InsertCountry(new IKFCountries("Luxembourg", "fr"));
            cth.InsertCountry(new IKFCountries("Morocco", "fr"));
            cth.InsertCountry(new IKFCountries("Switzerland", "fr"));
            cth.InsertCountry(new IKFCountries("Germany", "de"));
            cth.InsertCountry(new IKFCountries("Luxembourg", "de"));
            cth.InsertCountry(new IKFCountries("Switzerland", "de"));
            cth.InsertCountry(new IKFCountries("Brazil", "pt"));
            cth.InsertCountry(new IKFCountries("Portugal", "pt"));
            cth.InsertCountry(new IKFCountries("China (PRC)", "zh"));
            cth.InsertCountry(new IKFCountries("Hong Kong", "zh"));
            cth.InsertCountry(new IKFCountries("Taiwan (ROC)", "zh"));
            cth.InsertCountry(new IKFCountries("Bosnia and Herzegovina", "bs"));
            cth.InsertCountry(new IKFCountries("Belarus", "be"));
            cth.InsertCountry(new IKFCountries("Bulgaria", "bg"));
            cth.InsertCountry(new IKFCountries("Finland", "fi"));
            cth.InsertCountry(new IKFCountries("Georgia", "ka"));
            cth.InsertCountry(new IKFCountries("Greece", "el"));
            cth.InsertCountry(new IKFCountries("Hungaria", "hu"));
            cth.InsertCountry(new IKFCountries("India", "hi"));
            cth.InsertCountry(new IKFCountries("Indonesia", "id"));
            cth.InsertCountry(new IKFCountries("Croatia", "hr"));
            cth.InsertCountry(new IKFCountries("Ireland", "ga"));
            cth.InsertCountry(new IKFCountries("Italy", "it"));
            cth.InsertCountry(new IKFCountries("Japan", "ja"));
            cth.InsertCountry(new IKFCountries("Macau", "ms"));
            cth.InsertCountry(new IKFCountries("Mongolia", "mn"));
            cth.InsertCountry(new IKFCountries("Nepal", "ne"));
            cth.InsertCountry(new IKFCountries("Poland", "pl"));
            cth.InsertCountry(new IKFCountries("Romania", "ro"));
            cth.InsertCountry(new IKFCountries("Russia", "ru"));
            cth.InsertCountry(new IKFCountries("Scotland", "gd"));
            cth.InsertCountry(new IKFCountries("Serbia", "sr"));
            cth.InsertCountry(new IKFCountries("Slovakia", "sk"));
            cth.InsertCountry(new IKFCountries("South Korea", "ko"));
            cth.InsertCountry(new IKFCountries("Spain", "ca"));
            cth.InsertCountry(new IKFCountries("Sweden", "sv"));
            cth.InsertCountry(new IKFCountries("Turkey", "tr"));
            cth.InsertCountry(new IKFCountries("Ukrain", "uk"));
        }

        private void ResetTablesToNewLanguage()
        {
            using (SQLiteConnection dbstart = SQLiteHelper.GetConnection())
            {
                setting.CountryHyph = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString();
                sh.UpdateSetting(setting);
                Match match = mh.GetMyMatch(1);
                if (match != null)
                {
                    match.Home = AppResources.Thuis;
                    match.Guest = AppResources.Uit;
                    mh.UpdateMatch(match);
                }
            }
        }

    }
}


