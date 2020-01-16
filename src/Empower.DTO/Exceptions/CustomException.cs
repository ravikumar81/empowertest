using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empower.DTO
{   
    public class IsInvalidException : Exception
    {
        public IsInvalidException(string message, params object[] args)
            : base(string.Format(message, args)) { }

    }

    public class IsExistException : Exception
    {
        public IsExistException(string message, params object[] args)
           : base(string.Format(message, args)) { }
    } 

    public class IsNotFoundException : Exception
    {
        public IsNotFoundException(string message, params object[] args)
           : base(string.Format(message, args)) { }

    }

    public class IsRequiredException : Exception
    {
        public IsRequiredException(string message, params object[] args)
           : base(string.Format(message, args)) { }

    }

    public class IsLengthRequired : Exception
    {
        public IsLengthRequired(string message, params object[] args)
           : base(string.Format(message, args)) { }

    }   

    public class IsDeletedException : Exception
    {
        public IsDeletedException(string message, params object[] args)
          : base(string.Format(message, args)) { }
    }

    public class IsForbiddenException : Exception
    {
        public IsForbiddenException(string message, params object[] args)
          : base(string.Format(message, args)) { }      
    }   
}
