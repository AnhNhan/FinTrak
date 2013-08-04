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
    [Index(Columns = "_id")]
    public class TransactionModel : INotifyPropertyChanged
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        private uint _id;
        [Column]
        private uint _assetId;
        [Column]
        private float _amount;
        [Column]
        private uint _originId;
        [Column]
        private bool _originIsAsset;
        [Column]
        private uint _targetId;
        [Column]
        private bool _targetIsAsset;
        [Column]
        private DateTime _transactionDate;
        [Column]
        private DateTime _valueDate;

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

        public uint AssetId
        {
            get { return _assetId; }
            private set
            {
                _assetId = value;
                NotifyPropertyChanged("AssetId");
            }
        }

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

        public bool OriginIsAsset
        {
            get { return _originIsAsset; }
            private set
            {
                _originIsAsset = value;
                NotifyPropertyChanged("OriginIsAsset");
            }
        }

        public bool TargetIsAsset
        {
            get { return _targetIsAsset; }
            private set
            {
                _targetIsAsset = value;
                NotifyPropertyChanged("TargetIsAsset");
            }
        }

        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                _transactionDate = value;
                NotifyPropertyChanged("TransactionDate");
            }
        }

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

        public bool IsDeduction { get { return _amount < 0.0f; } }


        private AssetModel _asset;
        public AssetModel Asset
        {
            get
            {
                return _asset;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                _asset = value;
                AssetId = value.Id;
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
                _originId = value.Id;
                _originIsAsset = value.GetType().ToString() == "FinTrak.Asset.AssetModel";
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
                _targetId = value.Id;
                _targetIsAsset = value.GetType().ToString() == "FinTrak.Asset.AssetModel";
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
