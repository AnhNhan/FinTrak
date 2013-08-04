using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinTrak.Transaction;

namespace FinTrak.Asset
{
    public class AssetCollection : ObservableCollection<AssetModel>
    {
        public AssetCollection() : base() { }

        // for now, unused
        public TransactionCollection TransactionCollection { get; set; }
    }
}
