namespace HRIS.Domain.Objectives.Enums
{
    public enum ActionPlanStatus
    {
        Pending,//Default value.
        Accepted,//When the all workflows accepted by positions.
        InProgress,//When the actual date changed.
        Closed,//When the action plan finished.
        Cancelled//When the action plan declined by a workflow position.
        //-------------------------Notes-------------------------
        //1- (InProgress & Closed) indicates to the tracking state.
        //2- (Pending) indicates to the New added.
        //3- (Accepted & Cancelled) indicates to the result of approval.
        //---------------------------------------------------------
    }
}