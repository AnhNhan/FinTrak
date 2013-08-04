using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinTrak.Asset;
using FinTrak.Budget;
using FinTrak.Subject;
using FinTrak.Transaction;

namespace FinTrak
{
    class FinTrakDataContext : DataContext
    {
        public FinTrakDataContext(string connectionString) : base(connectionString) { }

        public Table<AssetModel> Assets;
        public Table<SubjectModel> Subjects;
        public Table<TransactionModel> Transactions;
        public Table<BudgetModel> Budgets;
    }
}
