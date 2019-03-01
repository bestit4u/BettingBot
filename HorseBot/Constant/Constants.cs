using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Betburger.Constant
{
    public enum BOOKIE
    {
        Bet365 = 0,
        Ladbrokes,
        Betfair,
        Betdaq,
        Matchbook
    }

    public enum MARKETCATEGORY
    {
        Handicap = 0,
        OverUnder,
        Moneyline
    }

    public enum SPORT
    {
        Horse,
        Dog
    }

    public enum SIDE
    {
        Back,
        Lay
    }




    public class Constants
    {
        public static bool bRun = false;
        public static CookieContainer container = new CookieContainer();

        public static int step = 0;
        public static DateTime start = DateTime.UtcNow;
        public static string pstk { get; set; }

        public static string[] allSports = {
                                            "Soccer",
                                            "Tennis",
                                            "Badminton",
                                            "Basketball",
                                            "Beach Volleyball",
                                            "Cricket",
                                            "E-Sports",
                                            "Golf",
                                            "Hockey",
                                            "Horse Racing",
                                            "Ice Hockey",
                                            "Volleyball",
                                            "Baseball",
                                            "In-Play Coupon",
                                            "Greyhounds",
                                            "Snooker",
                                            "American Football",
                                            "Austrailian Rules",
                                            "Boxing/UFC",
                                            "Cycling",
                                            "Darts",
                                            "Gaelic Sports",
                                            "Lotto",
                                            "Pool",
                                            "Rugby League",
                                            "Rugby Union",
                                            "Snooker",
                                            "Virtual Sports",
                                            "Curling",
                                            "Floorball",
                                            "Handball",
                                            "Netball",
                                            "Surfing",
                                            "Trotting",
                                            "Table Tennis"
                                        };

        public static string[] sports = {
                                            "Soccer", "American Football", "Basketball"
                                        };

        public static string[] soccerleaguesBet365 = {
                                                           "England", "Spain", "Italy", "Germany", "France", "UEFA", "Championship"
                                                     };

        public static string[] soccerleaguesLadbrokes = {
                                                            "English", "Spainish", "Italian", "German", "Franch", "UEFA", "Championship"
                                                        };

        public static string[] soccerleaguesPinnacle = {
                                                           "England", "Spain", "Italy", "Germany", "France", "UEFA", "Championship"
                                                     };
    }
}
