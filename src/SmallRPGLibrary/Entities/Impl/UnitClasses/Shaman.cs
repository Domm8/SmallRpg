using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using System;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    [UnitAction(UnitActionType.Heal)]
    public class Shaman : Wisard, ICurseCaster, IUnitHealer
    {
        protected override double Damage
        {
            get
            {
                return 4 * DamageMultiplier;
            }
        }

        protected override string MagicAttackName
        {
            get { return "Lightning Bolt"; }
        }

        public Shaman(Race unitRace) : base(unitRace)
        {
            if (UnitRace != Race.Orc)
            {
                throw new ArgumentException("Unit Race for class Shaman could be only Orc.");
            }
        }

        [UnitAction(UnitActionType.Heal)]
        public void Heal(IUnit unit)
        {
            unit.Healing(7 * DamageMultiplier, this, "High tide");
        }

        [UnitAction(UnitActionType.Curse)]
        public void CastCurse(IUnit unit)
        {
            unit.BecomeCursed(this);
        }
    }
}
