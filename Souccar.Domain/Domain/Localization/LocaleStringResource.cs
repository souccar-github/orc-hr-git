using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Localization
{
    /// <summary>
    /// Represents a locale string resource
    /// </summary>
    public class LocaleStringResource : Entity
    {
        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        /// 

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual ResourceGroup ResourceGroup { get; set; }
        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        /// 

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource value
        /// </summary>
        public virtual string ResourceValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this resource was installed by a plugin
        /// </summary>
        /// 

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsFromPlugin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this resource was modified by the user
        /// </summary>
        /// 

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsTouched { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual ResourceStatus ResourceStatus { get; set; }
        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public virtual Language Language { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", ResourceName, ResourceValue);
        }
    }

}
