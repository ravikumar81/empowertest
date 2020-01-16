using System;
using System.Data.Entity;
using System.Transactions;
//using Microsoft.EntityFrameworkCore;

namespace Empower.Repository
{
    public class UnitOfWorkManager : IManageUnitsOfWork
    {

        #region Private Members

        private TransactionScope _scope;
        public readonly DbContext _dbContext;

        #endregion

        #region Constructors / Destructors

        public UnitOfWorkManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region UnitOfWorkManager Implementation

        public void Begin()
        {
            _scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(2, 30, 0) }, TransactionScopeAsyncFlowOption.Enabled);
        }

        public void End(Exception ex = null)
        {
            try
            {
                if (ex != null) return;
                _scope.Complete();
            }
            finally
            {
                _scope.Dispose();
            }
        }

        #endregion

    }
}
