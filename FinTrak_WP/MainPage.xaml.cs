using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FinTrak_WP.Resources;

using FinTrak;
using FinTrak.Asset;
using FinTrak.Budget;
using FinTrak.Subject;
using FinTrak.Transaction;

namespace FinTrak_WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static bool _dataLoaded = false;

        private static NavigationService _navigator;
        public static NavigationService Navigator { get { return _navigator; } }

        StorageManager Storage;

        private List<ApplicationBarIconButton> AppBarButtons;

        // Konstruktor
        public MainPage()
        {
            Storage = App.Storage;
            InitializeComponent();
            AppBarButtons = new List<ApplicationBarIconButton>();
            if (!_dataLoaded)
            {
                this.Loaded += InitializeViews;
                this.Loaded += (s, e) => { _navigator = NavigationService; };
            }
        }

        private void InitializeViews(object sender, RoutedEventArgs e)
        {
            var assetView = new View.AssetsView();
            assetView.DataContext = Storage.Assets;
            uiRoot_pivot_assets.Content = assetView;

            var transactionsView = new View.TransactionsView();
            transactionsView.DataContext = Storage.Transactions;
            uiRoot_pivot_transactions.Content = transactionsView;

            var subjectView = new View.SubjectsView();
            subjectView.DataContext = Storage.Subjects;
            uiRoot_pivot_subjects.Content = subjectView;
        }

        private void clearStorage_Click(object sender, EventArgs e)
        {
            Storage.ClearAll();
        }

        private void AddAsset_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AssetEditPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AddTransaction_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/TransactionEditPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AddSubject_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/SubjectEditPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void uiRoot_pivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item == uiRoot_pivot_assets)
            {
                var button = (ApplicationBarIconButton)Resources["assetsAppBarButton"];
                AppBarButtons.Add(button);
                ApplicationBar.Buttons.Add(button);
            }
            else if (e.Item == uiRoot_pivot_transactions)
            {
                var button = (ApplicationBarIconButton)Resources["transactionsAppBarButton"];
                AppBarButtons.Add(button);
                ApplicationBar.Buttons.Add(button);
            }
            else if (e.Item == uiRoot_pivot_subjects)
            {
                var button = (ApplicationBarIconButton)Resources["subjectsAppBarButton"];
                AppBarButtons.Add(button);
                ApplicationBar.Buttons.Add(button);
            }

            if (ApplicationBar.Buttons.Count != 0)
            {
                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Opacity = 1.0;
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
                ApplicationBar.Mode = ApplicationBarMode.Minimized;
                ApplicationBar.Opacity = 0.2;
            }
        }
    }
}