using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEH.WPF.UI.Keyword
{
    public class SettingsViewModel :BaseViewModel
    {
        
        public string NaverCustomerID { get; set; }
        public string NaverApiKey { get; set; }
        public string NaverSecretKey { get; set; }
        public string OpenApiNaverClientID { get; set; }
        public string OpenApiNaverSecretKey { get; set; }
        public string OpenApi11stKey { get; set; }

        public BaseCommand SaveCommand { get; set; }

        public SettingsViewModel() 
        {
            //ddd
            //dr0002
            SaveCommand = new BaseCommand();
            SaveCommand.ExecuteHandler = (param) => {
               // EEH.Properties.APIInfoSettings.Default.KeywordSearchNaverCustomerID = NaverCustomerID;
               // EEH.Properties.APIInfoSettings.Default.KeywordSearchNaverApiKey = NaverApiKey;
               // EEH.Properties.APIInfoSettings.Default.KeywordSearchNaverSecret = NaverSecretKey;

               // EEH.Properties.APIInfoSettings.Default.OpenApiNaverClientID = OpenApiNaverClientID;
               //EEH.Properties.APIInfoSettings.Default.OpenApiNaverSecret = OpenApiNaverSecretKey;

               //EEH.Properties.APIInfoSettings.Default.OpenApi11StKey = OpenApi11stKey;
               // EEH.Properties.APIInfoSettings.Default.
                
            };
        }

        public void Init()
        {
            //NaverCustomerID = EEH.Properties.APIInfoSettings.Default.KeywordSearchNaverCustomerID;
            //NaverApiKey = EEH.Properties.APIInfoSettings.Default.KeywordSearchNaverApiKey;
            //NaverSecretKey = EEH.Properties.APIInfoSettings.Default.KeywordSearchNaverSecret;

            //OpenApiNaverClientID = EEH.Properties.APIInfoSettings.Default.OpenApiNaverClientID;
            //OpenApiNaverSecretKey = EEH.Properties.APIInfoSettings.Default.OpenApiNaverSecret;

            //OpenApi11stKey = EEH.Properties.APIInfoSettings.Default.OpenApi11StKey;
        }
    }
}
