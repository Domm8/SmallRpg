using System;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class RangeUnit : Unit
    {
        protected double Damage
        {
            get
            {
                var damage = 3;
                switch (UnitRace)
                {
                    case Race.Elf:
                        damage = 5;
                        break;
                    case Race.Human:
                        damage = 4;
                        break;
                    case Race.Undead:
                    case Race.Orc:
                        damage = 3;
                        break;
                }
                return damage * DamageMultiplier;
            }
        }

        protected double RangeDamage
        {
            get
            {
                var damage = 4;
                switch (UnitRace)
                {
                    case Race.Elf:
                        damage = 9;
                        break;
                    case Race.Human:
                        damage = 6;
                        break;
                    case Race.Orc:
                        damage = 4;
                        break;
                    case Race.Undead:
                        damage = 5;
                        break;
                }
                return damage * DamageMultiplier;
            }
        }

        protected override string ClassName
        {
            get
            {
                switch (UnitRace)
                {
                    case Race.Elf:
                    case Race.Orc:
                        return "Bowman";
                    case Race.Undead:
                        return "Hunter";
                }
                return base.ClassName;
            }
        }

        public RangeUnit(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
        }

        [UnitAction(UnitActionType.Attack)]
        protected void MeleeAttack(IUnit target)
        {
            target.TakeDamage(Damage, this, "sword");
        }

        [UnitAction(UnitActionType.Attack)]
        protected virtual void RangeAttack(IUnit target)
        {
            target.TakeDamage(RangeDamage, this, "bow");
        }
    }
}