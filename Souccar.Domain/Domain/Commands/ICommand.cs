using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Souccar.Domain.Commands
{
    public interface ICommand
    {
        bool IsValid();

        ICollection<ValidationResult> ValidationResults();
    }
}