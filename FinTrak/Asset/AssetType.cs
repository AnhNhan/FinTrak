using System;

namespace FinTrak.Asset
{
    public enum AssetType
    {
        Cash = 0,
        CashSavings = 1,
        Account = 2,
        AccountSavings = 4,
        CreditCard = 8,
        DebitCard = 16,
        Prepaid = 32,
    }
}
