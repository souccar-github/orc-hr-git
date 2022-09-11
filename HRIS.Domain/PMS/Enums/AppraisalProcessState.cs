namespace HRIS.Domain.PMS.Enums
{
    public enum AppraisalProcessState
    {
        Start,
        Submit,
        ManagerApprove,
        DepartmentManagerApprove,
        DepartmentManagerReject,
        FinalSubmit,
        HrManagerApprove,
        HrManagerReject,
        Finish
    }
}