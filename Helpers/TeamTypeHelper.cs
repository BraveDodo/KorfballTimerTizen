using KorfballTimerTizen.Model;
//using KorfballTimerTizen.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class TeamTypeHelper
    {
        public TeamType GetTeamType(int id)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<TeamType>() where x.Id == id select x).FirstOrDefault();
                return tt;
            }
        }


        // Retrieve the specific contact from the database. 
        public TeamType GetFirstTeamType(bool indoors)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<TeamType> myteamtypes = dbConn.Table<TeamType>().ToList<TeamType>();
                TeamType tt = new ObservableCollection<TeamType>(from x in myteamtypes where x.Indoors == indoors orderby x.Id select x).FirstOrDefault();
                return tt;
            }
        }

        // Retrieve the all contact list from the database. 
        public ObservableCollection<TeamType> GetTeamTypes()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<TeamType> myCollection = dbConn.Table<TeamType>().ToList<TeamType>();
                ObservableCollection<TeamType> teamTypeList = new ObservableCollection<TeamType>(myCollection);
                return teamTypeList;
            }
        }

        // Retrieve the all contact list from the database. 
        public ObservableCollection<TeamType> GetTeamTypesByTime(TimeSpan time)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<TeamType> myCollection = dbConn.Table<TeamType>().ToList<TeamType>();
                ObservableCollection<TeamType> teamTypeList = new ObservableCollection<TeamType>(from x in myCollection where x.PlayTime == time select x);
                return teamTypeList;
            }
        }

        public TeamType CheckTeamType(string description, bool indoor)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TeamType teamtype = (from x in dbConn.Table<TeamType>() where x.Description == description && x.Indoors == indoor select x).FirstOrDefault();
                return teamtype;
            }
        }

        public TeamType CheckLastTeamType(string description)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TeamType teamtype = (from x in dbConn.Table<TeamType>() where x.Description == description select x).FirstOrDefault();
                return teamtype;
            }
        }

        // Retrieve the specific teamtype list from the database. 
        public ObservableCollection<TeamType> GetSpecTeamTypes(bool indoors)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<TeamType> myteamtypes = dbConn.Table<TeamType>().ToList<TeamType>();
                ObservableCollection<TeamType> teamTypeList = new ObservableCollection<TeamType>(from x in myteamtypes where x.Indoors == indoors select x);
                return teamTypeList;
            }
        }

        //Update existing teamtype 
        public void UpdateTeamType(TeamType teamtype)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TeamType tt = (from x in dbConn.Table<TeamType>() where x.Id == teamtype.Id select x).FirstOrDefault();
                if (tt != null)
                {
                    TeamType existTeamType = CheckTeamType(teamtype.Description, teamtype.Indoors);
                    if (existTeamType == null || (existTeamType.Id == teamtype.Id && existTeamType.Description == teamtype.Description))
                    {
                        if (teamtype.PlayTime1 < teamtype.PlayTime)
                        {
                            tt.Description = teamtype.Description;
                            tt.Ball = teamtype.Ball;
                            tt.Pole = teamtype.Pole;
                            tt.Indoors = teamtype.Indoors;
                            tt.Periods = teamtype.Periods;
                            tt.PlayTime = teamtype.PlayTime;
                            tt.PlayTime1 = teamtype.PlayTime1;
                            tt.FieldLong = teamtype.FieldLong;
                            tt.FieldWide = teamtype.FieldWide;
                            tt.GoldenGoal = teamtype.GoldenGoal;
                            tt.Strafworp = teamtype.Strafworp;

                            dbConn.RunInTransaction(() =>
                            {
                                dbConn.Update(tt);
                            });
                        }
                    }
                }
            }
        }

        // Insert the new teamtype in the TeamType table. 
        public void InsertTeamType(TeamType newTeamType)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TeamType existTeamType = CheckTeamType(newTeamType.Description, newTeamType.Indoors);
                if (existTeamType == null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Insert(newTeamType);
                    });
                }
            }
        }

        public void DeleteTeamType(TeamType teamtype)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                TeamType existingTeamType = (from x in dbConn.Table<TeamType>() where x.Id == teamtype.Id select x).FirstOrDefault();
                if (existingTeamType != null)
                {
                    ObservableCollection<TeamType> lastteamtypes = GetSpecTeamTypes(existingTeamType.Indoors);
                    if (lastteamtypes.Count > 1)
                    {
                        MatchHelper mh = new MatchHelper();
                        ObservableCollection<Match> myteamMatches = mh.GetTeamTypeMatches(teamtype);
                        if (myteamMatches != null)
                        {
                            if (myteamMatches.FirstOrDefault().Id == 1)
                            {
                                Match specmatch = myteamMatches.FirstOrDefault();
                                specmatch.TeamTypeId = GetFirstTeamType(!existingTeamType.Indoors).Id;
                                mh.UpdateMatch(specmatch);
                                myteamMatches = mh.GetTeamTypeMatches(teamtype);
                            }
                            foreach (Match myteamMatch in myteamMatches)
                            {
                                ObservableCollection<MatchStat> matchstatlist = mh.GetMatchStats(myteamMatch.Id);
                                foreach (MatchStat matchstat in matchstatlist)
                                    mh.DeleteMatchStat(matchstat);

                                mh.DeleteMatch(myteamMatch);
                            }

                            dbConn.RunInTransaction(() =>
                            {
                                dbConn.Delete(existingTeamType);
                            });
                        }
                    }
                }
            }
        }
    }
}