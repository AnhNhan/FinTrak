using Microsoft.Phone.Data.Linq;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Windows.Storage;
using System.Text;
using System.Threading.Tasks;

using FinTrak.Asset;
using FinTrak.Budget;
using FinTrak.Subject;
using FinTrak.Transaction;

namespace FinTrak
{
    public class FinTrakDatabaseRepository
    {
        private FinTrakDataContext db;

        private Func<FinTrakDataContext, IQueryable<AssetModel>> queryAllAssets;
        private Func<FinTrakDataContext, AssetType, IQueryable<AssetModel>> queryAssetsOfType;
        private Func<FinTrakDataContext, IQueryable<BudgetModel>> queryAllBudgets;
        private Func<FinTrakDataContext, IQueryable<SubjectModel>> queryAllSubjects;
        private Func<FinTrakDataContext, IOrderedQueryable<TransactionModel>> queryAllTransactions;
        private Func<FinTrakDataContext, int, IOrderedQueryable<TransactionModel>> queryTransactionsFromAsset;

        public FinTrakDatabaseRepository()
        {
            queryAllAssets = CompiledQuery.Compile((FinTrakDataContext context) => from asset in db.Assets
                                                                                   select asset);
            queryAssetsOfType = CompiledQuery.Compile((FinTrakDataContext context, AssetType typeId) => from asset in db.Assets
                                                                                                        where asset.TypeId == typeId
                                                                                                        select asset);

            queryAllBudgets = CompiledQuery.Compile((FinTrakDataContext context) => from budget in db.Budgets
                                                                                    select budget);

            queryAllSubjects = CompiledQuery.Compile((FinTrakDataContext context) => from subject in db.Subjects
                                                                                     select subject);

            queryAllTransactions = CompiledQuery.Compile((FinTrakDataContext context) => from transaction in db.Transactions
                                                                                         orderby transaction.TransactionDate
                                                                                         select transaction);
            queryTransactionsFromAsset = CompiledQuery.Compile((FinTrakDataContext context, int assetId) => from transaction in db.Transactions
                                                                                                            where (transaction.OriginId == assetId && transaction.OriginIsAsset) || (transaction.TargetId == assetId && transaction.OriginIsAsset)
                                                                                                            orderby transaction.TransactionDate
                                                                                                            select transaction);
        }

        public async Task Initialize()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder scoresFolder = await localFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);

            db = new FinTrakDataContext(@"isostore:/data/db.sdf");
            if (!db.DatabaseExists())
            {
                db.CreateDatabase();
                DatabaseSchemaUpdater updater = db.CreateDatabaseSchemaUpdater();
                updater.DatabaseSchemaVersion = 1;
                updater.Execute();
            }
        }

        public List<AssetModel> LoadAssets()
        {
            return queryAllAssets(db).ToList();
        }

        public void SaveAssets(List<AssetModel> assets)
        {
            var newAssets = assets.Where(asset => asset.Id == 0);
            db.Assets.InsertAllOnSubmit(newAssets);
            db.SubmitChanges();
        }

        public List<BudgetModel> LoadBudgets()
        {
            return queryAllBudgets(db).ToList();
        }

        public void SaveBudgets(List<BudgetModel> budgets)
        {
            var newBudgets = budgets.Where(budget => budget.Id == 0);
            db.Budgets.InsertAllOnSubmit(newBudgets);
            db.SubmitChanges();
        }

        public List<SubjectModel> LoadSubjects()
        {
            return queryAllSubjects(db).ToList();
        }

        public void SaveSubjects(List<SubjectModel> subjects)
        {
            var newSubjects = subjects.Where(subject => subject.Id == 0);
            db.Subjects.InsertAllOnSubmit(newSubjects);
            db.SubmitChanges();
        }

        public List<TransactionModel> LoadTransactions(int assetId = 0)
        {
            IEnumerable<TransactionModel> transactions;
            if (assetId == 0)
            {
                transactions = queryAllTransactions(db);
            }
            else
            {
                transactions = queryTransactionsFromAsset(db, assetId);
            }
            return transactions.ToList();
        }

        public void SaveTransactions(List<TransactionModel> transactions)
        {
            var newTransactions = transactions.Where(transaction => transaction.Id == 0);
            db.Transactions.InsertAllOnSubmit(newTransactions);
            db.SubmitChanges();
        }
    }
}
