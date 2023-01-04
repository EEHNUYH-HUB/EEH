using EEH.Ecommerce;
using EEH.Keyword.Naver.Models;
using EEH.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEH.WPF.UI.Keyword.ViewModels
{
    public class KeywordSearchViewModel : BaseViewModel
    {
        string progressText;
        public string ProgressText
        {
            get { return progressText; }
            set { progressText = value; OnPropertyCanged("ProgressText"); }
        }
        double progressValue;
        public double ProgressValue
        {
            get { return progressValue; }
            set { progressValue = value; OnPropertyCanged("ProgressValue"); }
        }
        Visibility visibilityProgressBar = Visibility.Hidden;
        public Visibility VisibilityProgressBar
        {
            get { return visibilityProgressBar; }
            set { visibilityProgressBar = value; OnPropertyCanged("VisibilityProgressBar"); }
        }
        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyCanged("SearchText"); }
        }

        ObservableCollection<KeywordsTool> results;
        public ObservableCollection<KeywordsTool> Results
        {
            get { return results; }
            set { results = value; OnPropertyCanged("Results"); }
        }
        KeywordsTool selectedItem;
        public KeywordsTool SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyCanged("selectedItem"); }
        }
        public BaseCommand OnSearchCommand { get; set; }
        public BaseCommand OnSearchEcommerceCommand { get; set; }
        public BaseCommand OnStopCommand { get; set; }
        public BaseCommand OnSaveCommand { get; set; }
        List<IOpenApi> ecommerceApis;
        public KeywordSearchViewModel()
        {
            ecommerceApis = new List<IOpenApi>();
            ecommerceApis.Add(new OpenApiNaver());
            ecommerceApis.Add(new OpenApi11st());
            ecommerceApis.Add(new SearchInterpark());

            Results = new ObservableCollection<KeywordsTool>();
            OnSearchCommand = new BaseCommand();
            OnSearchCommand.ExecuteHandler = async (param) =>
            {

                EEH.Keyword.Naver.NaverAPIClient client = new EEH.Keyword.Naver.NaverAPIClient();
                KeywordsTool result = await client.Search(SearchText);
                Results.Add(result);
                SearchText = string.Empty;
                SelectedItem = result;
                Save();
                OnPropertyCanged("KeywordResult");
            };

            OnSearchEcommerceCommand = new BaseCommand();
            OnSearchEcommerceCommand.ExecuteHandler = (param) =>
            {
                if (SelectedItem.ExNotNull() && ecommerceApis.ExNotNull())
                {
                    VisibilityProgressBar = Visibility.Visible;

                    double totalCnt = 0;
                    double progressCnt = 0;

                    foreach (IOpenApi api in ecommerceApis)
                    {

                        foreach (KeywordModel keyword in SelectedItem.KeywordList)
                        {
                            totalCnt += 1;
                            long cnt = 0;
                            QueueAsync.Instance.AddTask(async () =>
                            {
                                cnt = await api.GetProductTotalCnt(keyword.RelKeyword);
                            }, (r, ErrorContext) =>
                            {
                                if (api.Type == EcommerceType.ETINTERPARK)
                                {
                                    keyword.EcommerceInterparkCnt = cnt;
                                }
                                else if (api.Type == EcommerceType.ET11ST)
                                {
                                    keyword.Ecommerce11stCnt = cnt;
                                }
                                else if (api.Type == EcommerceType.ETNAVER)
                                {
                                    keyword.EcommerceNaverCnt = cnt;
                                }

                                progressCnt += 1;
                                ProgressValue = progressCnt * 100 / totalCnt;

                                ProgressText = string.Format("{0}/{1} {2}%", progressCnt, totalCnt, Math.Round(progressValue, 2));
                                if (totalCnt == progressCnt)
                                {
                                    Save();
                                    HideProgress();
                                }

                            });
                        }


                    }
                }
            };

            OnStopCommand = new BaseCommand();
            OnStopCommand.ExecuteHandler = (param) =>
            {
                QueueAsync.Instance.Stop();
                Save();
                HideProgress();
            };

            Load();
        }
        void HideProgress()
        {
            VisibilityProgressBar = Visibility.Collapsed;
            ProgressValue = 0;
        }
        void Save()
        {
            string strJson = SelectedItem.ExJsonSerializeString();
            strJson.ExSaveForDataFolder("KEYWORD", SelectedItem.Keyword);


        }

        void Load()
        {
            List<string> keywords = "KEYWORD".ExListForDataFolder();

            foreach (string keyword in keywords)
            {
                string strJson = "KEYWORD".ExLoadForDataFolder(keyword);
                KeywordsTool r = strJson.ExDeserializeObject<KeywordsTool>();
                if (r.ExNotNull())
                    Results.Add(r);
            }
            if (Results.Count > 0)
            {
                SelectedItem = Results[0];
            }

        }
    }
}
