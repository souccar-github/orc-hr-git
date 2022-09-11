namespace Souccar.Core
{
    public class Enums
    {
        #region LockMode enum

        /// <summary>
        ///     Provides an NHibernate.LockMode facade so as to avoid a direct dependency on the NHibernate DLL.
        ///     Further information concerning lockmodes may be found at:
        ///     http://www.hibernate.org/hib_docs/nhibernate/html_single/#transactions-locking
        /// </summary>
        public enum LockMode
        {
            None,
            Read,
            Upgrade,
            UpgradeNoWait,
            Write
        }

        #endregion
    }
    public enum DomainErrors
    {
        DupLicateValueError = 1,
        ReferncesValueError = 2,
        InternalError = 500,
        SecurityError = 3

    }
}