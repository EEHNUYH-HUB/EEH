using EEH.WEB.Parameters;
using SmartSql;
using SmartSql.DbSession;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEH.WEB.Common
{
    public class SmartSqlMapper
    {
        public IDbSessionFactory SqlContext { get; set; }

        static SmartSqlMapper instance; 
        public static SmartSqlMapper Instance { get { return instance; } }

        public SmartSqlMapper(IDbSessionFactory sqlContext)
        {
            this.SqlContext = sqlContext;
            instance = this;
        }

        #region RequestParameters Execute
        public DataSet ExecuteDataSet(SmartSqlParameter paramters)
        {
            DataSet ds = null;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                ds = dbsession.GetDataSet(context);
            }
            return ds;
        }
        public Task<DataSet> ExecuteDataSetAsync(SmartSqlParameter paramters)
        {
            Task<DataSet> ds = null;

            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {

                ds = dbsession.GetDataSetAsync(context);
            }
            return ds;
        }
        public DataTable ExecuteDataTable(SmartSqlParameter paramters)
        {
            DataTable dataTable = null;

            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {

                dataTable = dbsession.GetDataTable(context);
            }
            return dataTable;
        }

        public Task<DataTable> ExecuteDataTableAsync(SmartSqlParameter paramters)
        {
            Task<DataTable> dataTable = null;

            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                dataTable = dbsession.GetDataTableAsync(context);
            }
            return dataTable;
        }

        public Task<int> ExecuteAsync(SmartSqlParameter paramters)
        {
            Task<int> result = null;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.ExecuteAsync(context);
            }
            return result;
        }

        public int Execute(SmartSqlParameter paramters)
        {
            int result = 0;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.Execute(context);
            }
            return result;
        }

        public TResult ExecuteScalar<TResult>(SmartSqlParameter paramters)
        {
            TResult result = default;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.ExecuteScalar<TResult>(context);
            }
            return result;
        }

        public Task<TResult> ExecuteScalarAsync<TResult>(SmartSqlParameter paramters)
        {
            Task<TResult> result = null;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {

                result = dbsession.ExecuteScalarAsync<TResult>(context);
            }
            return result;
        }
        #endregion

        #region Entitiy Execute
        public RequestContext GetContext(string scope, string statementId, IDictionary<string, object> param)
        {
            SmartSqlParameter rstContext = new SmartSqlParameter();
            rstContext.Scope = scope;
            rstContext.StatementId = statementId;
            rstContext.Parameters = param;
            return rstContext.ConvertRequestContext();
        }

        public IList<T> Query<T>(SmartSqlParameter paramters)
        {
            IList<T> t = null;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                t = dbsession.Query<T>(context);
            }
            return t;
        }

        public T QuerySingle<T>(SmartSqlParameter paramters)
        {
            T t = default;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                t = dbsession.QuerySingle<T>(context);
            }
            return t;
        }
        public IDictionary<string, object> QuerySingleDictionary(SmartSqlParameter paramters)
        {
            IDictionary<string, object> querySingle = null;
            RequestContext context = paramters.ConvertRequestContext();
            using (var dbsession = this.SqlContext.Open())
            {
                querySingle = dbsession.QuerySingleDictionary(context);
            }
            return querySingle;
        }

        public TEntity GetById<TEntity, TPrimaryKey>(TPrimaryKey primaryKey)
        {
            TEntity t = default;

            using (var dbsession = this.SqlContext.Open())
            {
                t = dbsession.GetById<TEntity, TPrimaryKey>(primaryKey);
            }
            return t;
        }

        public int Insert<T>(T enitity) where T : class
        {
            int result = 0;
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.Insert<T>(enitity);
            }
            return result;
        }

        public int Update<T>(T enitity) where T : class
        {
            int result = 0;
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.Update<T>(enitity);
            }
            return result;
        }

        public int DyUpdate<T>(object enitity)
        {
            int result = 0;
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.DyUpdate<T>(enitity);
            }
            return result;
        }

        public int DeleteById<TEntity, TPrimaryKey>(TPrimaryKey primaryKey)
        {
            int result = 0;
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.DeleteById<TEntity, TPrimaryKey>(primaryKey);
            }
            return result;
        }


        public int DeleteMany<TEntity, TPrimaryKey>(IEnumerable<TPrimaryKey> primaryKey)
        {
            int result = 0;
            using (var dbsession = this.SqlContext.Open())
            {
                result = dbsession.DeleteMany<TEntity, TPrimaryKey>(primaryKey);
            }
            return result;
        }
        #endregion

    }
}
