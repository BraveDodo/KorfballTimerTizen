using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KorfballTimerTizen.Model
{
    public class TeamType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public byte Ball { get; set; }
        public float Pole { get; set; }
        public byte FieldLong { get; set; }
        public byte FieldWide { get; set; }
        public TimeSpan PlayTime { get; set; }
        public byte Periods { get; set; }
        public bool Indoors { get; set; }
        public string Description { get; set; }
        public TimeSpan PlayTime1 { get; set; }
        public bool GoldenGoal { get; set; }
        public bool Strafworp { get; set; }

        public TeamType()
        { }
        public TeamType(string description, byte ball, float pole, byte fieldlong, byte fieldwide, System.TimeSpan playtime, byte periods, bool indoors, System.TimeSpan playtime1, bool strafworp)
        {
            Description = description;
            Ball = ball;
            Pole = pole;
            FieldLong = fieldlong;
            FieldWide = fieldwide;
            PlayTime = playtime;
            Periods = periods;
            Indoors = indoors;
            PlayTime1 = playtime1;
            Strafworp = strafworp;
        }
    }

    public class TeamTypeNames
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public TeamTypeNames()
        { }
        public TeamTypeNames(string description)
        {
            Description = description;
        }
    }

    public class IKFCountries
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Hyph { get; set; }
        public IKFCountries()
        { }
        public IKFCountries(string description, string hyph)
        {
            Description = description;
            Hyph = hyph;
        }
    }

    public class Match
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchTime { get; set; }
        public string Home { get; set; }
        public string Guest { get; set; }
        public int HomeScore { get; set; }
        public int GuestScore { get; set; }
        public int TeamTypeId { get; set; }
        public byte Period { get; set; }
        public bool Active { get; set; }
        public bool Done { get; set; }
        public int FontSize { get; set; }
        public bool TimeOut { get; set; }
        public int HomeSW { get; set; }
        public int GuestSW { get; set; }
        public string HomeColor { get; set; }
        public string GuestColor { get; set; }
        public string HomeColor1 { get; set; }
        public string GuestColor1 { get; set; }
        public bool HorizontalHome { get; set; }
        public bool HorizontalGuest { get; set; }
        public bool TwoColorsHome { get; set; }
        public bool TwoColorsGuest { get; set; }
        public bool TwoBandsHome { get; set; }
        public bool TwoBandsGuest { get; set; }
        public bool SubNumber { get; set; }
        public bool CardName { get; set; }

        public Match()
        { }
        public Match(DateTime matchdate, System.TimeSpan matchtime, string home, string guest, int homescore, int guestscore, int teamtypeId, byte period, bool active, bool done, int fontSize)
        {
            MatchDate = matchdate;
            MatchTime = matchtime;
            Home = home;
            Guest = guest;
            HomeScore = homescore;
            GuestScore = guestscore;
            TeamTypeId = teamtypeId;
            Period = period;
            Active = active;
            Done = done;
            FontSize = fontSize;
        }


    }

    public class ActiveMatch
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MatchId { get; set; }
        public DateTime LastTime { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan PlayTime { get; set; }
        public bool StartTimer { get; set; }
        public bool PauseTimer { get; set; }
        public bool Reset { get; set; }
        public bool Notificate0 { get; set; }
        public bool Notificate1 { get; set; }
        public byte TimeOutHome { get; set; }
        public byte TimeOutGuest { get; set; }
        public byte WisselHome { get; set; }
        public byte WisselGuest { get; set; }
        public byte HomeRed { get; set; }
        public byte HomeYellow { get; set; }
        public byte GuestRed { get; set; }
        public byte GuestYellow { get; set; }
        public TimeSpan TimeSC { get; set; }

        public ActiveMatch()
        { }
        public ActiveMatch(int matchid)
        { MatchId = matchid; }
    }

    public class Ball
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public byte BallSize { get; set; }
        public Ball()
        { }
        public Ball(byte ballsize)
        {
            BallSize = ballsize;
        }

    }

    public class Pole
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public float PoleSize { get; set; }
        public Pole()
        { }
        public Pole(float polesize)
        {
            PoleSize = polesize;
        }

    }

    public class Numbers
    {
        public int Id { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public Numbers()
        { }
    }

    public class Clubs
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ClubColor { get; set; }
        public string ClubColor1 { get; set; }
        public bool HorizontalClub { get; set; }
        public bool TwoColorsClub { get; set; }
        public bool TwoBandsClub { get; set; }
        public string Description { get; set; }
        public Clubs()
        { }
    }

    public class TimerSettings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool TimerStart { get; set; }
        //public byte StartOutdoor { get; set; }
        public bool Indoor { get; set; }
        public bool CanNotGoBack { get; set; }
        public int CountryId { get; set; }
        public string CountryHyph { get; set; }
        [MaxLength(3)]
        public int Vibrate { get; set; }
        public bool ScreenOn { get; set; }
        public TimeSpan ScreenTime { get; set; }
        public bool Round { get; set; }
        public int ScreenWidth { get; set; }

        public TimerSettings()
        { }
        public TimerSettings(bool timerstart, bool indoor, bool cannotgoback, int countryId, int vibrate, bool screenon, TimeSpan screentime)
        {
            TimerStart = timerstart;
            Indoor = indoor;
            CanNotGoBack = cannotgoback;
            CountryId = countryId;
            Vibrate = vibrate;
        }
    }

    public class RefSettings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool StopAtChange { get; set; }
        public bool MatchStats { get; set; }
        public bool SaveMatch { get; set; }
        public bool SubNumber { get; set; }
        public bool CardName { get; set; }
        public bool Audio1Min { get; set; }
        public bool AudioEnd { get; set; }
        public bool TimeOut { get; set; }
        public RefSettings()
        { }
        public RefSettings(bool stopatchange, bool matchstats, bool savematch, bool audio1min, bool audioend, bool timeout, bool subNumber, bool cardname)
        {
            StopAtChange = stopatchange;
            MatchStats = matchstats;
            SaveMatch = savematch;
            SubNumber = subNumber;
            CardName = cardname;
            Audio1Min = audio1min;
            AudioEnd = audioend;
            TimeOut = timeout;
        }
    }

    public class FieldType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool Indoor { get; set; }
        public FieldType()
        { }
        public FieldType(bool indoor)
        {
            Indoor = indoor;
        }
    }

    public class MatchStat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int ActionMin { get; set; }
        public int HomeScore { get; set; }
        public int GuestScore { get; set; }
        public string ActionItem { get; set; }
        public string PlayerName { get; set; }
        public string Color { get; set; }
        public bool Sync { get; set; }

        public MatchStat()
        { }
        public MatchStat(int match, int actionmin, int homescore, int guestscore, string actionitem)
        {
            MatchId = match;
            ActionMin = actionmin;
            HomeScore = homescore;
            GuestScore = guestscore;
            ActionItem = actionitem;
        }
    }

    public class PlayerChange
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Playerchange { get; set; }
        public bool HomeTeam { get; set; }

        public PlayerChange()
        { }
    }


}