using SmallRPG.Attributes;
using SmallRPG.Enums;

namespace SmallRPG.Entities.Interface
{
    public interface IUnitHealer
    {
        [UnitAction(UnitActionType.Heal)]
        void Heal(IUnit unit);
    }
}