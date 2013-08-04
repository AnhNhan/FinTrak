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

namespace FinTrak.Budget
{
    [Table]
    [Index(Columns = "_id")]
    public class BudgetModel : INotifyPropertyChanged
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        private uint _id;
        [Column]
        private string _name;
        [Column]
        private float _alloc;
        [Column]
        private float _spent;

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

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                value.Trim();
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public float AllocatedAmount
        {
            get { return _alloc; }
            set
            {
                if (value == 0.0f)
                {
                    throw new ArgumentException();
                }
                _alloc = value;
                NotifyPropertyChanged("AllocatedAmount");
            }
        }

        public float SpentAmount
        {
            get { return _spent; }
            set
            {
                if (value == 0.0f)
                {
                    throw new ArgumentException();
                }
                _spent = value;
                NotifyPropertyChanged("SpentAmount");
            }
        }

        #endregion

        public List<TransactionModel> Transactions;

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
