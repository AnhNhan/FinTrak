using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using FinTrak.Transaction;

namespace FinTrak_WP.View
{
    public partial class TransactionDetailPage : PhoneApplicationPage
    {
        public TransactionModel Transaction { get; set; }

        public TransactionDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("transactionId"))
            {
                uint assetId = uint.Parse(NavigationContext.QueryString["transactionId"]);
                Transaction = App.Storage.Transactions.Where(_transaction => _transaction.Id == assetId).First();
            }

            if (Transaction == null)
            {
                MessageBox.Show("The transaction you asked for sadly had not been found. Something went terribly wrong", "Serious trouble :(", MessageBoxButton.OK);
                NavigationService.GoBack();
            }

            DataContext = Transaction;
        }
    }
}