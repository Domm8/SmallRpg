using System;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses.Range
{
    public class AxeThrower : RangeUnit
    {
        public AxeThrower(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
            if (UnitRace != Race.Orc)
            {
                throw new ArgumentException("Unit Race for class AxThrower could be only Orc.");
            }
        }

        protected override AttackParameters RangeAttackParams
        {
            get { return new AttackParameters("Throwing Axe", 4); }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get { return new AttackParameters("Axe", 3); }
        }

        protected override string ClassName
        {
            get
            {
                return "AxeThrower";
            }
        }
    }
}