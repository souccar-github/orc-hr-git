using System;

namespace Souccar.Domain.DomainModel
{
    public interface IHistory
    {
        DateTime FromDate { get; set; }
        DateTime? ExpireDate { get; }
        string Comment { get; set; }
    }
}