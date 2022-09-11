#region

using System.ComponentModel;
using UI.Helpers.Attributes;

#endregion

namespace UI.Models
{
    public class Template
    {
        [AutoComplete("Home", "AutoCompleteResult", "searchText")]
        [DisplayName(@"European Country Search")]
        public string SearchText { get; set; }
    }
}