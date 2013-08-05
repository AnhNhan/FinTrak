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
using FinTrak.Budget;
using FinTrak.Subject;
using FinTrak.Transaction;

namespace FinTrak_WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<ApplicationBarIconButton> AppBarButtons;

        public static AssetCollection Assets { get; private set; }
        public static TransactionCollection Transactions { get; private set; }

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            AppBarButtons = new List<ApplicationBarIconButton>();
            Assets = new AssetCollection();
            Transactions = new TransactionCollection();
            AddTestData();
        }

        void AddTestData()
        {
            Assets.Add(new AssetModel
            {
                Title = "Dud",
                TypeId = AssetType.Cash,
                CurrentBalance = 24.66f,
            });

            Assets.Add(new AssetModel
            {
                Title = "Dede",
                TypeId = AssetType.Account,
                CurrentBalance = -4.37f,
            });

            Assets.Add(new AssetModel
            {
                Title = "Ha ho",
                TypeId = AssetType.CashSavings,
                CurrentBalance = 224.98f,
            });

            Assets.Add(new AssetModel
            {
                Title = "Dörd",
                TypeId = AssetType.CashSavings,
                CurrentBalance = 25.46f,
            });

            Assets.Add(new AssetModel
            {
                Title = "Hsd",
                TypeId = AssetType.Prepaid,
                CurrentBalance = 48.40f,
            });

            var assetView = new View.AssetsView();
            assetView.DataContext = Assets;
            // uiRoot_pivot_assets_content.Content = assetView;
            uiRoot_pivot_assets.Content = assetView;
        }

        private void add_Click(object sender, EventArgs e) { }

        private void save_Click(object sender, EventArgs e) { }

        private void clear_Click(object sender, EventArgs e) { }

        private void AddAsset_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AssetEditPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void uiRoot_pivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item == uiRoot_pivot_assets)
            {
                var button = (ApplicationBarIconButton)Resources["assetsAppBarButton"];
                AppBarButtons.Add(button);
                ApplicationBar.Buttons.Add(button);

                if (ApplicationBar.Buttons.Count != 0)
                {
                    ApplicationBar.IsVisible = true;
                }
            }
        }

        private void uiRoot_pivot_UnloadingPivotItem(object sender, PivotItemEventArgs e)
        {
            List<ApplicationBarIconButton> buttonsForDeletion = new List<ApplicationBarIconButton>();
            foreach (var button in AppBarButtons)
            {
                ApplicationBar.Buttons.Remove(button);
                buttonsForDeletion.Add(button);
            }

            foreach (var button in buttonsForDeletion)
            {
                AppBarButtons.Remove(button);
            }

            if (ApplicationBar.Buttons.Count == 0)
            {
                ApplicationBar.IsVisible = false;
            }
        }
    }
}