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
using FinTrak.Transaction;

namespace FinTrak_WP.View
{
    public partial class AssetDetailPage : PhoneApplicationPage
    {
        public AssetModel Asset { get; set; }

        public AssetDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("assetId"))
            {
                uint assetId = uint.Parse(NavigationContext.QueryString["assetId"]);
                Asset = App.Storage.Assets.Where(_asset => _asset.Id == assetId).First();
            }

            if (Asset == null)
            {
                MessageBox.Show("The asset you asked for sadly had not been found. Something went terribly wrong", "Serious trouble :(", MessageBoxButton.OK);
                NavigationService.GoBack();
            }

            DataContext = Asset;
        }

        private void edit_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AssetEditPage.xaml?assetId=" + Asset.Id, UriKind.RelativeOrAbsolute));
            DataContext = Asset;
        }

        private void delete_Click(object sender, EventArgs e)
        {

        }
    }
}