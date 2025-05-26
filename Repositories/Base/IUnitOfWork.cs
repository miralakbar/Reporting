using System;
using System.Threading.Tasks;

namespace Application.Repositories.Base
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IKycOperationRepository KycOperationRepository();
        IKycAccessOperationRepository KycAccessOperationRepository();
        IParentPartnerRepository ParentPartnerRepository();
        IPartnerRepository PartnerRepository();
        IManualAccessRepository ManualAccessRepository();
        IBiometricProfileRepository BiometricProfileRepository();
        IBiometricProfileHistoryRepository BiometricProfileHistoryRepository();
        ICustomerRepository CustomerRepository();

        /// <summary> 
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 
        /// </summary>
        void Begin();

        /// <summary>
        /// 
        /// </summary>
        Task BeginAsync();

        /// <summary>
        /// 
        /// </summary>
        void Commit();

        /// <summary>
        /// 
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// 
        /// </summary>
        void Rollback();

        /// <summary>
        /// 
        /// </summary>
        Task RollbackAsync();
    }
}