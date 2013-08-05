using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrak.Transaction
{
    public interface ITransactionTarget
    {
        uint Id { get; set; }

        string ITLabel { get; }
        string ITType { get; }
    }
}
