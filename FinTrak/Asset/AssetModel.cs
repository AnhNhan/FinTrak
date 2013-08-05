using Microsoft.Phone.Data.Linq.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinTrak.Transaction;

namespace FinTrak.Asset
{
    [Table]
    [Index(Columns = "Id")]
    public class AssetModel : INotifyPropertyChanged, ITransactionTarget
    {
        private uint _id;
        private string _title;
        private AssetType _typeId;
        // Just for caching
        private float _currentBalance = 0.0f;
        // Just for caching
        private int _numTransactions = 0;
        private DateTime _dateCreated = DateTime.Now;

        #region properties

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public uint Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        [Column]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                value.Trim();
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        [Column]
        public AssetType TypeId
        {
            get
            {
                return _typeId;
            }
            set
            {
                _typeId = value;
                NotifyPropertyChanged("TypeId");
            }
        }
        public string Type
        {
            get
            {
                return GetTypeName();
            }
        }

        [Column]
        public float CurrentBalance
        {
            get
            {
                return _currentBalance;
            }
            set
            {
                _currentBalance = value;
                NotifyPropertyChanged("CurrentBalance");
            }
        }

        [Column]
        public int NumTransactions
        {
            get
            {
                return _numTransactions;
            }
            set
            {
                _numTransactions = value;
                NotifyPropertyChanged("NumTransactions");
            }
        }

        [Column]
        public DateTime DateCreated
        {
            get
            {
                return _dateCreated;
            }
            set
            {
                _dateCreated = value;
                NotifyPropertyChanged("DateCreated");
            }
        }

        #endregion

        public string GetTypeName()
        {
            return GetTypeNameForAssetTypeId(TypeId);
        }

        public static string GetTypeNameForAssetTypeId(AssetType TypeId)
        {
            switch (TypeId)
            {
                case AssetType.Account:
                    return "Account";
                case AssetType.AccountSavings:
                    return "Savings Account";
                case AssetType.Cash:
                    return "Cash";
                case AssetType.CashSavings:
                    return "Cash Savings";
                case AssetType.CreditCard:
                    return "Credit Card";
                case AssetType.DebitCard:
                    return "Debit/Charge Card";
                case AssetType.Prepaid:
                    return "Prepaid account/card";
                default:
                    return "unknown";
            }
        }

        public string BalanceColor
        {
            get
            {
                if (CurrentBalance < 0.0f)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
        }

        // Unused, we populate it from time to time, but it is incomplete in general
        public List<TransactionModel> Transactions = new List<TransactionModel>();

        public void AddTransaction(TransactionModel transaction, bool isOrigin = true)
        {
            Transactions.Add(transaction);
            NumTransactions += 1;

            if (isOrigin)
            {
                CurrentBalance -= transaction.Amount;
                transaction.Origin = this;
            }
            else
            {
                CurrentBalance += transaction.Amount;
                transaction.Target = this;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region interface

        public string ITLabel { get { return _title; } }

        public string ITType { get { return GetTypeName(); } }

        #endregion
    }
}
