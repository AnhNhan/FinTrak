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
using FinTrak.Budget;
using FinTrak.Subject;
using FinTrak.Transaction;

namespace FinTrak_WP.View
{
    public partial class TransactionEditPage : PhoneApplicationPage
    {
        bool _didNotExist = true;
        TransactionModel Transaction;

        public TransactionEditPage()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (_didNotExist)
            {
                Transaction = new TransactionModel();
            }

            string label = xactLabel.Text.Trim();
            if (label.Length == 0)
            {
                MessageBox.Show("You have to provide a label for the asset", "No label", MessageBoxButton.OK);
                return;
            }

            string amountText = xactAmount.Text.Trim();
            if (amountText.Length == 0)
            {
                MessageBox.Show("Please also provide an amount :P");
                return;
            }

            float amount = float.Parse(amountText);
            amount = (float)Math.Round(amount, 2);
            if (amount < 0.01f)
            {
                MessageBox.Show("I'm sorry, but you can't use 0.00 as an amount");
                return;
            }

            ITransactionTarget origin = xactFrom.SelectedItem;
            ITransactionTarget target = xactTo.SelectedItem;

            Type originType = origin.GetType();
            Type targetType = target.GetType();
            Type subjectType = typeof(SubjectModel);

            if (origin == target)
            {
                MessageBox.Show("I'm sorry, but the origin and the target of the transaction can not be the same");
                return;
            } else if (originType.Equals(targetType) && (originType.Equals(subjectType) || targetType.Equals(subjectType))) {
                MessageBox.Show("I'm sorry, but you can not transfer money from an subject to another subject. A transaction must either originate from or target an asset.");
                return;
            }

            Transaction.Label = label;
            Transaction.Amount = amount;
            Transaction.Origin = origin;
            Transaction.Target = target;
            Transaction.TransactionDate = (DateTime)xactXactDate.Value;
            Transaction.ValueDate = (DateTime)xactValueDate.Value;

            if (_didNotExist)
            {
                MainPage.Transactions.Add(Transaction);

                if (Transaction.OriginIsAsset)
                {
                    AssetModel _origin = (AssetModel)origin;
                    _origin.AddTransaction(Transaction);
                }

                if (Transaction.TargetIsAsset)
                {
                    AssetModel _target = (AssetModel)target;
                    _target.AddTransaction(Transaction);
                }
            }

            NavigationService.GoBack();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}