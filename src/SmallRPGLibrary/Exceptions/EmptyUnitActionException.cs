namespace SmallRPGLibrary.Exceptions
{
    public class EmptyUnitActionException : BusinessLogicException
    {
        public EmptyUnitActionException()
        {
        }

        public EmptyUnitActionException(string message)
            : base(message)
        {
        }
    }
}