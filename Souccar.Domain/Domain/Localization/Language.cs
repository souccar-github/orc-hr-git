using System.Collections.Generic;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Localization
{
    /// <summary>
    /// Represents a language
    /// </summary>
    [Module("Localization", Exclude = false)]
    public class Language : Entity, IAggregateRoot
    {

        public Language()
        {
            LocaleStringResources = new List<LocaleStringResource>();
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the language culture
        /// </summary>
        public virtual LanguageCulture LanguageCulture { get; set; }


        /// <summary>
        /// Gets or sets the display name 
        /// </summary>
        /// 

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string DisplayName { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        /// <summary>
        /// Gets or sets a value indicating whether the language supports "Right-to-left"
        /// </summary>
        public virtual bool Rtl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the language is published
        /// </summary>
        /// 

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool Published { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [UserInterfaceParameter(IsHidden = true)]
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets locale string resources
        /// </summary>
        public virtual IList<LocaleStringResource> LocaleStringResources { get; protected set; }

        // codehint: sm-add
        public override string ToString()
        {
            return this.LanguageCulture.ToString();
        }
    }
}
