using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinTrak.Asset;
using FinTrak.Transaction;

namespace FinTrak.Subject
{
    public class SubjectCollection : ObservableCollection<SubjectModel>
    {
        public SubjectCollection() : base() { }
        public SubjectCollection(List<SubjectModel> subjects) : base(subjects) { }
    }
}
