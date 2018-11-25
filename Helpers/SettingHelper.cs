using KorfballTimerTizen.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class SettingHelper
    {
        SQLiteConnection database = SQLiteHelper.GetConnection();
        static object locker = new object();

        public bool TableExists(String tableName)
        {
            var cursor = database.GetTableInfo(tableName);
            return cursor.FirstOrDefault() != null;
        }

        public bool TableRowExists(String tableName)
        {
            var cursor = database.GetTableInfo(tableName);
            return cursor.FirstOrDefault() != null;
        }

        // Retrieve the specific setting from the database. 
        public ObservableCollection<TimerSettings> GetSettings()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<TimerSettings> mysettings = dbConn.Table<TimerSettings>().ToList();
                ObservableCollection<TimerSettings> settingsList = new ObservableCollection<TimerSettings>(from x in mysettings select x);
                return settingsList;
            }
        }

        public RefSettings GetRefSetting()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var setting = (from x in dbConn.Table<RefSettings>() select x).FirstOrDefault();
                return setting;
            }
        }

        public IEnumerable<TimerSettings> GetItems()
        {
            lock (locker)
            {
                using (var dbstart = SQLiteHelper.GetConnection())
                { return (from i in dbstart.Table<TimerSettings>() select i).ToList(); }
            }
        }

        // Retrieve the specific setting from the database. 
        public TimerSettings GetSetting()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var setting = (from x in dbConn.Table<TimerSettings>() select x).FirstOrDefault();
                return setting;
            }
        }

        public void InsertSetting(TimerSettings newcontact)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newcontact);
                    dbConn.SaveTransactionPoint();
                });
            }
        }

        //Update existing teamtype 
        public void UpdateSetting(TimerSettings setting)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<TimerSettings>() select x).FirstOrDefault();
                if (tt != null)
                {
                    tt.TimerStart = setting.TimerStart;
                    tt.Indoor = setting.Indoor;
                    //tt.StartOutdoor = setting.StartOutdoor;
                    tt.CanNotGoBack = setting.CanNotGoBack;
                    tt.CountryId = setting.CountryId;
                    tt.CountryHyph = setting.CountryHyph;
                    tt.Vibrate = setting.Vibrate;
                    tt.ScreenOn = setting.ScreenOn;
                    tt.ScreenTime = setting.ScreenTime;
                    tt.Round = setting.Round;
                    tt.ScreenWidth = setting.ScreenWidth;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(tt);
                    });
                }
            }
        }

        public void DeleteSetting(int id)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TimerSettings existingsetting = (from x in dbConn.Table<TimerSettings>() where x.Id == id select x).FirstOrDefault();
                if (existingsetting != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingsetting);
                    });
                }
            }
        }

        public void InsertRefSetting(RefSettings newcontact)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newcontact);
                });
            }
        }

        public void UpdateRefSetting(RefSettings setting)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<RefSettings>() select x).FirstOrDefault();
                if (tt != null)
                {
                    tt.StopAtChange = setting.StopAtChange;
                    tt.MatchStats = setting.MatchStats;
                    tt.SaveMatch = setting.SaveMatch;
                    tt.Audio1Min = setting.Audio1Min;
                    tt.AudioEnd = setting.AudioEnd;
                    tt.TimeOut = setting.TimeOut;
                    tt.CardName = setting.CardName;
                    tt.SubNumber = setting.SubNumber;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(tt);
                    });
                }
            }
        }

    }
}