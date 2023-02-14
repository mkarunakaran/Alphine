using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.SharedKernel.Exceptions
{
    public class DomainException : Exception
    {
       internal DomainException(string fieldName, string businessMessage)
            : base(businessMessage)
        {
            this.FieldName = fieldName;
        }

       public string FieldName { get; }
    }
}
