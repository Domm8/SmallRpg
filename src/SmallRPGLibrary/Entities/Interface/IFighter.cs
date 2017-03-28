using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Interface
{
    public interface IFighter
    {
        bool IsAlive { get; }
        bool IsHelpfull();
        bool IsHealer();
        void DoRandomActionByType(IUnit unit, UnitActionType actionType, bool sameActionTry = false);
    }
}