using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEH.FOOTBALL.BIZ.Model
{
    public class LeagueModel
    {
        public string StrDate { get; set; }
        public int LeagueId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public List<PlayerModel> AllPlayer { get; set; }
        public List<TeamModel> Teams { get; set; }
        public List<PlayModel> Games { get; set; }
        public int Status { get; set; }
        public string PlayTime { get; set; }
        public string PlayTeamCnt { get; set; }
    }
    public class PlayModel
    {
        public int LeaguId { get; set; }
        public int PlayId { get; set; }
        public TeamModel LeftTeam { get; set; }
        public TeamModel RightTeam { get; set; }
        public bool IsEnd { get; set; }
        public string WinTeamType { get; set; }
    }
    public class TeamModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamType { get; set; }
        public int Key { get; set; }
        public string Label { get; set; }
        
        public List<PlayerModel> Players { get; set; }
        public int Score { get; set; }
        public int WinCnt { get; set; }
        public int TieCnt { get; set; }
        public int LossCnt { get; set; }
        public int WinScore { get { return WinCnt * 3 + TieCnt * 1; } }
        public TeamModel Clone()
        {
            return this.MemberwiseClone() as TeamModel;
        }
    }


    public class PlayerModel
    {
        public string ImageId { get; set; }
        public int TeamId { get; set; }
        public bool IsChecked { get; set; }
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Goal { get; set; }
        public int Assist { get; set; }
        public int Save { get; set; }
        public int Score { get; set; }
        public string TeamName { get; set; }
        public string TeamType { get; set; }
    }
   
    public class LeagueResult
    {
        public int LeagueID { get; set; }
        public string LocationName { get; set; }
        public string StrDate { get; set; }
        public List<TeamModel> Teams { get; set; }
        public List<PlayerModel> BestPlayer { get; set; }
        public int PlayerMaxScore { get; set; }

    }

}
