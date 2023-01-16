using ZENC.CORE.API.Common;
using SmartSql;
using System.Data;
using ZENC.CORE;
using System.Xml.Linq;
using SmartSql.DbSession;
using Microsoft.VisualBasic;
using SmartSql.Configuration.Tags;
using static SmartSql.Configuration.Tags.Switch;
using Newtonsoft.Json;
using EEH.FOOTBALL.BIZ.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using SmartSql.Configuration;

namespace EEH.FOOTBALL.BIZ
{
    public class FootballBiz
    {
        public List<TeamModel> GetTeams()
        {
            List<TeamModel> rtn = null;
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    rtn = GetTeams(dbseesion);
                });
            }

            return rtn;
        }

        public List<LeagueResult> GetLeagueHistory()
        {
            List<LeagueResult> rtn = new List<LeagueResult>();
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    using (DataSet ds = dbseesion.GetDataSet(SmartSqlMapper.Instance.GetContext("SQL", "SELLEAGUEHISTORY", null)))
                    {
                        if(ds.Tables.Count > 1)
                        {
                            DataTable teamDT = ds.Tables[0];
                            DataTable playerDT = ds.Tables[1];

                            
                            if(teamDT.Rows.Count > 0)
                            {
                                foreach (DataRow row in teamDT.Rows) 
                                {
                                    int leagueID = row["pk_id"].EzInt();
                                    LeagueResult result =rtn.Find(x=>x.LeagueID == leagueID);
                                    if (result.EzIsNull())
                                    {
                                        result = new LeagueResult();
                                        result.LeagueID = leagueID;
                                        result.Teams = new List<TeamModel>();
                                        result.BestPlayer = new List<PlayerModel>();
                                        result.LocationName = row["locationname"].EzToString();
                                        result.StrDate = row["col_date"].EzToString();

                                        rtn.Add(result);
                                    }

                                    TeamModel team = new TeamModel();
                                    
                                    team.TeamName = row["teamname"].EzToString();
                                    team.TeamType = row["teamtype"].EzToString();
                                    team.WinCnt = row["WINCNT"].EzInt();
                                    team.TieCnt = row["tiecnt"].EzInt();
                                    team.LossCnt = row["losscnt"].EzInt();
                                    team.Players = new List<PlayerModel>();
                                    result.Teams.Add(team);
                                    result.Teams = result.Teams.OrderByDescending(x => x.WinScore).ToList();
                                }
                            }

                            if(playerDT.Rows.Count > 0)
                            {
                             
                                foreach (DataRow row in playerDT.Rows)
                                {
                                    int leagueID = row["leagueid"].EzInt();
                                    LeagueResult result = rtn.Find(x => x.LeagueID == leagueID);

                                    if (result.EzNotNull())
                                    {
                                        

                                        PlayerModel player = new PlayerModel();

                                        player.Name = row["col_name"].EzToString();
                                        player.PlayerId = row["playerid"].EzInt();
                                        
                                        player.Goal = row["goal"].EzInt();
                                        player.Assist = row["assist"].EzInt();
                                        player.Save = row["save"].EzInt();
                                        player.Score = row["playerscore"].EzInt();


                                        player.TeamName = row["teamname"].EzToString();
                                        player.TeamType = row["teamtype"].EzToString();

                                        TeamModel md = result.Teams.Find(x => x.TeamName == player.TeamName);
                                        if (md != null)
                                        {
                                            md.Players.Add(player);
                                        }

                                        
                                        if(player.Score > 0)
                                        {
                                            if(result.BestPlayer.Count == 0)
                                            {
                                                result.BestPlayer.Add(player);
                                                result.PlayerMaxScore = player.Score;
                                            }
                                            else
                                            {
                                                if(result.PlayerMaxScore == player.Score)
                                                {
                                                    result.BestPlayer.Add(player);
                                                }
                                                else if (result.PlayerMaxScore < player.Score)
                                                {
                                                    result.BestPlayer.Clear();
                                                    result.BestPlayer.Add(player);
                                                    result.PlayerMaxScore = player.Score;
                                                }
                                            }
                                        }
                                        
                                    }
                                }

                            }


                        }
                    }
                });
            }
            return rtn.OrderByDescending(x => x.StrDate.EzInt()).ToList();
        }
        public LeagueModel GetRunningLeague()
        {
            LeagueModel rtn = null;
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    using (DataTable dt = dbseesion.GetDataTable(SmartSqlMapper.Instance.GetContext("SQL", "SELLEAGUEEDIT", null)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            rtn = new LeagueModel();
                            rtn.LeagueId = row["pk_id"].EzInt();
                            rtn.StrDate = row["col_date"].EzToString();
                            rtn.LocationId = row["fk_location_id"].EzInt();
                            rtn.LocationName = row["locationname"].EzToString();
                            rtn.Status = row["col_status"].EzInt();


                            List<PlayerModel> playersList = new List<PlayerModel>();
                            rtn.Teams = GetLeagueTeams(rtn.LeagueId, dbseesion, ref playersList);
                            rtn.AllPlayer = playersList;

                            rtn.Games = new List<PlayModel>();


                            Dictionary<string, object> dic2 = new Dictionary<string, object>();
                            dic2.Add("leagueid", rtn.LeagueId);
                            using (DataTable dt2= dbseesion.GetDataTable(SmartSqlMapper.Instance.GetContext("SQL", "SELLEAGUEPLAYLIST", dic2)))
                            {
                                if (dt2.EzNotNull() && dt2.Rows.Count > 0)
                                {
                                    foreach(DataRow row2 in dt2.Rows)
                                    {
                                        PlayModel game = new PlayModel();
                                        game.PlayId = row2["pk_id"].EzInt();
                                        game.LeaguId = rtn.LeagueId;
                                        game.LeftTeam = rtn.Teams.Find(x => x.TeamId == row2["fk_left_team_id"].EzInt()).Clone();
                                        game.RightTeam = rtn.Teams.Find(x => x.TeamId == row2["fk_right_team_id"].EzInt()).Clone();
                                        game.LeftTeam.Score = row2["col_left_score"].EzInt();
                                        game.RightTeam.Score = row2["col_right_score"].EzInt();
                                        game.IsEnd = true;
                                        game.WinTeamType = "";
                                        if (game.LeftTeam.Score != game.RightTeam.Score)
                                        {
                                            if(game.LeftTeam.Score > game.RightTeam.Score)
                                            {
                                                game.WinTeamType = game.LeftTeam.TeamType;
                                            }
                                            else
                                            {
                                                game.WinTeamType = game.RightTeam.TeamType;
                                            }
                                        }
                                        
                                        rtn.Games.Add(game);
                                    }
                                }
                            }

                        }
                    }
                });
            }

            return rtn;
        }
        private static List<TeamModel> GetTeams(IDbSession dbseesion)
        {
            List<TeamModel> rtn = new List<TeamModel>();

            using (DataTable dt = dbseesion.GetDataTable(SmartSqlMapper.Instance.GetContext("SQL", "SELTEAMTYPES", null)))
            {
                if (dt.EzNotNull() && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        TeamModel model = new TeamModel();
                        model.TeamId = row["pk_id"].EzInt();
                        model.TeamName = row["col_name"].EzToString();
                        model.Key = model.TeamId;
                        model.Label = model.TeamName;
                        model.TeamType = row["col_type"].EzToString();
                        model.Players = new List<PlayerModel>();
                        rtn.Add(model);
                    }
                }
            }
            return rtn;
        }

        static List<TeamModel> GetLeagueTeams(int leagueid, IDbSession dbseesion, ref List<PlayerModel> players)
        {
            List<TeamModel> rtn = GetTeams(dbseesion);
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("leagueid", leagueid);

            using (DataTable dt = dbseesion.GetDataTable(SmartSqlMapper.Instance.GetContext("SQL", "SELLEAGUEPLAYER", dic)))
            {
                if (dt.EzNotNull() && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int teamTypeID = row["fk_team_type_id"].EzInt();
                        int playerID = row["fk_member_id"].EzInt();
                        string palyerName = row["col_name"].EzToString();

                        PlayerModel player = new PlayerModel { PlayerId = playerID, Name = palyerName };
                        players.Add(player);

                        TeamModel team = rtn.Find(x => x.TeamId == teamTypeID);
                        if (team.EzNotNull())
                        {
                            team.Players.Add(player);
                        }
                    }
                }
            }

            return rtn;
        }


        public bool UpsertMember(string name, string phone, string birthday, int pkid)
        {
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("name", name);
                    dic.Add("birthday", birthday);
                    dic.Add("phone", phone);

                    if (pkid < 0)
                    {
                        pkid = dbseesion.ExecuteScalar<int>(SmartSqlMapper.Instance.GetContext("SQL", "CHKMEMBER", dic));
                    }

                    if (pkid > 0)
                    {
                        dic.Add("pkid", pkid);

                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATEMEMBER", dic));
                    }
                    else
                    {

                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "INSERTMEMBER", dic));
                    }

                });
            }

            return true;
        }

        public bool Upsertlocation(Dictionary<string, object> row)
        {
            string name = row["col_name"].EzToString();
            string add = row["col_address"].EzToString();
            string add2 = row["col_address2"].EzToString();
            double la = row["col_latitude"].EzDouble();
            double lo = row["col_longitude"].EzDouble();
            int pkid = row["pk_id"].EzInt();

            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("name", name);
                    dic.Add("add", add);
                    dic.Add("add2", add2);
                    dic.Add("la", la);
                    dic.Add("lo", lo);

                    if (pkid < 0)
                    {
                        pkid = dbseesion.ExecuteScalar<int>(SmartSqlMapper.Instance.GetContext("SQL", "CHKLOCATION", dic));
                    }

                    if (pkid > 0)
                    {
                        dic.Add("pkid", pkid);

                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATELOCATION", dic));
                    }
                    else
                    {

                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "INSERTLOCATION", dic));
                    }

                });
            }

            return true;
        }

        public void InsertLeagueMember(int leagueid, List<int> ps,bool isnext)
        {
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("leagueid", leagueid);
                    dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "DELETELEAGUEPLAYER", dic));
                    foreach (var id in ps)
                    {

                        dic.Clear();
                        dic.Add("leagueid", leagueid);
                        dic.Add("memberid", id);


                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "INSERTLEAGUEPLAYER", dic));
                    }
                    dic.Clear();
                    dic.Add("col_status", 3);
                    dic.Add("pk_id", leagueid);
                    if(isnext)
                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATELEAGUESTATUS", dic));
                });
            }
        }
        /*
           public void TeamMapping(int leagueid, List<Dictionary<string, object>> players)
        {
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    foreach (var dic in players)
                    {
                        Dictionary<string, object> dic2 = new Dictionary<string, object>();
                        dic2.Add("fk_team_type_id", dic["fk_team_type_id"].EzInt());
                        dic2.Add("pk_id", dic["pk_id"].EzInt());
                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATETEAMMAPPING", dic2));

                    }

                    Dictionary<string, object> dic3 = new Dictionary<string, object>();
                    dic3.Add("col_status", 4);
                    dic3.Add("pk_id", leagueid);
                    dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATELEAGUESTATUS", dic3));
                });
            }
        } 
         
         */
        public void TeamMapping(int leagueid, List<Dictionary<string, object>> players)
        {
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    foreach (var dic in players)
                    {
                        Dictionary<string, object> dic2 = new Dictionary<string, object>();
                        dic2.Add("fk_team_type_id", dic["teamId"].EzInt());
                        dic2.Add("playerid", dic["playerId"].EzInt());
                        dic2.Add("leagueid", leagueid);
                        dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATETEAMMAPPING", dic2));

                    }

                    Dictionary<string, object> dic3 = new Dictionary<string, object>();
                    dic3.Add("col_status", 4);
                    dic3.Add("pk_id", leagueid);
                    dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATELEAGUESTATUS", dic3));
                });
            }
        }
      
        
        public void UpsertGameInfo(int leagueid, Dictionary<string, object> gameInfo)
        {

            int pk_id = gameInfo["playId"].EzInt();


            if (gameInfo.ContainsKey("leftTeam") && gameInfo.ContainsKey("rightTeam"))
            {


                Dictionary<string, object> leftTeam = JsonConvert.DeserializeObject<Dictionary<string, object>>(gameInfo["leftTeam"].EzToString());
                Dictionary<string, object> rightTeam = JsonConvert.DeserializeObject<Dictionary<string, object>>(gameInfo["rightTeam"].EzToString());

                if (leftTeam.ContainsKey("players") && rightTeam.ContainsKey("players"))
                {

                    List<Dictionary<string, object>> leftMembers = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(leftTeam["players"].EzToString());
                    List<Dictionary<string, object>> rightMembers = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(rightTeam["players"].EzToString());

                    List<Dictionary<string, object>> players = new List<Dictionary<string, object>>();
                    players.AddRange(leftMembers);
                    players.AddRange(rightMembers);
                    int leftScore = leftTeam["score"].EzInt();
                    int rightScore = rightTeam["score"].EzInt();
                    int leftTeamID = leftTeam["teamId"].EzInt();
                    int rightTeamID = rightTeam["teamId"].EzInt();
                    using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
                    {
                        dbseesion.TransactionWrap(() =>
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("fk_league_id", leagueid);
                            dic.Add("fk_left_team_id", leftTeamID);
                            dic.Add("fk_right_team_id", rightTeamID);
                            dic.Add("leftscore", leftScore);
                            dic.Add("rightscore", rightScore);
                            if (pk_id > 0)
                            {
                                dic.Add("pk_id", pk_id);
                                dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "UPDATEGAME", dic));
                            }
                            else
                            {
                                pk_id = dbseesion.ExecuteScalar<int>(SmartSqlMapper.Instance.GetContext("SQL", "INSERTGAME", dic));
                            }


                            dic.Clear();
                            dic.Add("pk_id", pk_id);
                            dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "DELETEGAMEDTL", dic));

                            
                            foreach (var member in players)
                            {
                                dic.Clear();
                                int goal = member["goal"].EzInt();
                                int assist = member["assist"].EzInt();
                                int save = member["save"].EzInt();

                                List<int> coltypes = new List<int>();
                                for (int i = 0; i < goal; i++)
                                {
                                    coltypes.Add(0);
                                }
                                for (int i = 0; i < assist; i++)
                                {
                                    coltypes.Add(1);
                                }
                                for (int i = 0; i < save; i++)
                                {
                                    coltypes.Add(2);
                                }

                                foreach (int type in coltypes)
                                {
                                    dic.Clear();
                                    dic.Add("playid", pk_id);
                                    dic.Add("coltype", type);
                                    dic.Add("memberid", member["playerId"].EzInt());
                                    dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "INSERTGAMEDTL", dic));
                                }
                            }



                        });
                    }
                }

            }

        }
        public void DeleteGameInfo(int gameid)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pk_id", gameid);
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    dbseesion.Execute(SmartSqlMapper.Instance.GetContext("SQL", "DELETEGAME", dic));

                });
            }
        }
    }
}