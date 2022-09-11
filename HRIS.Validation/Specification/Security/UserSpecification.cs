using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Souccar.Domain.Security;
using SpecExpress;

namespace HRIS.Validation.Specification.Security
{
    
    public class UserSpecification : Validates<User>
    {
        public UserSpecification()
        {
            IsDefaultForType();
            
            Check(x => x.Username).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FullName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.Email).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            //Check(x => x.MobilePhone).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

        }
    }
}
