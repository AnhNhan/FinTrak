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

        private FinTrakDatabaseRepository dbRepo = new FinTrakDatabaseRepository();

        public static AssetCollection Assets { get; private set; }
        public static TransactionCollection Transactions { get; private set; }
        public static SubjectCollection Subjects { get; private set; }

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            AppBarButtons = new List<ApplicationBarIconButton>();
            if (!_dataLoaded)
            {
                this.Loaded += InitializeData;
            }
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
        }

        void InitializeAssets()
        {
            Assets = new AssetCollection(dbRepo.LoadAssets());

            Assets.Add(new AssetModel
            {
                Title = "Dud",
                TypeId = AssetType.Cash,
            });

            Assets.Add(new AssetModel
            {
                Title = "Dede",
                TypeId = AssetType.Account,
            });

            Assets.Add(new AssetModel
            {
                Title = "Ha ho",
                TypeId = AssetType.CashSavings,
            });

            Assets.Add(new AssetModel
            {
                Title = "Dörd",
                TypeId = AssetType.CashSavings,
            });

            Assets.Add(new AssetModel
            {
                Title = "Hsd",
                TypeId = AssetType.Prepaid,
            });

            var assetView = new View.AssetsView();
            assetView.DataContext = Assets;
            uiRoot_pivot_assets.Content = assetView;

            Assets.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveAssets(Assets.ToList());
            };
        }

        void InitializeSubjects()
        {
            Subjects = new SubjectCollection(dbRepo.LoadSubjects());

            Subjects.Add(new SubjectModel
            {
                Name = "Bank of America",
                Label = "Banking Corp.",
            });
            Subjects.Add(new SubjectModel
            {
                Name = "FedEx",
                Label = "Postal Corp.",
            });

            //var transactionsView = new View.TransactionsView();
            //transactionsView.DataContext = Subjects;
            //uiRoot_pivot_transactions.Content = transactionsView;

            Subjects.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveSubjects(Subjects.ToList());
            };
        }

        void InitializeTransactions()
        {
            Transactions = new TransactionCollection(dbRepo.LoadTransactions());

            Random random = new Random();
            AddTransaction(Assets.ElementAt(1), Assets.ElementAt(2), random);
            AddTransaction(Assets.ElementAt(2), Assets.ElementAt(3), random);
            AddTransaction(Assets.ElementAt(3), Assets.ElementAt(2), random);
            AddTransaction(Assets.ElementAt(1), Assets.ElementAt(4), random);
            AddTransaction(Assets.ElementAt(2), Assets.ElementAt(0), random);

            var transactionsView = new View.TransactionsView();
            transactionsView.DataContext = Transactions;
            uiRoot_pivot_transactions.Content = transactionsView;

            Transactions.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveTransactions(Transactions.ToList());
            };
        }

        void AddTransaction(AssetModel origin, AssetModel target, Random random)
        {
            TransactionModel xact = new TransactionModel
            {
                Label = string.Format("{0}{1}{2}", (char)random.Next(65, 90), (char)random.Next(65, 130), (char)random.Next(65, 130)),
                Amount = (float)Math.Round(random.NextDouble() * random.Next(0, 3000), 2),
                TransactionDate = new DateTime(random.Next(2000, 2013), random.Next(1, 12), random.Next(1, 29)),
            };

            origin.AddTransaction(xact, true);
            target.AddTransaction(xact, false);
            Transactions.Add(xact);
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

            if (ApplicationBar.Buttons.Count != 0)
            {
                ApplicationBar.IsVisible = true;
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

        private void clearStorage_Click(object sender, EventArgs e)
        {
            dbRepo.ClearStorage();
            Assets.Clear();
            Transactions.Clear();
            Subjects.Clear();
        }
    }
}