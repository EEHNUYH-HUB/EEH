using EEH.DB.DA;
using EEH.DB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EEH.DB
{
    public class DbHandler
    {
        public static BaseDBAccess GetInstance(DBInfo info) 
        {
            if (info.DBType == DBTYPE.POSTGRESQL)
            {
                return new PostgreSQLAccess(info);

            }
            else if (info.DBType == DBTYPE.MSSQL)
            {
                return new MSSQLAccess(info);
            }

            else
            {
                return null;
            }
        }
        
        public bool ConnectionTest(DBInfo info)
        {
            BaseDBAccess da = GetInstance(info);
            if(da.ExNotNull())
                return da.ConnectionText();

            return false;
        }
        

        public List<string> GetTables(DBInfo info)
        {
            List<string > rtn = new List<string>(); 
            BaseDBAccess da = GetInstance(info);
            
            if (da.ExNotNull())
            {
                DataTable dt = null;
                if(info.DBType == DBTYPE.MSSQL)
                    dt = da.FillDataTableUsingQuery("SELECT * FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME");
                else if(info.DBType == DBTYPE.POSTGRESQL)
                    dt = da.FillDataTableUsingQuery("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'public' ORDER BY TABLE_NAME");

                if (dt.ExNotNull())
                {
                    using (dt)
                    {
                        string colName = string.Empty;
                        foreach(DataColumn col in dt.Columns) 
                        {
                            string lower = col.ColumnName.ExToLower();
                            
                            if(lower == "table_name")
                            {
                                colName = col.ColumnName;
                                break;
                            }
                        }


                        foreach(DataRow row in dt.Rows)
                        {
                            string tableName = row[colName].ExToString();
                            if(tableName.ExNotNullOrEmpty())
                                rtn.Add(tableName);
                        }
                    }
                }
            }

            return rtn;
        }

        public List<string> GetColumns (DBInfo info ,string tableName)
        {
            List<string> rtn = new List<string>();
            BaseDBAccess da = GetInstance(info);

            if (da.ExNotNull())
            {
                DataTable dt = null;
                if (info.DBType == DBTYPE.MSSQL || info.DBType == DBTYPE.POSTGRESQL)
                    dt = da.FillDataTableUsingQuery(string.Format( "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' ORDER BY ORDINAL_POSITION  ", tableName));
                
                if (dt.ExNotNull())
                {
                    using (dt)
                    {
                        string colName = string.Empty;
                        foreach (DataColumn col in dt.Columns)
                        {
                            string lower = col.ColumnName.ExToLower();

                            if (lower == "column_name")
                            {
                                colName = col.ColumnName;
                                break;
                            }
                        }


                        foreach (DataRow row in dt.Rows)
                        {
                            string strValue = row[colName].ExToString();
                            if (strValue.ExNotNullOrEmpty())
                                rtn.Add(strValue);
                        }
                    }
                }
            }

            return rtn;
        }
    }
}