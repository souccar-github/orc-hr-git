using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
   // [Module(ModulesNames.EmployeeRelationServices)]
   // [Order(100)]

    public class TaskSetting : Entity, IConfigurationRoot
    {
        public virtual string Name { set; get; }
        public virtual int Number { set; get; }
        public virtual TimeFactor TimeFactor { set; get; }
        public virtual int TotalSeconds {
            get { return GetTotalSeconds(); }
        }

        private int GetTotalSeconds()
        {
            if (TimeFactor == TimeFactor.Hours)
            {
                return Number * 3600;
            }
            if (TimeFactor == TimeFactor.Minutes)
            {
                return Number * 60;
            }
            return TimeFactor == TimeFactor.Seconds ? Number : 0;
        }

    }
}
