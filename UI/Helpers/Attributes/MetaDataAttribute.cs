#region

using System;
using System.Web.Mvc;

#endregion

namespace UI.Helpers.Attributes
{
    public abstract class MetaDataAttribute : Attribute
    {
        public abstract void Process(ModelMetadata modelMetadata);
    }
}