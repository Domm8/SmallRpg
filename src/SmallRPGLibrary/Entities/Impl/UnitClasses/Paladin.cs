using System;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Paladin : MeleeUnit
    {
        public Paladin(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (unitRace != Race.Human)
            {
                throw new ArgumentException("Unit Race for class Paladin could be only Human.");
            }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get
            {
                return new AttackParameters("Battle Hammer", 17);
            }
        }
    }
}