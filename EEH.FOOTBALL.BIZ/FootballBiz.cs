
using SmartSql;
using System.Data;

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
using EEH.WEB.Common;

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
                        if (ds.Tables.Count > 1)
                        {
                            DataTable teamDT = ds.Tables[0];
                            DataTable playerDT = ds.Tables[1];


                            if (teamDT.Rows.Count > 0)
                            {
                                foreach (DataRow row in teamDT.Rows)
                                {
                                    int leagueID = row["pk_id"].ExInt();
                                    LeagueResult result = rtn.Find(x => x.LeagueID == leagueID);
                                    if (result.ExIsNull())
                                    {
                                        result = new LeagueResult();
                                        result.LeagueID = leagueID;
                                        result.Teams = new List<TeamModel>();
                                        result.BestPlayer = new List<PlayerModel>();
                                        result.LocationName = row["locationname"].ExToString();
                                        result.StrDate = row["col_date"].ExToString();

                                        rtn.Add(result);
                                    }

                                    TeamModel team = new TeamModel();

                                    team.TeamName = row["teamname"].ExToString();
                                    team.TeamType = row["teamtype"].ExToString();
                                    team.WinCnt = row["WINCNT"].ExInt();
                                    team.TieCnt = row["tiecnt"].ExInt();
                                    team.LossCnt = row["losscnt"].ExInt();
                                    team.Players = new List<PlayerModel>();
                                    result.Teams.Add(team);
                                    result.Teams = result.Teams.OrderByDescending(x => x.WinScore).ToList();
                                }
                            }

                            if (playerDT.Rows.Count > 0)
                            {

                                foreach (DataRow row in playerDT.Rows)
                                {
                                    int leagueID = row["leagueid"].ExInt();
                                    LeagueResult result = rtn.Find(x => x.LeagueID == leagueID);

                                    if (result.ExNotNull())
                                    {


                                        PlayerModel player = new PlayerModel();

                                        player.Name = row["col_name"].ExToString();
                                        player.PlayerId = row["playerid"].ExInt();

                                        player.Goal = row["goal"].ExInt();
                                        player.Assist = row["assist"].ExInt();
                                        player.Save = row["save"].ExInt();
                                        player.Score = row["playerscore"].ExInt();
                                        player.ImageId = row["col_imageid"].ExToString();

                                        player.TeamName = row["teamname"].ExToString();
                                        player.TeamType = row["teamtype"].ExToString();

                                        TeamModel md = result.Teams.Find(x => x.TeamName == player.TeamName);
                                        if (md != null)
                                        {
                                            md.Players.Add(player);
                                        }


                                        if (player.Score > 0)
                                        {
                                            if (result.BestPlayer.Count == 0)
                                            {
                                                result.BestPlayer.Add(player);
                                                result.PlayerMaxScore = player.Score;
                                            }
                                            else
                                            {
                                                if (result.PlayerMaxScore == player.Score)
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
            return rtn.OrderByDescending(x => x.StrDate.ExInt()).ToList();
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
                            rtn.LeagueId = row["pk_id"].ExInt();
                            rtn.StrDate = row["col_date"].ExToString();
                            rtn.LocationId = row["fk_location_id"].ExInt();
                            rtn.LocationName = row["locationname"].ExToString();
                            rtn.Status = row["col_status"].ExInt();
                            
                            
                            int ptime = row["col_play_time"].ExInt();
                            int pcnt = row["col_play_team_cnt"].ExInt();
                            if (ptime > 0)
                            {
                                rtn.PlayTime = ptime.ToString();
                            }
                            if (pcnt > 0)
                            {
                                rtn.PlayTeamCnt = pcnt.ToString();
                            }
                            List<PlayerModel> playersList = new List<PlayerModel>();
                            rtn.Teams = GetLeagueTeams(rtn.LeagueId, dbseesion, ref playersList);
                            rtn.AllPlayer = playersList;

                            rtn.Games = new List<PlayModel>();


                            Dictionary<string, object> dic2 = new Dictionary<string, object>();
                            dic2.Add("leagueid", rtn.LeagueId);
                            using (DataTable dt2 = dbseesion.GetDataTable(SmartSqlMapper.Instance.GetContext("SQL", "SELLEAGUEPLAYLIST", dic2)))
                            {
                                if (dt2.ExNotNull() && dt2.Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in dt2.Rows)
                                    {
                                        PlayModel game = new PlayModel();
                                        game.PlayId = row2["pk_id"].ExInt();
                                        game.LeaguId = rtn.LeagueId;
                                        game.LeftTeam = rtn.Teams.Find(x => x.TeamId == row2["fk_left_team_id"].ExInt()).Clone();
                                        game.RightTeam = rtn.Teams.Find(x => x.TeamId == row2["fk_right_team_id"].ExInt()).Clone();
                                        game.LeftTeam.Score = row2["col_left_score"].ExInt();
                                        game.RightTeam.Score = row2["col_right_score"].ExInt();
                                        game.IsEnd = true;
                                        game.WinTeamType = "";
                                        if (game.LeftTeam.Score != game.RightTeam.Score)
                                        {
                                            if (game.LeftTeam.Score > game.RightTeam.Score)
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
                if (dt.ExNotNull() && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        TeamModel model = new TeamModel();
                        model.TeamId = row["pk_id"].ExInt();
                        model.TeamName = row["col_name"].ExToString();
                        model.Key = model.TeamId;
                        model.Label = model.TeamName;
                        model.TeamType = row["col_type"].ExToString();
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
                if (dt.ExNotNull() && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int teamTypeID = row["fk_team_type_id"].ExInt();
                        int playerID = row["fk_member_id"].ExInt();
                        string palyerName = row["col_name"].ExToString();

                        PlayerModel player = new PlayerModel { PlayerId = playerID, Name = palyerName };
                        players.Add(player);

                        TeamModel team = rtn.Find(x => x.TeamId == teamTypeID);
                        if (team.ExNotNull())
                        {
                            team.Players.Add(player);
                        }
                    }
                }
            }
            if (rtn != null && rtn.Count > 0)
            {
                return rtn.Where(x => x.Players?.Count > 0)?.ToList();
            }
            else
                return rtn;
        }


        public bool UpsertMember(string imageid,string name, string phone, string birthday, int pkid)
        {
            using (var dbseesion = SmartSqlMapper.Instance.SqlContext.Open())
            {
                dbseesion.TransactionWrap(() =>
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("name", name);
                    dic.Add("birthday", birthday);
                    dic.Add("phone", phone);
                    dic.Add("imageid", imageid);

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
            string name = row["col_name"].ExToString();
            string add = row["col_address"].ExToString();
            string add2 = row["col_address2"].ExToString();
            double la = row["col_latitude"].ExDouble();
            double lo = row["col_longitude"].ExDouble();
            int pkid = row["pk_id"].ExInt();

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

        public void InsertLeagueMember(int leagueid, List<int> ps, bool isnext)
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
                    if (isnext)
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
                        dic2.Add("fk_team_type_id", dic["fk_team_type_id"].ExInt());
                        dic2.Add("pk_id", dic["pk_id"].ExInt());
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
                        dic2.Add("fk_team_type_id", dic["teamId"].ExInt());
                        dic2.Add("playerid", dic["playerId"].ExInt());
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

            int pk_id = gameInfo["playId"].ExInt();


            if (gameInfo.ContainsKey("leftTeam") && gameInfo.ContainsKey("rightTeam"))
            {


                Dictionary<string, object> leftTeam = JsonConvert.DeserializeObject<Dictionary<string, object>>(gameInfo["leftTeam"].ExToString());
                Dictionary<string, object> rightTeam = JsonConvert.DeserializeObject<Dictionary<string, object>>(gameInfo["rightTeam"].ExToString());

                if (leftTeam.ContainsKey("players") && rightTeam.ContainsKey("players"))
                {

                    List<Dictionary<string, object>> leftMembers = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(leftTeam["players"].ExToString());
                    List<Dictionary<string, object>> rightMembers = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(rightTeam["players"].ExToString());

                    List<Dictionary<string, object>> players = new List<Dictionary<string, object>>();
                    players.AddRange(leftMembers);
                    players.AddRange(rightMembers);
                    int leftScore = leftTeam["score"].ExInt();
                    int rightScore = rightTeam["score"].ExInt();
                    int leftTeamID = leftTeam["teamId"].ExInt();
                    int rightTeamID = rightTeam["teamId"].ExInt();
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
                                int goal = member["goal"].ExInt();
                                int assist = member["assist"].ExInt();
                                int save = member["save"].ExInt();

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
                                    dic.Add("memberid", member["playerId"].ExInt());
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