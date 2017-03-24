using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    [UnitAction(UnitActionType.Heal)]
    public class Druid : Wizard, IUnitHealer
    {
        protected override AttackParameters MagicAttackParams
        {
            get { return new AttackParameters("Moon Fire", 10);  }
        }

        public Druid(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (UnitRace != Race.Elf)
            {
                throw new ArgumentException("Unit Race for class Druid could be only Elf.");
            }
        }

        [UnitAction(UnitActionType.Heal)]
        public void Heal(IUnit unit)
        {
            unit.RestoreHealth(6 * DamageMultiplier, this, "Soul of the Forest");
        }
    }
}