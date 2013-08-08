using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using FinTrak.Asset;

namespace FinTrak_WP.View
{
    public partial class AssetsView : UserControl
    {
        public AssetsView()
        {
            InitializeComponent();
        }

        private void AssetView_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPage.Navigator.Navigate(new Uri("/View/AssetDetailPage.xaml?assetId=" + ((AssetModel)((AssetView)sender).DataContext).Id, UriKind.RelativeOrAbsolute));
        }
    }
}
