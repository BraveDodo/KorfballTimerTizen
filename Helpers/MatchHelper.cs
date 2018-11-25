using KorfballTimerTizen.Model;
using KorfballTimerTizen.res;
//using KTW.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class MatchHelper
    {
        public Match GetMyMatch(int id)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<Match>() where x.Id == id select x).FirstOrDefault();
                return tt;
            }
        }

        public Match GetMatch(bool active)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<Match>() where x.Active == active select x).FirstOrDefault();
                return tt;
            }
        }


        public ActiveMatch GetActiveMatch()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<ActiveMatch>() select x).FirstOrDefault();
                return tt;
            }
        }

        // Retrieve the specific contact from the database. 
        public Match GetNextMatch()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<Match>() where x.MatchDate >= DateTimeOffset.Now && x.Done == false orderby x.MatchDate select x).FirstOrDefault();
                return tt;
            }
        }

        public Match GetSpecificMatch(string home, DateTime date, TimeSpan matchtime)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<Match>().ToList() where x.Home == home && x.MatchDate.Year == date.Year && x.MatchDate.Month == date.Month && x.MatchDate.Day == date.Day && x.MatchTime.Hours == matchtime.Hours && x.MatchTime.Minutes == matchtime.Minutes select x).FirstOrDefault();
                return tt;
            }
        }

        // Retrieve the specific contact from the database. 
        public ObservableCollection<Match> GetActiveMatches()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<Match> allmatches = dbConn.Table<Match>().ToList();
                ObservableCollection<Match> activematches = new ObservableCollection<Match>(from x in allmatches where x.Active == true select x);
                return activematches;
            }
        }
        //TODO delete matchtime
        // Retrieve the specific contact from the database. 
        public ObservableCollection<Match> GetMatches(bool done)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<Match> allmatches = dbConn.Table<Match>().ToList();
                ObservableCollection<Match> playedmatches;
                if (done)
                    playedmatches = new ObservableCollection<Match>(from x in allmatches where x.Done == done orderby x.MatchDate descending select x);
                else
                    playedmatches = new ObservableCollection<Match>(from x in allmatches where x.Done == done orderby x.MatchDate select x);
                return playedmatches;
            }
        }

        // Retrieve the specific contact from the database. 
        public ObservableCollection<Match> GetRefMatches(bool done, string myteam)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<Match> allmatches = dbConn.Table<Match>().ToList();
                ObservableCollection<Match> playedmatches = new ObservableCollection<Match>(from x in allmatches where x.Done == done && (x.Home != myteam && x.Guest != myteam) orderby x.MatchDate select x);
                return playedmatches;
            }
        }

        // Retrieve the specific contact from the database. 
        public ObservableCollection<Match> GetMyTeamMatches(string myteam)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<Match> allmatches = dbConn.Table<Match>().ToList();
                ObservableCollection<Match> myteammatches = new ObservableCollection<Match>(from x in allmatches where x.Home == myteam || x.Guest == myteam select x);
                return myteammatches;
            }
        }

        // Retrieve the specific contact from the database. 
        public ObservableCollection<Match> GetTeamTypeMatches(TeamType myteam)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<Match> allmatches = dbConn.Table<Match>().ToList();
                ObservableCollection<Match> teamtypematches = new ObservableCollection<Match>(from x in allmatches where x.TeamTypeId == myteam.Id orderby x.Id select x);
                return teamtypematches;
            }
        }

        public Clubs GetClub(string myclub)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var club = (from x in dbConn.Table<Clubs>() where x.Description.ToLower() == myclub.ToLower() select x).FirstOrDefault();
                return club;
            }
        }

        public Clubs GetClub()
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var club = (from x in dbConn.Table<Clubs>() select x).FirstOrDefault();
                return club;
            }
        }

        // Retrieve the specific contact from the database. 
        public MatchStat GetMatchStat(int id)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<MatchStat>() where x.Id == id select x).FirstOrDefault();
                return tt;
            }
        }

        public MatchStat GetMatchStat(int home, int guest)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<MatchStat>() where x.HomeScore == home && x.GuestScore == guest orderby x.Id descending select x).FirstOrDefault();
                return tt;
            }
        }

        public MatchStat CheckMatchStat(int home, int guest, string action, int matchId)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<MatchStat>() where x.MatchId == matchId && x.HomeScore == home && x.GuestScore == guest && x.ActionItem == action orderby x.Id descending select x).FirstOrDefault();
                return tt;
            }
        }

        public ObservableCollection<MatchStat> GetMatchStatList(int id)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mymatchstats = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> matchStatList = new ObservableCollection<MatchStat>(from x in mymatchstats where x.MatchId == id orderby x.Id descending select x);
                return matchStatList;
            }
        }

        public ObservableCollection<MatchStat> GetMatchStats(int matchid)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mymatchstats = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> matchStatList = new ObservableCollection<MatchStat>(from x in mymatchstats where x.MatchId == matchid orderby x.Id select x);
                return matchStatList;
            }
        }

        public ObservableCollection<MatchStat> GetGamesStats(int matchid)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mymatchstats = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> matchStatList = new ObservableCollection<MatchStat>(from x in mymatchstats where x.MatchId == matchid && (!x.ActionItem.Contains(AppResources.guestyellow) && !x.ActionItem.Contains(AppResources.guestred) && !x.ActionItem.Contains(AppResources.homeyellow) && !x.ActionItem.Contains(AppResources.homered) && !x.ActionItem.Contains(AppResources.homesubF) && !x.ActionItem.Contains(AppResources.homesubM) && !x.ActionItem.Contains(AppResources.guestsubF) && !x.ActionItem.Contains(AppResources.guestsubM) && !x.ActionItem.Contains(AppResources.Vrijeworp) && !x.ActionItem.Contains(AppResources.Strafworp + " (")) orderby x.Id select x);
                return matchStatList;
            }
        }

        public ObservableCollection<MatchStat> GetWisselsStats(int matchid)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mymatchstats = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> matchStatList = new ObservableCollection<MatchStat>(from x in mymatchstats where x.MatchId == matchid && (x.ActionItem.Contains(AppResources.guestyellow) || x.ActionItem.Contains(AppResources.guestred) || x.ActionItem.Contains(AppResources.homeyellow) || x.ActionItem.Contains(AppResources.homered) || x.ActionItem.Contains(AppResources.homesubF) || x.ActionItem.Contains(AppResources.homesubM) || x.ActionItem.Contains(AppResources.guestsubF) || x.ActionItem.Contains(AppResources.guestsubM)) orderby x.Id select x);
                return matchStatList;
            }
        }

        public ObservableCollection<MatchStat> GetSyncMatchStats(int matchid)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mymatchstats = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> matchStatList = new ObservableCollection<MatchStat>(from x in mymatchstats where x.MatchId == matchid && !x.Sync orderby x.Id select x);
                return matchStatList;
            }
        }

        public MatchStat GetHomeMatchStat(int matchid, int homescore)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<MatchStat>() where x.MatchId == matchid && x.HomeScore == homescore orderby x.GuestScore select x).FirstOrDefault();
                return tt;
            }
        }

        public ObservableCollection<MatchStat> GetHomeMatchStats(int matchid, int homescore)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mylanguagetypes = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> ScoreList = new ObservableCollection<MatchStat>(from x in mylanguagetypes where x.MatchId == matchid && x.HomeScore == homescore select x);
                return ScoreList;
            }
        }

        public MatchStat GetGuestMatchStat(int matchid, int guestscore)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<MatchStat>() where x.MatchId == matchid && x.GuestScore == guestscore orderby x.GuestScore select x).FirstOrDefault();
                return tt;
            }
        }

        public ObservableCollection<MatchStat> GetGuestMatchStats(int matchid, int guestscore)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mylanguagetypes = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> ScoreList = new ObservableCollection<MatchStat>(from x in mylanguagetypes where x.MatchId == matchid && x.GuestScore == guestscore orderby x.Id descending select x);
                return ScoreList;
            }
        }

        public ObservableCollection<MatchStat> GetMatchCards(int matchid, string color)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mymatchstats = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> matchStatList = new ObservableCollection<MatchStat>(from x in mymatchstats where x.MatchId == matchid && x.ActionItem.Contains(color) orderby x.Id select x);
                return matchStatList;
            }
        }

        public ObservableCollection<MatchStat> GetMatchSubs(int matchid, string subM, string subF)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                List<MatchStat> mymatchstats = dbConn.Table<MatchStat>().ToList();
                ObservableCollection<MatchStat> matchStatList = new ObservableCollection<MatchStat>(from x in mymatchstats where x.MatchId == matchid && (x.ActionItem.Contains(subM) || x.ActionItem.Contains(subF)) orderby x.Id select x);
                return matchStatList;
            }
        }

        public PlayerChange GetPlayerChange(int id, bool home)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                var tt = (from x in dbConn.Table<PlayerChange>() where x.Playerchange == id && x.HomeTeam == home select x).FirstOrDefault();
                return tt;
            }
        }

        //Update existing match 
        public void InsertMatch(Match match)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {

                Match existMatch = GetSpecificMatch(match.Home, match.MatchDate, match.MatchTime);
                if (existMatch == null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Insert(match);
                    });

                }
            }
        }

        //Update existing match 
        public void UpdateMatch(Match match)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                Match tt = (from x in dbConn.Table<Match>() where x.Id == match.Id select x).FirstOrDefault();
                if (tt != null)
                {
                    tt.Home = match.Home;
                    tt.Guest = match.Guest;
                    tt.HomeScore = match.HomeScore;
                    tt.GuestScore = match.GuestScore;
                    tt.Active = match.Active;
                    tt.Done = match.Done;
                    tt.MatchDate = match.MatchDate;
                    tt.MatchTime = match.MatchTime;
                    tt.TeamTypeId = match.TeamTypeId;
                    tt.Period = match.Period;
                    tt.FontSize = match.FontSize;
                    tt.TimeOut = match.TimeOut;
                    tt.GuestSW = match.GuestSW;
                    tt.HomeSW = match.HomeSW;
                    tt.HomeColor = match.HomeColor;
                    tt.GuestColor = match.GuestColor;
                    tt.HomeColor1 = match.HomeColor1;
                    tt.GuestColor1 = match.GuestColor1;
                    tt.HorizontalHome = match.HorizontalHome;
                    tt.TwoColorsHome = match.TwoColorsHome;
                    tt.HorizontalGuest = match.HorizontalGuest;
                    tt.TwoColorsGuest = match.TwoColorsGuest;
                    tt.SubNumber = match.SubNumber;
                    tt.CardName = match.CardName;
                    tt.TwoBandsHome = match.TwoBandsHome;
                    tt.TwoBandsGuest = match.TwoBandsGuest;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(tt);
                    });
                }
            }
        }

        //Update existing match 
        public void InactivateMatch(Match match)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                Match tt = (from x in dbConn.Table<Match>() where x.Id == match.Id select x).FirstOrDefault();
                if (tt != null)
                {
                    tt.Home = match.Home;
                    tt.Guest = match.Guest;
                    tt.HomeScore = match.HomeScore;
                    tt.GuestScore = match.GuestScore;
                    tt.Active = false;
                    tt.Done = match.Done;
                    tt.MatchDate = match.MatchDate;
                    tt.MatchTime = match.MatchTime;
                    tt.TeamTypeId = match.TeamTypeId;
                    tt.Period = match.Period;
                    tt.TimeOut = match.TimeOut;
                    tt.GuestSW = match.GuestSW;
                    tt.HomeSW = match.HomeSW;
                    tt.HomeColor = match.HomeColor;
                    tt.GuestColor = match.GuestColor;
                    tt.HomeColor1 = match.HomeColor1;
                    tt.GuestColor1 = match.GuestColor1;
                    tt.HorizontalHome = match.HorizontalHome;
                    tt.TwoColorsHome = match.TwoColorsHome;
                    tt.HorizontalGuest = match.HorizontalGuest;
                    tt.TwoColorsGuest = match.TwoColorsGuest;
                    tt.SubNumber = match.SubNumber;
                    tt.CardName = match.CardName;
                    tt.TwoBandsHome = match.TwoBandsHome;
                    tt.TwoBandsGuest = match.TwoBandsGuest;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(tt);
                    });
                }
            }
        }

        //Delete specific matchstat 
        public void DeleteMatch(Match match)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                ObservableCollection<MatchStat> matchstats = GetMatchStats(match.Id);
                foreach (MatchStat matchstat in matchstats)
                {
                    DeleteMatchStat(matchstat);
                }

                Match existingmatch = (from x in dbConn.Table<Match>() where x.Id == match.Id select x).FirstOrDefault();
                if (existingmatch != null)// && existingmatch.Id != 1)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingmatch);

                    });
                }
            }
        }

        //Update existing match 
        public void InsertMatchStat(MatchStat matchstat)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                MatchStat checkMatchStat = CheckMatchStat(matchstat.HomeScore, matchstat.GuestScore, matchstat.ActionItem, matchstat.MatchId);
                if (checkMatchStat == null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Insert(matchstat);
                    });
                }
                else
                    UpdateMatchStat(matchstat);
            }
        }

        //Update existing match 
        public void UpdateMatchStat(MatchStat matchstat)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                MatchStat tt = (from x in dbConn.Table<MatchStat>() where x.Id == matchstat.Id select x).FirstOrDefault();
                if (tt != null)
                {
                    tt.MatchId = matchstat.MatchId;
                    tt.ActionMin = matchstat.ActionMin;
                    tt.HomeScore = matchstat.HomeScore;
                    tt.GuestScore = matchstat.GuestScore;
                    tt.ActionItem = matchstat.ActionItem;
                    tt.Color = matchstat.Color;
                    tt.Sync = matchstat.Sync;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(tt);
                    });
                }
            }
        }

        //Delete specific matchstat 
        public void DeleteMatchStat(MatchStat matchstat)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                MatchStat existingMatchStat = (from x in dbConn.Table<MatchStat>() where x.Id == matchstat.Id select x).FirstOrDefault();
                if (existingMatchStat != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingMatchStat);
                    });
                }
            }
        }

        //Update existing match 
        public void UpdateActiveMatch(ActiveMatch match)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                ActiveMatch tt = (from x in dbConn.Table<ActiveMatch>() where x.Id == match.Id select x).FirstOrDefault();
                if (tt != null)
                {
                    tt.MatchId = match.MatchId;
                    tt.StartTimer = match.StartTimer;
                    tt.PauseTimer = match.PauseTimer;
                    tt.Reset = match.Reset;
                    tt.LastTime = match.LastTime;
                    tt.PlayTime = match.PlayTime;
                    tt.StartTime = match.StartTime;
                    tt.TimeOutGuest = match.TimeOutGuest;
                    tt.TimeOutHome = match.TimeOutHome;
                    tt.Notificate0 = match.Notificate0;
                    tt.Notificate1 = match.Notificate1;
                    tt.TimeSC = match.TimeSC;
                    tt.WisselGuest = match.WisselGuest;
                    tt.WisselHome = match.WisselHome;
                    tt.HomeRed = match.HomeRed;
                    tt.HomeYellow = match.HomeYellow;
                    tt.GuestRed = match.GuestRed;
                    tt.GuestYellow = match.GuestYellow;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(tt);
                    });
                }
            }
        }

        //Update existing match 
        public void InsertActiveMatch(ActiveMatch matchstat)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(matchstat);
                });
            }
        }


        public void InsertPlayerChange(PlayerChange playerChange)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(playerChange);
                });
            }
        }

        public void DeletePlayerChange(PlayerChange playerChange)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Delete(playerChange);
                });
            }
        }

        //Update existing match 
        public void InsertClub(Clubs club)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(club);
                });

            }
        }

        //Update existing match 
        public void UpdateClub(Clubs club)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                Clubs tt = (from x in dbConn.Table<Clubs>() where x.Id == club.Id select x).FirstOrDefault();
                if (tt != null)
                {
                    tt.Description = club.Description;
                    tt.ClubColor = club.ClubColor;
                    tt.ClubColor1 = club.ClubColor1;
                    tt.HorizontalClub = club.HorizontalClub;
                    tt.TwoColorsClub = club.TwoColorsClub;
                    tt.TwoBandsClub = club.TwoBandsClub;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(tt);
                    });
                }
            }
        }

    }
}