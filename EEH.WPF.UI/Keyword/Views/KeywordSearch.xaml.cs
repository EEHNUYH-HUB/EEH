using EEH.WPF.UI.Keyword;
using EEH.WPF.UI.Keyword.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EEH.WPF.UI.Keyword.Views
{
    /// <summary>
    /// KeywordSearch.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeywordSearch : Page
    {
        public KeywordSearch()
        {
            InitializeComponent();
            this.DataContext = new KeywordSearchViewModel();
        }

       
    }
}
