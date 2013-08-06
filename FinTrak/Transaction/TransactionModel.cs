using Microsoft.Phone.Data.Linq.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinTrak.Asset;
using FinTrak.Subject;

namespace FinTrak.Transaction
{
    [Table]
    [Index(Columns = "Id")]
    public class TransactionModel : INotifyPropertyChanged
    {
        private uint _id;
        private string _label;
        private float _amount;
        private uint _originId;
        private bool _originIsAsset;
        private uint _targetId;
        private bool _targetIsAsset;
        private DateTime _transactionDate;
        private DateTime _valueDate;

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
        public string Label
        {
            get { return _label; }
            set
            {
                value.Trim();
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _label = value;
                NotifyPropertyChanged("Label");
            }
        }

        [Column]
        public float Amount
        {
            get { return _amount; }
            set
            {
                if (value == 0.0f)
                {
                    throw new ArgumentException();
                }
                _amount = value;
                NotifyPropertyChanged("Amount");
            }
        }

        [Column]
        public uint OriginId
        {
            get { return _originId; }
            private set
            {
                _originId = value;
                NotifyPropertyChanged("OriginId");
                // TODO: Update Origin association
            }
        }

        [Column]
        public uint TargetId
        {
            get { return _targetId; }
            private set
            {
                _targetId = value;
                NotifyPropertyChanged("TargetId");
                // TODO: Update Target association
            }
        }

        [Column]
        public bool OriginIsAsset
        {
            get { return _originIsAsset; }
            private set
            {
                _originIsAsset = value;
                NotifyPropertyChanged("OriginIsAsset");
            }
        }

        [Column]
        public bool TargetIsAsset
        {
            get { return _targetIsAsset; }
            private set
            {
                _targetIsAsset = value;
                NotifyPropertyChanged("TargetIsAsset");
            }
        }

        [Column]
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                _transactionDate = value;
                NotifyPropertyChanged("TransactionDate");
            }
        }

        [Column]
        public DateTime ValueDate
        {
            get { return _valueDate; }
            set
            {
                _valueDate = value;
                NotifyPropertyChanged("ValueDate");
            }
        }

        #endregion

        public string AmountColor
        {
            get
            {
                if (!TargetIsAsset)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
        }

        private ITransactionTarget _origin;
        public ITransactionTarget Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                _origin = value;
                OriginId = value.Id;
                OriginIsAsset = value.GetType().Equals(typeof(AssetModel));
                NotifyPropertyChanged("Origin");
            }
        }
        private ITransactionTarget _target;
        public ITransactionTarget Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                _target = value;
                TargetId = value.Id;
                TargetIsAsset = value.GetType().Equals(typeof(AssetModel));
                NotifyPropertyChanged("Target");
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
    }
}
