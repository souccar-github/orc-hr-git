using System;

namespace Souccar.Domain.DomainModel
{
    public abstract class OrderEntity : IndexEntity,IOrder
    {
        public virtual int Order {get;set;}
       
    }
}
