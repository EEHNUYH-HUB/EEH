using EEH.DB.Models;

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EEH.DB.DA
{
    public abstract class BaseDBAccess
    {
        public string ConnectString { get; set; }
        public BaseDBAccess(string connectString)
        {
            ConnectString = connectString;
        }

        protected abstract DbConnection GetConnection();
        protected abstract DbCommand GetCommand(string txt, DbConnection con);
        protected abstract DbDataAdapter GetAdapter(DbCommand cmd);

        public bool ConnectionText()
        {
            try
            {
                using (DbConnection con = GetConnection())
                {
                    con.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public DataSet FillDataSet(string storedProcedure, List<DbParameter> paramArray)
        {
            DataSet rtn = new DataSet();

            using (DbConnection con = GetConnection())
            {
                using (DbCommand cmd = GetCommand(storedProcedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (paramArray != null)
                    {
                        foreach (DbParameter param in paramArray)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    using (DbDataAdapter da = GetAdapter(cmd))
                    {
                        da.Fill(rtn);
                    }
                }
            }
            return rtn;
        }

        public DataSet FillDataSetUsingQuery(string cmdText)
        {
            DataSet rtn = new DataSet();
            using (DbConnection con = GetConnection())
            {
                using (DbCommand cmd = GetCommand(cmdText, con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (DbDataAdapter da = GetAdapter(cmd))
                    {
                        da.Fill(rtn);
                    }
                }
            }
            return rtn;
        }


        public DataTable FillDataTable(string storedProcedure, List<DbParameter> paramArray)
        {
            DataTable rtn = new DataTable();

            using (DbConnection con = GetConnection())
            {
                using (DbCommand cmd = GetCommand(storedProcedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (paramArray != null)
                    {
                        foreach (DbParameter param in paramArray)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    using (DbDataAdapter da = GetAdapter(cmd))
                    {
                        da.Fill(rtn);
                    }
                }
            }

            return rtn;
        }

        public DataTable FillDataTableUsingQuery(string cmdText)
        {
            DataTable rtn = new DataTable();
            using (DbConnection con = GetConnection())
            {
                using (DbCommand cmd = GetCommand(cmdText, con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (DbDataAdapter da = GetAdapter(cmd))
                    {
                        da.Fill(rtn);
                    }
                }
            }
            return rtn;
        }

        static readonly object sync = new object();


        public void ExecuteNonQuery(string storedProcedure, List<DbParameter> paramArray)
        {
            lock (sync)
            {
                using (DbConnection con = GetConnection())
                {
                    con.Open();
                    using (DbTransaction tr = con.BeginTransaction())
                    {
                        using (DbCommand cmd = con.CreateCommand())
                        {
                            if (cmd.ExNotNull())
                            {
                                cmd.CommandText = storedProcedure;
                                cmd.CommandType = CommandType.StoredProcedure;

                                if (paramArray.ExNotNull())
                                {
                                    foreach (NpgsqlParameter param in paramArray)
                                    {
                                        cmd.Parameters.Add(param);
                                    }
                                }

                                cmd.ExecuteNonQuery();

                                tr.Commit();
                            }
                        }
                    }
                }
            }

        }

        public void ExecuteNonQueryUsingQuery(string cmdText)
        {
            lock (sync)
            {
                using (DbConnection con = GetConnection())
                {
                    con.Open();
                    using (DbTransaction tr = con.BeginTransaction())
                    {
                        using (DbCommand cmd = con.CreateCommand())
                        {
                            if (cmd.ExNotNull())
                            {
                                cmd.CommandText = cmdText;
                                cmd.CommandType = CommandType.Text;
                                cmd.ExecuteScalar();

                                tr.Commit();
                            }
                        }
                    }
                }
            }

        }


        public object ExecuteScalar(string storedProcedure, List<DbParameter> paramArray)
        {
            object rtn = null;
            lock (sync)
            {
                using (DbConnection con = GetConnection())
                {
                    con.Open();
                    using (DbTransaction tr = con.BeginTransaction())
                    {
                        using (DbCommand cmd = con.CreateCommand())
                        {
                            if (cmd.ExNotNull())
                            {
                                cmd.CommandText = storedProcedure;
                                cmd.CommandType = CommandType.StoredProcedure;

                                if (paramArray.ExNotNull())
                                {
                                    foreach (NpgsqlParameter param in paramArray)
                                    {
                                        cmd.Parameters.Add(param);
                                    }
                                }

                                rtn = cmd.ExecuteScalar();

                                tr.Commit();
                            }
                        }
                    }
                }
            }
            return rtn;
        }

        public object ExecuteScalarUsingQuery(string cmdText)
        {
            object rtn = null;
            lock (sync)
            {
                using (DbConnection con = GetConnection())
                {
                    con.Open();
                    using (DbTransaction tr = con.BeginTransaction())
                    {
                        using (DbCommand cmd = con.CreateCommand())
                        {
                            if (cmd.ExNotNull())
                            {
                                cmd.CommandText = cmdText;
                                cmd.CommandType = CommandType.Text;
                                rtn = cmd.ExecuteScalar();

                                tr.Commit();
                            }
                        }
                    }
                }
            }
            return rtn;
        }

    }

    //public BaseDBAccess GetFactory(DBTYPE type)
    //{
    //    return null;
    //    //if (type == DBTYPE.POSTGRESQL)
    //    //{
    //    //    using (Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(connStr))
    //    //    {
    //    //        con.Open();
    //    //    }

    //    //}
    //    //else if (type == DBTYPE.MSSQL)
    //    //{
    //    //    connStr = string.Format("Server={0};Database={1};User Id={2};Password={3}", info.Server, info.DatabaseName, info.UserID, info.Password);
    //    //    using (SqlConnection con = new SqlConnection(connStr))
    //    //    {
    //    //        con.Open();
    //    //    }
    //    //}
    //}
}
