namespace UI.Utilities
{
    public class PathStep
    {
        public MasterRecordOrder MasterRecordOrder { get; set; }
        public int StepOrder { get; set; }
        public int StepId { get; set; }
        public RibbonLevels Level { get; set; }
        public string StepName { get; set; }
        public string ActionName { get; set; }
        public string ContextName { get; set; }
        public string AreaName { get; set; }
    }
}