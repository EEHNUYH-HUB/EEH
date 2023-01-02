using EEH.Ecommerce;
using EEH.Keyword.Naver.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEH.WPF.UI.Keyword
{
    public class KeywordSearchViewModel: BaseViewModel
    {
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

        List<IOpenApi> ecommerceApis;
        public KeywordSearchViewModel()
        {
            ecommerceApis = new List<IOpenApi>();
            //ecommerceApis.Add(new OpenApi11st());
            ecommerceApis.Add(new OpenApiNaver());
            //ecommerceApis.Add(new SearchInterpark());

            Results = new ObservableCollection<KeywordsTool>();
            OnSearchCommand = new BaseCommand();
            OnSearchCommand.ExecuteHandler =async (param) => {

                EEH.Keyword.Naver.NaverAPIClient client = new EEH.Keyword.Naver.NaverAPIClient();
                KeywordsTool result = await client.Search(SearchText);
                Results.Add(result);
                SearchText = string.Empty;
                SelectedItem = result;
                OnPropertyCanged("KeywordResult");
            };

            OnSearchEcommerceCommand = new BaseCommand();
            OnSearchEcommerceCommand.ExecuteHandler = async (param) =>
            {
                if (SelectedItem.ExNotNull() && ecommerceApis.ExNotNull())
                {
                    foreach(OpenApiNaver api in ecommerceApis)
                    {
                        foreach(KeywordModel keyword in SelectedItem.KeywordList)
                        {
                            keyword.EcommerceNaverCnt =await api.GetProductTotalCnt(keyword.RelKeyword);
                        }
                    }
                }
            };
        }
    }
}
