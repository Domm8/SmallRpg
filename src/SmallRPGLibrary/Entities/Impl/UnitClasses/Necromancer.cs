﻿using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.BuffSettings;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Necromancer : Unit, IDiseaseCaster, IWizard
    {
        private double Damage
        {
            get
            {
                return CountUnitAttackDamage(9);
            }
        }

        protected override Characteristics Characteristics
        {
            get
            {
                return new Characteristics
                {
                    Stamina = 1,
                    Speed = 5,
                };
            }
        }

        public Necromancer(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
            if (UnitRace != Race.Undead)
            {
                throw new ArgumentException("Unit Race for class Necromancer could be only Undead.");
            }
        }

        [UnitAction(UnitActionType.Disease)]
        public void CastDisease(IUnit unit)
        {
            var buffSettings = new TargetBuffSettings
            {
                BuffCaster = this,
                Target = unit,
            };
            unit.AddBuff(new DiseaseBuff(buffSettings));
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, "Plague");
        }
    }
}
