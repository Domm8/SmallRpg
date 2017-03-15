using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    [UnitAction(UnitActionType.Heal)]
    public class Druid : Wisard, IUnitHealer
    {
        public Druid(Race unitRace) : base(unitRace)
        {
            if (UnitRace != Race.Elf)
            {
                throw new ArgumentException("Unit Race for class Druid could be only Elf.");
            }
        }

        [UnitAction(UnitActionType.Heal)]
        public void Heal(IUnit unit)
        {
            unit.Healing(6 * DamageMultiplier, this, "Soul of the Forest");
        }
    }
}