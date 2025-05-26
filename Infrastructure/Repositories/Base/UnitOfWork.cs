using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using Application.Repositories.Base;
using Infrastructure.Database;
using Application.Repositories;

namespace Infrastructure.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        // Repositories
        private IKycOperationRepository kycOperationRepository;
        private IKycAccessOperationRepository kycAccessOperationRepository;
        private IParentPartnerRepository parentPartnerRepository;
        private IPartnerRepository partnerRepository;
        private IManualAccessRepository manualAccessRepository;
        private IBiometricProfileRepository biometricProfileRepository;
        private IBiometricProfileHistoryRepository biometricProfileHistoryRepository;
        private ICustomerRepository customerRepository;

        public IKycOperationRepository KycOperationRepository() => kycOperationRepository = kycOperationRepository ?? new KycOperationRepository(_dbContext);
        public IKycAccessOperationRepository KycAccessOperationRepository() => kycAccessOperationRepository = kycAccessOperationRepository ?? new KycAccessOperationRepository(_dbContext);
        public IParentPartnerRepository ParentPartnerRepository() => parentPartnerRepository = parentPartnerRepository ?? new ParentPartnerRepository(_dbContext);
        public IPartnerRepository PartnerRepository() => partnerRepository = partnerRepository ?? new PartnerRepository(_dbContext);
        public IManualAccessRepository ManualAccessRepository() => manualAccessRepository = manualAccessRepository ?? new ManualAccessRepository(_dbContext);
        public IBiometricProfileRepository BiometricProfileRepository() => biometricProfileRepository = biometricProfileRepository ?? new BiometricProfileRepository(_dbContext);
        public IBiometricProfileHistoryRepository BiometricProfileHistoryRepository() => biometricProfileHistoryRepository = biometricProfileHistoryRepository ?? new BiometricProfileHistoryRepository(_dbContext);
        public ICustomerRepository CustomerRepository() => customerRepository = customerRepository ?? new CustomerRepository(_dbContext);

        #region IUnitOfWork Members

        private bool disposed = false;
        private readonly KycDBContext _dbContext;

        public IDbContextTransaction Transaction { get; set; }

        public UnitOfWork(KycDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_dbContext);
        }

        public void Begin()
        {
            Transaction = _dbContext.Database.BeginTransaction();
        }

        public async Task BeginAsync()
        {
            Transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public async Task CommitAsync()
        {
            await Transaction.CommitAsync();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            await Transaction.RollbackAsync();
        }

        public int SaveChanges()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region IDisposable Members
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
