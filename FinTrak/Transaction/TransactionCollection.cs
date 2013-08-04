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

        // for now, usused
        public AssetCollection AssetCollection { get; set; }
    }
}
