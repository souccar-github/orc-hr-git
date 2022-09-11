namespace HRIS.Validation.MessageKeys
{
    public class GlobalMessages
    {
        public const string ResourceGroupName = "GlobalMessages";
        
        public const string Unknown = "Unknown";
        public const string CanNotAdd = "CanNotAdd";
        public const string CanNotDelete = "CanNotDelete";
        public const string CanNotRead = "CanNotRead";
        public const string CanNotUpdate = "CanNotUpdate";
        public const string DeleteConfirm = "DeleteConfirm";
        public const string Done = "Done";
        public const string Faild = "Faild";
        public const string CannotGetInformation = "CannotGetInformation";// تستخدم في حال ارسال ريكويست الى السيرفر لجلب بيانات معينة وفشل الحصول على البيانات
        public const string Duplication = "Duplication";
        public const string EntityInUse = "EntityInUse";
        public const string ErrorWhileAdd = "ErrorWhileAdd"; 
        public const string ErrorWhileDelete = "ErrorWhileDelete";
        public const string ErrorWhileUpdate = "ErrorWhileUpdate";
        public const string IndexInUse = "IndexInUse";
        public const string NoIdexes = "NoIdexes";
        public const string NotAuthorized = "NotAuthorized";
        public const string ReferncesValue = "ReferncesValue";
        public const string OperationConfirm = "OperationConfirm";

        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
