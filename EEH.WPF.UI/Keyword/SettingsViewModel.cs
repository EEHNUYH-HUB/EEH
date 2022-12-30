using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEH.WPF.UI.Keyword
{
    public class SettingsViewModel : BaseViewModel
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
            SaveCommand = new BaseCommand();
            SaveCommand.ExecuteHandler = (param) =>
            {
                
                EEH.APIInfoSettings.Default.KeywordSearchNaverCustomerID = NaverCustomerID;
                EEH.APIInfoSettings.Default.KeywordSearchNaverApiKey = NaverApiKey;
                EEH.APIInfoSettings.Default.KeywordSearchNaverSecret = NaverSecretKey;

                EEH.APIInfoSettings.Default.OpenApiNaverClientID = OpenApiNaverClientID;
                EEH.APIInfoSettings.Default.OpenApiNaverSecret = OpenApiNaverSecretKey;

                EEH.APIInfoSettings.Default.OpenApi11StKey = OpenApi11stKey;
                EEH.APIInfoSettings.Default.Save();


            };
        }

        public void Init()
        {
           NaverCustomerID = EEH.APIInfoSettings.Default.KeywordSearchNaverCustomerID;
           NaverApiKey = EEH.APIInfoSettings.Default.KeywordSearchNaverApiKey;
           NaverSecretKey = EEH.APIInfoSettings.Default.KeywordSearchNaverSecret;

           OpenApiNaverClientID = EEH.APIInfoSettings.Default.OpenApiNaverClientID;
           OpenApiNaverSecretKey = EEH.APIInfoSettings.Default.OpenApiNaverSecret;

           OpenApi11stKey = EEH.APIInfoSettings.Default.OpenApi11StKey;
        }
    }
}
