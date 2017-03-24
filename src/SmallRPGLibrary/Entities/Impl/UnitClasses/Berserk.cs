using System;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Berserk : MeleeUnit
    {
        public Berserk(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (unitRace != Race.Orc)
            {
                throw new ArgumentException("Unit Race for class Berserk could be only Orc.");
            }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get
            {
                return new AttackParameters("Furious Slash", 19);
            }
        }
    }
}