using System;

namespace Domain.Seedwork
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commit changes to database
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback changes to database
        /// </summary>
        /// <returns></returns>
        void Rollback();
    }
}
