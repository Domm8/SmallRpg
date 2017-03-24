using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Mage : Wizard
    {
        public Mage(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (UnitRace != Race.Human)
            {
                throw new ArgumentException("Unit Race for class Mage could be only Human.");
            }
        }

        protected override AttackParameters MagicAttackParams
        {
            get { return new AttackParameters("Fire Bolt", 8);}
        }

        [UnitAction(UnitActionType.Attack)]
        public void CastFrostBolt(IUnit target)
        {
            target.TakeDamage(CountUnitAttackDamage(5), this, "Frost Bolt");
        }
    }
}