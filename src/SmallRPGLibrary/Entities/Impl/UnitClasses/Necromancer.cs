using SmallRPGLibrary.Entities.Impl;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using System;
using SmallRPGLibrary.Services;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Necromancer : Unit, IDiseaseCaster, IWisard
    {
        private double Damage
        {
            get
            {
                return 5 * DamageMultiplier;
            }
        }

        public Necromancer(Race unitRace) : base(unitRace)
        {
            if (!IsDarkRace && UnitRace == Race.Orc)
            {
                throw new ArgumentException("Unit Race for class Necromancer could be only Undead.");
            }
        }

        [UnitAction(UnitActionType.Attack)]
        public void CastDisease(IUnit unit)
        {
            unit.BecomeDiseased(this);
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, "magic");
        }
    }
}
