using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEH.WPF.UI.Keyword.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        string naverCustomerID;
        public string NaverCustomerID
        {
            get
            {
                return naverCustomerID;
            }
            set
            {
                naverCustomerID = value;
                OnPropertyCanged("NaverCustomerID");
            }
        }

        string naverApiKey;
        public string NaverApiKey
        {
            get
            {
                return naverApiKey;
            }
            set
            {
                naverApiKey = value;
                OnPropertyCanged("NaverApiKey");
            }
        }
        string naverSecretKey;
        public string NaverSecretKey
        {
            get
            {
                return naverSecretKey;
            }
            set
            {
                naverSecretKey = value;
                OnPropertyCanged("NaverSecretKey");
            }
        }
        string openApiNaverClientID;
        public string OpenApiNaverClientID
        {
            get
            {
                return openApiNaverClientID;
            }
            set
            {
                openApiNaverClientID = value;
                OnPropertyCanged("OpenApiNaverClientID");
            }
        }
        string openApiNaverSecretKey;
        public string OpenApiNaverSecretKey
        {
            get
            {
                return openApiNaverSecretKey;
            }
            set
            {
                openApiNaverSecretKey = value;
                OnPropertyCanged("OpenApiNaverSecretKey");
            }
        }
        string openApi11stKey;
        public string OpenApi11stKey
        {
            get
            {
                return openApi11stKey;
            }
            set
            {
                openApi11stKey = value;
                OnPropertyCanged("OpenApi11stKey");
            }
        }

        public BaseCommand SaveCommand { get; set; }

        public SettingsViewModel()
        {
            SaveCommand = new BaseCommand();
            SaveCommand.ExecuteHandler = (param) =>
            {

                APIInfoSettings.Default.KeywordSearchNaverCustomerID = NaverCustomerID;
                APIInfoSettings.Default.KeywordSearchNaverApiKey = NaverApiKey;
                APIInfoSettings.Default.KeywordSearchNaverSecret = NaverSecretKey;

                APIInfoSettings.Default.OpenApiNaverClientID = OpenApiNaverClientID;
                APIInfoSettings.Default.OpenApiNaverSecret = OpenApiNaverSecretKey;

                APIInfoSettings.Default.OpenApi11StKey = OpenApi11stKey;
                APIInfoSettings.Default.Save();


            };
            Init();
        }

        public void Init()
        {

            NaverCustomerID = APIInfoSettings.Default.KeywordSearchNaverCustomerID;
            NaverApiKey = APIInfoSettings.Default.KeywordSearchNaverApiKey;
            NaverSecretKey = APIInfoSettings.Default.KeywordSearchNaverSecret;

            OpenApiNaverClientID = APIInfoSettings.Default.OpenApiNaverClientID;
            OpenApiNaverSecretKey = APIInfoSettings.Default.OpenApiNaverSecret;

            OpenApi11stKey = APIInfoSettings.Default.OpenApi11StKey;
        }
    }
}
