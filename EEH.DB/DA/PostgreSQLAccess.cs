using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEH.DB.Models;
using Npgsql;

namespace EEH.DB.DA
{
    public class PostgreSQLAccess : BaseDBAccess
    {

        public PostgreSQLAccess(DBInfo info) : base(string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}", info.Server, info.Port, info.DatabaseName, info.UserID, info.Password))
        {

        }
        public PostgreSQLAccess(string connectionString) : base(connectionString)
        {

        }

        protected override DbDataAdapter GetAdapter(DbCommand cmd)
        {
            return new NpgsqlDataAdapter(cmd as NpgsqlCommand);
        }

        protected override DbCommand GetCommand(string txt, DbConnection con)
        {
            return new NpgsqlCommand(txt, con as NpgsqlConnection);
        }

        protected override DbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectString);
        }
    }
}
