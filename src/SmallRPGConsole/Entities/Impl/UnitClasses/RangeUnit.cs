﻿using System;
using SmallRPG.Attributes;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;

namespace SmallRPG.Entities.Impl.UnitClasses
{
    public class RangeUnit : Unit
    {
        private double Damage
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

        private double RangeDamage
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
                    case Race.Human:
                        return "Crossbowman";
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
        private void MeleeAttack(IUnit target)
        {
            target.TakeDamage(Damage, this, "sword");
        }

        [UnitAction(UnitActionType.Attack)]
        private void RangeAttack(IUnit target)
        {
            target.TakeDamage(RangeDamage, this, "bow");
        }
    }
}