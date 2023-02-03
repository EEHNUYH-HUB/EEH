using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EEH.DB.Models
{
    public class DBInfo
    {
        [JsonPropertyName("DBType")]
        public DBTYPE DBType { get; set; }
        [JsonPropertyName("Server")]
        public string Server { get; set; }
        [JsonPropertyName("Port")]
        public int Port { get; set; }
        [JsonPropertyName("DatabaseName")]
        public string DatabaseName { get; set; }
        [JsonPropertyName("UserID")]
        public string UserID { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }

    }
}
