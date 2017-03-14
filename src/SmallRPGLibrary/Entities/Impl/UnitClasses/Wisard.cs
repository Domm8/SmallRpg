using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using System;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Wisard : Unit, IUnitImprover, IWisard
    {

        protected virtual double Damage
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

        protected virtual string MagicAttackName
        {
            get { return "Magic"; }
        }

        public Wisard(Race unitRace) : base(unitRace)
        {
        }

        [UnitAction(UnitActionType.HelpBuff)]
        public void ImproveUnit(IUnit unit)
        {
            unit.BecomeImproved(this);
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, MagicAttackName);
        }
    }
}
