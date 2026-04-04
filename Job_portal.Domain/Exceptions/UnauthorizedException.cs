using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Domain.Exceptions
{
    public class UnauthorizedException:Exception
    {
        public UnauthorizedException(string message):base(message)
        {
            
        }
    }
}
