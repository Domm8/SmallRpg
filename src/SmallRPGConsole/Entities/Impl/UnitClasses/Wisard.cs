using SmallRPG.Attributes;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using System;

namespace SmallRPG.Entities.Impl.UnitClasses
{
    public class Wisard : Unit, IUnitImprover, IWisard
    {

        private double Damage
        {
            get
            {
                var damage = 4;
                switch (UnitRace)
                {
                    case Race.Elf:
                        damage = 10;
                        break;
                    case Race.Human:
                        damage = 4;
                        break;
                }
                return damage * DamageMultiplier;
            }
        }

        public Wisard(Race unitRace) : base(unitRace)
        {
            if (IsDarkRace)
            {
                throw new ArgumentException("Unit Race for class Wisard could not include Dark races like Orcs or Undeads.");
            }
        }

        [UnitAction(UnitActionType.Help)]
        public void ImproveUnit(IUnit unit)
        {
            unit.BecomeImproved(this);
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, "magic");
        }
    }
}
