using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

using FinTrak;
using FinTrak.Asset;
using FinTrak.Budget;
using FinTrak.Subject;
using FinTrak.Transaction;

namespace FinTrak
{
    public class StorageManager
    {
        private bool _dataLoaded = false;
        private FinTrakDatabaseRepository dbRepo = new FinTrakDatabaseRepository();

        public AssetCollection Assets { get; private set; }
        public TransactionCollection Transactions { get; private set; }
        public SubjectCollection Subjects { get; private set; }

        public async void InitializeData()
        {
            await dbRepo.Initialize();

            InitializeAssets();
            InitializeTransactions();
            InitializeSubjects();

            _dataLoaded = true;

            // Link assets and transactions with each other
            foreach (AssetModel asset in Assets)
            {
                List<TransactionModel> xacts = Transactions.FindTransactionsForAsset(asset);

                foreach (TransactionModel xact in xacts)
                {
                    bool _added = false;
                    if (xact.OriginIsAsset && xact.OriginId == asset.Id)
                    {
                        asset.AddTransaction(xact, true);
                        _added = true;
                    }
                    if (xact.TargetIsAsset && xact.TargetId == asset.Id && !_added)
                    {
                        asset.AddTransaction(xact, false);
                    }
                    else if (xact.TargetIsAsset && xact.TargetId == asset.Id)
                    {
                        xact.Target = asset;
                    }
                }
            }

            foreach (SubjectModel subject in Subjects)
            {
                List<TransactionModel> xacts = Transactions.Where((TransactionModel transaction) =>
                {
                    return (transaction.OriginId == subject.Id && !transaction.OriginIsAsset) || (transaction.TargetId == subject.Id && !transaction.TargetIsAsset);
                }).ToList();

                foreach (TransactionModel xact in xacts)
                {
                    if (!xact.TargetIsAsset)
                    {
                        xact.Target = subject;
                    }
                    if (!xact.OriginIsAsset)
                    {
                        xact.Origin = subject;
                    }
                }
            }
        }

        void InitializeAssets()
        {
            Assets = new AssetCollection(dbRepo.LoadAssets());

            Assets.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveAssets(Assets.ToList());
            };
        }

        void InitializeSubjects()
        {
            Subjects = new SubjectCollection(dbRepo.LoadSubjects());

            Subjects.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveSubjects(Subjects.ToList());
            };
        }

        void InitializeTransactions()
        {
            Transactions = new TransactionCollection(dbRepo.LoadTransactions());

            Transactions.CollectionChanged += (s1, e1) =>
            {
                dbRepo.SaveTransactions(Transactions.ToList());
            };
        }

        public void SaveAll()
        {
            dbRepo.SaveGenerically();
        }

        public void ClearAll()
        {
            dbRepo.ClearStorage();
            Assets.Clear();
            Transactions.Clear();
            Subjects.Clear();
        }
    }
}
