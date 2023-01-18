using EEH.WEB.Auth;
using EEH.WEB.Common;
using EEH.WEB.Parameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEH.WEB.Controllers
{
    public class SmartSqlController : DynamicController
    {
        SmartSqlMapper mapper;
        public SmartSqlController(SmartSqlMapper mapper, IAuthenticator ctx) : base(ctx)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// 조회 쿼리를 실행하며 Ado.net의 ExecuteDataSet과 동일한 기능을 수행합니다.
        /// </summary>
        /// <param name="requestParamters"></param>
        /// <returns></returns>
        [HttpPost]
        public object ExecuteDataSet([FromBody] SmartSqlParameter requestParamters)
        {
            return mapper.ExecuteDataSet(requestParamters);

        }
        /// <summary>
        /// 조회 쿼리를 실행하며 Ado.net의 ExecuteDataTable과 동일한 기능을 수행합니다.
        /// </summary>
        /// <param name="requestParamters"></param>
        /// <returns></returns>
        [HttpPost]
        public object ExecuteDataTable([FromBody] SmartSqlParameter requestParamters)
        {
            return mapper.ExecuteDataTable(requestParamters);
        }
        /// <summary>
        /// 쿼리를 실행하여 단일 값을 반환합니다. Ado.net의 ExecuteScalar와 동일한 기능을 수행합니다.
        /// </summary>
        /// <param name="requestParamters"></param>
        /// <returns></returns>
        [HttpPost]
        public object ExecuteScalar([FromBody] SmartSqlParameter requestParamters)
        {
            return mapper.ExecuteScalar<object>(requestParamters);

        }
        /// <summary>
        /// 단일 C,R,U를 실행하며 영행받은 행의 수슬 반환합니다.  Ado.net의 ExecuteNonQuery와 동일한 기능을 수행합니다.
        /// </summary>
        /// <param name="requestParamters"></param>
        /// <returns></returns>
        [HttpPost]
        public object ExecuteNonQuery([FromBody] SmartSqlParameter requestParamters)
        {
            return mapper.Execute(requestParamters);
        }
    }
}
