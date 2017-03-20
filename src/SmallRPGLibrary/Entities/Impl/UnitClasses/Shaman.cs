﻿using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Services;

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

        public Shaman(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
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
            if (unit.DeactivateBuff(typeof (ImprovementBuff)))
            {
                GameLogger.Instance.Log(string.Format("{0} was cursed by {1}", unit, this), LogLevel.Improve);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be cursed by {1}, bacause he is not improved or he is dead.", unit,
                                  this), LogLevel.Warn);
            }
        }
    }
}
