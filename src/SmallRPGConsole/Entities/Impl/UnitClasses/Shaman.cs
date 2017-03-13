using SmallRPG.Attributes;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using System;

namespace SmallRPG.Entities.Impl.UnitClasses
{
    public class Shaman : Unit, IUnitImprover, ICurseCaster, IWisard, IUnitHealer
    {
        private double Damage
        {
            get
            {
                return 4 * DamageMultiplier;
            }
        }

        public Shaman(Race unitRace) : base(unitRace)
        {
            if (!IsDarkRace && UnitRace == Race.Undead)
            {
                throw new ArgumentException("Unit Race for class Shaman could be only Orc.");
            }
        }

        [UnitAction(UnitActionType.Heal)]
        public void Heal(IUnit unit)
        {
            unit.Healing(5 * DamageMultiplier, this, "high tide");
        }

        [UnitAction(UnitActionType.Help)]
        public void ImproveUnit(IUnit unit)
        {
            unit.BecomeImproved(this);
        }

        [UnitAction(UnitActionType.Attack)]
        public void CastCurse(IUnit unit)
        {
            unit.BecomeCursed(this);
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, "lightning bolt");
        }
    }
}
