using SmartSql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEH.WEB.Parameters
{
    public class SmartSqlParameter
    {
        /// <summary>
        /// SmartSql Scope 이름
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Scope is Required")]
        public string Scope { get; set; }
        /// <summary>
        /// SmartSql StatementId 이름
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "StatementId is Required")]
        public string StatementId { get; set; }
        /// <summary>
        /// 클라이언트로부터 전달되는 파라미터 정보
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }


        public  dynamic ConvertRequestContext()
        {
            RequestContext context = new RequestContext() { Scope = this.Scope, SqlId = this.StatementId };

            dynamic eo = this.Parameters?.Aggregate(new ExpandoObject() as IDictionary<string, Object>,
                                        (a, p) => { a.Add(p.Key, p.Value); return a; });
            context.Request = eo;
            return context;
        }

        public  dynamic ConvertExpandoObject()
        {
            dynamic eo = Parameters.Aggregate(new ExpandoObject() as IDictionary<string, Object>,
                                       (a, p) => { a.Add(p.Key, p.Value); return a; });
            return eo;
        }
    }
}
