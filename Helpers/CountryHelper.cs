using KorfballTimerTizen.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    class CountryHelper
    {
        public IKFCountries GetCountry(int countryId)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<IKFCountries> mycountries = dbConn.Table<IKFCountries>().ToList();
                IKFCountries mycountry = new ObservableCollection<IKFCountries>(from x in mycountries where x.Id == countryId select x).FirstOrDefault();
                return mycountry;
            }
        }

        public IKFCountries GetCountryByName(string country)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<IKFCountries> mycountries = dbConn.Table<IKFCountries>().ToList();
                IKFCountries mycountry = new ObservableCollection<IKFCountries>(from x in mycountries where x.Description == country select x).FirstOrDefault();
                return mycountry;
            }
        }

        public ObservableCollection<IKFCountries> CountryList()
        {

            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<IKFCountries> mycountries = dbConn.Query<IKFCountries>("SELECT IKFCountries.Description, IKFCountries.Id FROM IKFCountries GROUP BY IKFCountries.Description");
                ObservableCollection<IKFCountries> countryist = new ObservableCollection<IKFCountries>(from x in mycountries orderby x.Description select x);
                return countryist;
            }
        }

        public ObservableCollection<IKFCountries> CountryListOnHyph(string hyph)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {

                List<IKFCountries> mycountries = dbConn.Table<IKFCountries>().ToList();
                ObservableCollection<IKFCountries> countryist = new ObservableCollection<IKFCountries>(from x in mycountries where x.Hyph == hyph orderby x.Description select x);
                return countryist;
            }
        }

        public void InsertCountry(IKFCountries newcontact)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newcontact);
                });
            }
        }

    }
}
