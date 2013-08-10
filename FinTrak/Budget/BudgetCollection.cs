using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrak.Budget
{
    class BudgetCollection : ObservableCollection<BudgetModel>
    {
        public BudgetCollection() : base() { }
        public BudgetCollection(List<BudgetModel> budgets) : base(budgets) { }
    }
}
