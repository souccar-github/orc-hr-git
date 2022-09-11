using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Domain
{
    public class SouccarException:Exception
    {
        public SouccarException()
        {

        }
        public SouccarException(string message):base(message)
        {

        }
    }
}
