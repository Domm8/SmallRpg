
namespace SmallRPGLibrary.Exceptions
{
    public class UnitActionNotAllowedException : BusinessLogicException
    {
        public UnitActionNotAllowedException()
        {  
        }
        
        public UnitActionNotAllowedException(string message) : base(message)
        {  
        }
    }
}