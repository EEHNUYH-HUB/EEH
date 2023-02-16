using EEH.DB.DA;
using EEH.DB.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

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

        public DataTable GetColumns (DBInfo info ,string tableName)
        {
            
            BaseDBAccess da = GetInstance(info);

            if (da.ExNotNull())
            {
                
                if (info.DBType == DBTYPE.MSSQL || info.DBType == DBTYPE.POSTGRESQL)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(" SELECT 			A.COLUMN_NAME  ");
                    sb.AppendLine("     ,			B.FOREIGN_TABLE_NAME  ");
                    sb.AppendLine("     ,			B.FOREIGN_COLUMN_NAME  ");
                    sb.AppendLine(" FROM 			INFORMATION_SCHEMA.COLUMNS A  ");
                    sb.AppendLine(" LEFT OUTER JOIN (  ");
                    sb.AppendLine("                     SELECT 	DISTINCT ");
                    sb.AppendLine(" 		                    CCU.TABLE_NAME      AS FOREIGN_TABLE_NAME ");
                    sb.AppendLine(" 	                     , 	CCU.COLUMN_NAME     AS FOREIGN_COLUMN_NAME ");
                    sb.AppendLine(" 	                     , 	KCU.TABLE_NAME      AS TABLE_NAME ");
                    sb.AppendLine(" 	                     ,	KCU.COLUMN_NAME     AS COLUMN_NAME ");
                    sb.AppendLine("                     FROM 	INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC ");
                    sb.AppendLine("                     JOIN	INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU ON TC.CONSTRAINT_NAME = KCU.CONSTRAINT_NAME ");
                    sb.AppendLine("                     JOIN 	INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU ON CCU.CONSTRAINT_NAME = TC.CONSTRAINT_NAME ");
                    sb.AppendLine("                     WHERE 	TC.constraint_type = 'FOREIGN KEY' ");
                    sb.AppendLine("                 ) B ON A.TABLE_NAME  = B.TABLE_NAME AND A.COLUMN_NAME = B.COLUMN_NAME ");
                    sb.AppendLine($" WHERE A.TABLE_NAME = '{tableName}' ");
                    return  da.FillDataTableUsingQuery(sb.ToString());
                }
            }

            return null;
        }


        public DataTable Build(DBInfo info, string query)
        {
            BaseDBAccess da = GetInstance(info);

            if (da.ExNotNull())
            {
                return da.FillDataTableUsingQuery(query);
            }

            return null;
        }
    }
}