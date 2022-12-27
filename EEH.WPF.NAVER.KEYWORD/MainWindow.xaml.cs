using EEH.Keyword.Naver;
using EEH.Keyword.Naver.Models;
using EEH.OpenMarket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EEH.WPF.NAVER.KEYWORD
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private NaverAPIClient client = new NaverAPIClient();
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string keyword = "자전거";
            KeywordsTool result = await client.Search(keyword);


            IOpenApi openApiNaver = new OpenApiNaver();
            IOpenApi openApi11st = new OpenApi11st();
            IOpenApi interpark = new SearchInterpark();



           
            DateTime dt = DateTime.Now;
            int r1 = await openApi11st.GetProductTotalCnt(keyword);
            System.Diagnostics.Debug.WriteLine("11번가:"+r1+":"+DateTime.Now.Subtract(dt).TotalMilliseconds);
            dt = DateTime.Now;
            int r2 = await openApiNaver.GetProductTotalCnt(keyword);

            System.Diagnostics.Debug.WriteLine("네이버:"+r2 + ":" + DateTime.Now.Subtract(dt).TotalMilliseconds);
            dt = DateTime.Now;
            int r3 = await interpark.GetProductTotalCnt(keyword);

            System.Diagnostics.Debug.WriteLine("인터파크:"+r3 + ":" + DateTime.Now.Subtract(dt).TotalMilliseconds);
            dt = DateTime.Now;

        }

    }
}
