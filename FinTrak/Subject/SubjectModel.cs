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

namespace FinTrak.Subject
{
    [Table]
    [Index(Columns="Id")]
    public class SubjectModel : INotifyPropertyChanged, ITransactionTarget
    {
        private uint _id;
        private string _name;
        private string _label;
        private string _type;
        private string _phone;
        private string _email;
        private DateTime _dateCreated;

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
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                value.Trim();
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _label = value;
                NotifyPropertyChanged("Title");
            }
        }

        [Column]
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                value.Trim();
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }

        [Column(CanBeNull = true)]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                value.Trim();
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _phone = value;
                NotifyPropertyChanged("Phone");
            }
        }

        [Column(CanBeNull = true)]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                value.Trim();
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
                _email = value;
                NotifyPropertyChanged("Email");
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

        #region interface

        public string ITLabel { get { return Label; } }

        public string ITType { get { return Type; } }

        #endregion

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
