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
    public partial class TransactionsView : UserControl
    {
        public TransactionsView()
        {
            InitializeComponent();
        }

        private void TransactionView_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPage.Navigator.Navigate(new Uri("/View/TransactionDetailPage.xaml?transactionId=" + ((TransactionModel)((TransactionView)sender).DataContext).Id, UriKind.RelativeOrAbsolute));
        }
    }
}
