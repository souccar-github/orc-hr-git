#region

using System.Activities;
using UI.Helpers.Configuration;

#endregion

namespace UI.Workflows.Areas.PMSComprehensive
{
    public sealed class PrepareSystemForAppraisal : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> Text { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string text = context.GetValue(Text);

            WebConfigHelper.Modify("StartAppraisal", "True");
        }
    }
}