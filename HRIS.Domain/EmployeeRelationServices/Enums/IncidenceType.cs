
namespace HRIS.Domain.EmployeeRelationServices.Enums
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// Update By:Ammar Alziebak
    /// </summary>
    public enum IncidenceType
    {
        /// <summary>
        /// منح مكافأة لموظف
        /// </summary>
        GrantingBonus,//0
        /// <summary>
        /// فرض عقوبة بحق موظف
        /// </summary>
        ImpositionPenalty,//1
        /// <summary>
        /// تعيين لموظف
        /// </summary>
        AssignEmployee,//2
        /// <summary>
        /// فرز موظف 	
        /// </summary>
        SortEmployee,//3
        /// <summary>
        /// تعيين مجدد لموظف 	
        /// </summary>
        SetRemade,//4
        /// <summary>
        /// نقل وظيفي لموظف 	
        /// </summary>
        FunctionalTransfer,//5
        /// <summary>
        /// تعيين بعقد لموظف	
        /// </summary>
        AssignByContract,//6
        /// <summary>
        /// تأصيل موظف 	
        /// </summary>
        ConsolidateEmployee,//7
        /// <summary>
        /// تجديد عقد موظف 	
        /// </summary>
        RenewContract,//8
        /// <summary>
        /// تكليف موظف بمسمى وظيفي 	
        /// </summary>
        AssignEmployeeFromJobTitle,//9
        /// <summary>
        /// إنهاء تكليف موظف من مسمى  وظيفي 	
        /// </summary>
        EndAssigningEmployeeFromJobTitle,//10
        /// <summary>
        /// مباشرة موظف حكماً 	
        /// </summary>
        StartEmployment,//11
        /// <summary>
        /// إنهاء خدمة موظف 	
        /// </summary>
        EndEmployment,//12
        /// <summary>
        /// إنهاء عقد موظف	
        /// </summary>
        EndContract,//13
        /// <summary>
        /// منح علاوة لموظف 	
        /// </summary>
        GrantingPromotion,//14
        /// <summary>
        /// كف يد 	
        /// </summary>
        PalmHand,//15
        /// <summary>
        /// إنهاء كف يد موظف 	
        /// </summary>
        EndPalmHand,//16
        /// <summary>
        /// إلغاء كف يد موظف 	
        /// </summary>
        CancelPalmHand,//17
        /// <summary>
        /// بحكم المستقيل لموظف 	
        /// </summary>
        VirtueOutgoingEmployee,//18
        /// <summary>
        /// ضم خدمة موظف 	
        /// </summary>
        IncludeEmployeeService,//19
        /// <summary>
        /// تمديد خدمة موظف 	
        /// </summary>
        ExtendServiceEmployee,//20
        /// <summary>
        /// اسناد موظف 	
        /// </summary>
        AssigningEmployee,//21
        /// <summary>
        /// إعادة للعمل لموظف 	
        /// </summary>
        ReturnToWork,//22
        /// <summary>
        /// نقل موظف 	
        /// </summary>
        MoveEmployee,//23
        /// <summary>
        /// ندب موظف إلى جهة خارجية 	
        /// </summary>
        AssignEmployeeToExternalParty,//24
        /// <summary>
        /// ندب موظف من جهة خارجية 	
        /// </summary>
        AssignEmployeeFromExternalParty,//25
        /// <summary>
        /// إنهاء ندب موظف إلى جهة خارجية 	
        /// </summary>
        EndAssignEmployeeToExternalParty,//26
        /// <summary>
        /// إنهاء ندب موظف من جهة خارجية 	
        /// </summary>
        EndAssignEmployeeFromExternalParty,//27
        /// <summary>
        /// إعارة موظف إلى جهة خارجية 	
        /// </summary>
        LoanEmployeeToExternalParty,//28
        /// <summary>
        /// استعارة موظف من جهة خارجية 	
        /// </summary>
        BorrowEmployeeFromExternalParty,//29
        /// <summary>
        /// إنهاء إعارة موظف إلى جهة خارجية 	
        /// </summary>
        EndLoanEmployeeToExternalParty,//30
        /// <summary>
        /// إنهاء استعارة موظف من جهة خارجية 	
        /// </summary>
        EndBorrowEmployeeFromExternalParty,//31
        /// <summary>
        /// زيادة راتب بمرسوم
        /// </summary>
        SalaryIncreaseByDecree,//32
        /// <summary>
        /// الترفيع بشكل تلقائي
        /// </summary>
        AutomaticPromotion,//33
        //MilitaryService,
        //Assigning,
        //Promotion,
        //Reward,
        //TerminationAssigningEmployee,
        //AssigningEmployeeToPosition,
        Other,//34
        AssignRepresentative,//تعيين وكيل
        EndAssignRepresentative,//انهاء وكيل
        UpdateName,//تعديل الاسم او الكنية
        ReAssignAndFixation,//اعادة تعيين
        UpdateSalary,//تعديل الاجر
        EmployeeSettlement,//تصنيف وتسوية

    }
}