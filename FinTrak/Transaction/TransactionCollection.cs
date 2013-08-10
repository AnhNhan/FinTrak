using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinTrak.Asset;
using FinTrak.Subject;

namespace FinTrak.Transaction
{
    public class TransactionCollection : ObservableCollection<TransactionModel>
    {
        public TransactionCollection() : base() { }
        public TransactionCollection(List<TransactionModel> transactions) : base(transactions) { }

        // for now, usused
        public AssetCollection AssetCollection { get; set; }

        public List<TransactionModel> FindTransactionsForAsset(AssetModel asset)
        {
            return this.Where((TransactionModel transaction) =>
            {
                return (transaction.OriginId == asset.Id && transaction.OriginIsAsset) || (transaction.TargetId == asset.Id && transaction.TargetIsAsset);
            }).ToList();
        }
    }
}
