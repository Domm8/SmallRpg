using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Interface
{
    public interface IUnitHealer
    {
        [UnitAction(UnitActionType.Heal)]
        void Heal(IUnit unit);
    }
}