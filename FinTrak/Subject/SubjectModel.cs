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
    [Index(Columns="_id")]
    public class SubjectModel : INotifyPropertyChanged, ITransactionTarget
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        private uint _id;
        [Column]
        private string _name;
        [Column]
        private string _title;
        [Column]
        private string _type;
        [Column(CanBeNull = true)]
        private string _phone;
        [Column(CanBeNull = true)]
        private string _email;
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

        public string ITLabel { get { return _title; } }

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
