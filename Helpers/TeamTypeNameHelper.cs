using KorfballTimerTizen.Model;
//using KorfballTimerTizen.Resources;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class TeamTypeNameHelper
    {
        public TeamTypeNames GetTeamTypeName(string team)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var getteam = (from x in dbConn.Table<TeamTypeNames>() where x.Description == team select x).FirstOrDefault();
                return getteam;
            }
        }

        public TeamTypeNames GetTeamTypeNameById(int id)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var getteam = (from x in dbConn.Table<TeamTypeNames>() where x.Id == id select x).FirstOrDefault();
                return getteam;
            }
        }

        public TeamTypeNames GetFirstTeamTypeName()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var getteam = (from x in dbConn.Table<TeamTypeNames>() select x).FirstOrDefault();
                return getteam;
            }
        }

        // Retrieve the all contact list from the database. 
        public ObservableCollection<TeamTypeNames> GetAllTeamTypeNames()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<TeamTypeNames> myCollection = dbConn.Table<TeamTypeNames>().ToList<TeamTypeNames>();
                ObservableCollection<TeamTypeNames> teamList = new ObservableCollection<TeamTypeNames>(myCollection);
                return teamList;
            }
        }

        //Update existing teamtype 
        public void UpdateTeamTypeName(TeamTypeNames team)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TeamTypeNames updateteam = (from x in dbConn.Table<TeamTypeNames>() where x.Id == team.Id select x).FirstOrDefault();
                if (updateteam != null)
                {
                    TeamTypeNames existteam = GetTeamTypeName(team.Description);
                    if (existteam == null)
                    {
                        updateteam.Description = team.Description;

                        dbConn.RunInTransaction(() =>
                        {
                            dbConn.Update(updateteam);
                        });
                    }
                }
            }
        }

        // Insert the new teamtype in the TeamType table. 
        public void InsertTeamTypeName(TeamTypeNames team)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TeamTypeNames existteam = GetTeamTypeName(team.Description);
                if (existteam == null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Insert(team);
                    });
                }
            }
        }

        public void DeleteTeamTypeName(TeamTypeNames team)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                if (GetAllTeamTypeNames().Count > 1)
                {
                    TeamTypeHelper tth = new TeamTypeHelper();
                    TeamType teamtype = tth.CheckLastTeamType(team.Description);
                    if (teamtype == null)
                    {
                        TeamTypeNames existingcontact = (from x in dbConn.Table<TeamTypeNames>() where x.Description == team.Description select x).FirstOrDefault();
                        if (existingcontact != null)
                        {
                            dbConn.RunInTransaction(() =>
                            {
                                dbConn.Delete(existingcontact);
                            });
                        }
                    }
                }
            }
        }
    }
}

