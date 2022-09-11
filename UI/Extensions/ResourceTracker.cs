#region

using System.Collections.Generic;
using System.Web;

#endregion

namespace AjaxControlToolkitMvc
{
    public class ResourceTracker
    {
        private const string resourceKey = "__resources";

        private readonly List<string> _resources;

        public ResourceTracker(HttpContextBase context)
        {
            _resources = (List<string>) context.Items[resourceKey];
            if (_resources == null)
            {
                _resources = new List<string>();
                context.Items[resourceKey] = _resources;
            }
        }

        public void Add(string url)
        {
            url = url.ToLower();
            _resources.Add(url);
        }

        public bool Contains(string url)
        {
            url = url.ToLower();
            return _resources.Contains(url);
        }
    }
}