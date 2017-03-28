using System;

namespace SmallRPGLibrary.Exceptions
{
    public class UnitActionNotAllowedException : Exception
    {
        public UnitActionNotAllowedException()
        {  
        }
        
        public UnitActionNotAllowedException(string message) : base(message)
        {  
        }
    }
}