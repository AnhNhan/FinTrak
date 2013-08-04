using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FinTrak_WP.Resources;

using FinTrak.Asset;

namespace FinTrak_WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            AddTestData();
        }

        void AddTestData()
        {
            AssetCollection assetCollection = new AssetCollection();

            assetCollection.Add(new AssetModel
            {
                Title = "Dud",
                TypeId = AssetType.Cash,
                CurrentBalance = 24.66f,
            });

            assetCollection.Add(new AssetModel
            {
                Title = "Dede",
                TypeId = AssetType.Account,
                CurrentBalance = -4.37f,
            });

            assetCollection.Add(new AssetModel
            {
                Title = "Ha ho",
                TypeId = AssetType.CashSavings,
                CurrentBalance = 224.98f,
            });

            assetCollection.Add(new AssetModel
            {
                Title = "Dörd",
                TypeId = AssetType.CashSavings,
                CurrentBalance = 25.46f,
            });

            assetCollection.Add(new AssetModel
            {
                Title = "Hsd",
                TypeId = AssetType.Prepaid,
                CurrentBalance = 48.40f,
            });

            var assetView = new View.AssetsView();
            assetView.DataContext = assetCollection;
            // uiRoot_pivot_assets_content.Content = assetView;
            uiRoot_pivot_assets.Content = assetView;
        }

        private void add_Click(object sender, EventArgs e) { }

        private void save_Click(object sender, EventArgs e) { }

        private void clear_Click(object sender, EventArgs e) { }
    }
}