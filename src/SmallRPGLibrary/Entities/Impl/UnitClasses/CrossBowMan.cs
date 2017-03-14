using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    [UnitAction(UnitActionType.Heal)]
    public class CrossBowMan : RangeUnit, IUnitHealer
    {
        public CrossBowMan(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (unitRace != Race.Human)
            {
                throw new ArgumentException("Unit Race for class CrossBowMan could be only Human.");
            }
        }

        protected override string ClassName
        {
            get { return "Crossbowman"; }
        }

        [UnitAction(UnitActionType.Heal)]
        public void Heal(IUnit unit)
        {
            unit.Healing(4 * DamageMultiplier, this, "bandaging");
        }

        protected override void RangeAttack(IUnit target)
        {
            target.TakeDamage(RangeDamage, this, "crossbow");
        }
    }
}