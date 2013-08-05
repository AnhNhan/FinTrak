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
    [Index(Columns = "_id")]
    public class AssetModel : INotifyPropertyChanged, ITransactionTarget
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        private uint _id;
        [Column]
        private string _title;
        [Column]
        private AssetType _typeId;
        // Just for caching
        [Column]
        private float _currentBalance = 0.0f;
        // Just for caching
        [Column]
        private int _numTransactions = 0;
        [Column]
        private DateTime _dateCreated;

        #region properties

        public uint Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

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

        public List<TransactionModel> Transactions { get; set; }

        public void AddTransaction(TransactionModel transaction, bool isOrigin = true)
        {
            Transactions.Add(transaction);
            CurrentBalance += transaction.Amount;
            NumTransactions += 1;
            transaction.Asset = this;

            if (isOrigin)
            {
                transaction.Origin = this;
            }
            else
            {
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

        public string GetName()
        {
            return _title;
        }

        public string GetTitle()
        {
            throw new NotImplementedException();
        }

        public new string GetType()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
