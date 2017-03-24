using System;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses.Range
{
    public class Hunter: RangeUnit  
    {
        public Hunter(Race race, int unitIndex)
            : base(race, unitIndex)
        {
            if (UnitRace != Race.Elf)
            {
                throw new ArgumentException("Unit Race for class Hunter could be only Elf.");
            }
        }

        protected override AttackParameters RangeAttackParams
        {
            get { return new AttackParameters("Big Bow", 9); }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get { return new AttackParameters("Sword", 5); }
        }

        protected override string ClassName
        {
            get { return "Forest Hunter"; }
        }
    }
}