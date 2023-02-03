using EEH.DB.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EEH.DB.DA
{
    public class MSSQLAccess : BaseDBAccess
    {

        public MSSQLAccess(DBInfo info) : base(string.Format("Server={0};Database={1};User Id={2};Password={3}", info.Server, info.DatabaseName, info.UserID, info.Password))
        {

        }
        public MSSQLAccess(string connectionString) : base(connectionString)
        {

        }

        protected override DbDataAdapter GetAdapter(DbCommand cmd)
        {
            return new SqlDataAdapter(cmd as SqlCommand);
        }

        protected override DbCommand GetCommand(string txt, DbConnection con)
        {
            return new SqlCommand(txt, con as SqlConnection);
        }

        protected override DbConnection GetConnection()
        {
            return new SqlConnection(ConnectString);
        }
    }
}

