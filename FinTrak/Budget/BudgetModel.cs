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
    [Index(Columns = "Id")]
    public class BudgetModel : INotifyPropertyChanged
    {
        private uint _id;
        private string _name;
        private float _alloc;
        private float _spent;

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

        [Column]
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

        [Column]
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

        public List<TransactionModel> Transactions = new List<TransactionModel>();

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
