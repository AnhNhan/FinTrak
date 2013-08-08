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

        private List<ApplicationBarIconButton> AppBarButtons;

        private static FinTrakDatabaseRepository dbRepo = new FinTrakDatabaseRepository();

        public static AssetCollection Assets { get; private set; }
        public static TransactionCollection Transactions { get; private set; }
        public static SubjectCollection Subjects { get; private set; }

        private static NavigationService _navigator;
        public static NavigationService Navigator { get { return _navigator; } }

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            AppBarButtons = new List<ApplicationBarIconButton>();
            if (!_dataLoaded)
            {
                this.Loaded += InitializeData;
                this.Loaded += InitializeViews;
                this.Loaded += (s, e) => { _navigator = NavigationService; };
            }
        }

        private void InitializeViews(object sender, RoutedEventArgs e)
        {
            var assetView = new View.AssetsView();
            assetView.DataContext = Assets;
            uiRoot_pivot_assets.Content = assetView;

            var transactionsView = new View.TransactionsView();
            transactionsView.DataContext = Transactions;
            uiRoot_pivot_transactions.Content = transactionsView;

            var subjectView = new View.SubjectsView();
            subjectView.DataContext = Subjects;
            uiRoot_pivot_subjects.Content = subjectView;
        }

        #region data stuff

        async void InitializeData(object sender, RoutedEventArgs e)
        {
            await dbRepo.Initialize();

            InitializeAssets();
            InitializeTransactions();
            InitializeSubjects();

            _dataLoaded = true;
            this.Loaded -= InitializeData;

            // Link assets and transactions with each other
            foreach (AssetModel asset in Assets)
            {
                List<TransactionModel> xacts = Transactions.Where((TransactionModel transaction) =>
                {
                    return (transaction.OriginId == asset.Id && transaction.OriginIsAsset) || (transaction.TargetId == asset.Id && transaction.TargetIsAsset);
                }).ToList();

                foreach (TransactionModel xact in xacts) {
                    asset.AddTransaction(xact, xact.OriginIsAsset);
                }
            }

            foreach (SubjectModel subject in Subjects)
            {
                List<TransactionModel> xacts = Transactions.Where((TransactionModel transaction) =>
                {
                    return (transaction.OriginId == subject.Id && !transaction.OriginIsAsset) || (transaction.TargetId == subject.Id && !transaction.TargetIsAsset);
                }).ToList();

                foreach (TransactionModel xact in xacts)
                {
                    if (!xact.TargetIsAsset)
                    {
                        xact.Target = subject;
                    }
                    if (!xact.OriginIsAsset)
                    {
                        xact.Origin = subject;
                    }
                }
            }
        }

        void InitializeAssets()
        {
            Assets = new AssetCollection(dbRepo.LoadAssets());

            Assets.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveAssets(Assets.ToList());
            };
        }

        void InitializeSubjects()
        {
            Subjects = new SubjectCollection(dbRepo.LoadSubjects());

            Subjects.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveSubjects(Subjects.ToList());
            };
        }

        void InitializeTransactions()
        {
            Transactions = new TransactionCollection(dbRepo.LoadTransactions());

            Transactions.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveTransactions(Transactions.ToList());
            };
        }

        #endregion

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

        private void clearStorage_Click(object sender, EventArgs e)
        {
            dbRepo.ClearStorage();
            Assets.Clear();
            Transactions.Clear();
            Subjects.Clear();
        }

        public static void SaveAll()
        {
            dbRepo.SaveGenerically();
        }
    }
}