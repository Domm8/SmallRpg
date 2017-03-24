using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses.Range
{
    [UnitAction(UnitActionType.Heal)]
    public class CrossBowMan : RangeUnit, IUnitHealer
    {
        protected override AttackParameters RangeAttackParams
        {
            get { return new AttackParameters("Crossbow", 6); }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get { return new AttackParameters("Knife", 4); }
        }

        public CrossBowMan(Race unitRace, int unitIndex)
            : base(Race.Human, unitIndex)
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
            unit.RestoreHealth(3 * DamageMultiplier, this, "Bandaging");
        }
    }
}