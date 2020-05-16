using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial10.Helper
{
    public class DbServiceExceptionHandler : Exception
    {
        public ExceptionHandlerEnumType Type { get; set; }

        public DbServiceExceptionHandler(ExceptionHandlerEnumType type, string msg) : base(message : msg)
        {
            Type = type;
        }

    }

    public enum ExceptionHandlerEnumType
    {
        NotFound = 0,
        NotUnique = 1
    }
}
