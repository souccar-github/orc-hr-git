using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    // اعدادات أجهزة البصمة ويتم توليد البيانات بشكل تلقائي ومهمة المستخدم فقط التعديل لاضافة المعلومات الناقصة
    [Order(25)]
    [Module(ModulesNames.AttendanceSystem)]
    public class BioMetricSetting : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5, IsReference = true)]
        public virtual BioMetricDevice BioMetricDevice { get; set; } // الجهاز المدعوم ولا يظهر واجهة الاندكس لانها مولدة تلقائيا

        [UserInterfaceParameter(Order = 10)]
        public virtual string Name { get; set; } // الاسم الذي سيظهر كإسم مميز للجهاز
        
        [UserInterfaceParameter(Order = 15)]
        public virtual string IpAddress { get; set; } // عنوان الاي بي للجهاز الموصول الى الشبكة
        
        [UserInterfaceParameter(Order = 20)]
        public virtual int Port { get; set; } // رقم البورت للجهاز 
        public virtual bool IsActive { get; set; } // رقم البورت للجهاز 
    }
}
