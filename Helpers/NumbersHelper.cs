using KorfballTimerTizen.Model;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class NumbersHelper
    {
        static SQLiteConnection dbConn = SQLiteHelper.GetConnection();

        public ObservableCollection<Numbers> GetNumbers()
        {
            List<Numbers> myCollection = dbConn.Table<Numbers>().ToList<Numbers>();
            ObservableCollection<Numbers> PoleList = new ObservableCollection<Numbers>(myCollection);
            return PoleList;
        }

        public ObservableCollection<Numbers> GetTime()
        {
            List<Numbers> mynumbers = dbConn.Table<Numbers>().ToList<Numbers>();
            ObservableCollection<Numbers> myminutes = new ObservableCollection<Numbers>(from x in mynumbers select x);
            return myminutes;
        }

        public ObservableCollection<Numbers> GetSeconds()
        {
            List<Numbers> myteamtypes = dbConn.Table<Numbers>().ToList<Numbers>();
            ObservableCollection<Numbers> teamTypeList = new ObservableCollection<Numbers>(from x in myteamtypes select x);
            return teamTypeList;
        }

        // Retrieve the all contact list from the database. 
        public ObservableCollection<Numbers> GetNumbersUpTo(byte i)
        {
            List<Numbers> myCollection = dbConn.Table<Numbers>().ToList<Numbers>();
            ObservableCollection<Numbers> PoleList = new ObservableCollection<Numbers>(from x in myCollection where (byte.Parse(x.Number) <= i && x.Number != "00") select x);
            return PoleList;
        }

        // Retrieve the all contact list from the database. 
        public ObservableCollection<Numbers> GetAllNumbersUpTo(byte i)
        {
            List<Numbers> myCollection = dbConn.Table<Numbers>().ToList<Numbers>();
            ObservableCollection<Numbers> PoleList = new ObservableCollection<Numbers>(from x in myCollection where (byte.Parse(x.Number) <= i) select x);
            return PoleList;
        }

    }
}