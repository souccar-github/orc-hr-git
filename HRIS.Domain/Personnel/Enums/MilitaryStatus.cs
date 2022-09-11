#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Personnel.Enums
{

    public enum MilitaryStatus
    {
        /// <summary>
        /// Ãäåì ÎÏãÊå 
        /// </summary>
        Served,//Ãäåì ÎÏãÊå 
        /// <summary>
        /// ãÚİì
        /// </summary>
        Exempt,//ãÚİì
        /// <summary>
        /// ãÄÌá
        /// </summary>
        Delayed,//ãÄÌá
        /// <summary>
        /// æÍíÏ
        /// </summary>
        NotObligedToServe,//æÍíÏ
        /// <summary>
        /// ÇÍÊİÇÙ
        /// </summary>
        Hold,// ÇÍÊİÇÙ
        /// <summary>
        /// Úáì ÑÃÓ ÎÏãÊå
        /// </summary>
        Serving,//Úáì ÑÃÓ ÎÏãÊå
        /// <summary>
        /// ÇÍÊíÇØ
        /// </summary>
        Reserve,//ÇÍÊíÇØ
        
        /// <summary>
        /// áÇ ÔíÁ
        /// </summary>
        Nothing//áÇ ÔíÁ
    }
} 