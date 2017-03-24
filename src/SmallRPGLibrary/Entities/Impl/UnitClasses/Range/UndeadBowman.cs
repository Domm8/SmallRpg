using System;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses.Range
{
    public class UndeadBowman : RangeUnit
    {
        public UndeadBowman(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (unitRace != Race.Undead)
            {
                throw new ArgumentException("Unit Race for class UndeadBowman could be only Undead.");
            }
        }

        protected override AttackParameters RangeAttackParams
        {
            get { return new AttackParameters("Bow", 5); }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get { return new AttackParameters("Sword", 3); }
        }

        protected override string ClassName
        {
            get { return "Undead Bowman"; }
        }
    }
}