using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Infrastructure.Exceptions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ORMException:Exception
    {
        public ORMException()
        {
            
        }
        public ORMException(string message):base(message)
        {
        }
    }
}
