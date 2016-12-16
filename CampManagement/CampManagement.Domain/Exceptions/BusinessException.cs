using System;

namespace CampManagement.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {

        }
    }
}
