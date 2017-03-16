using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using System;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Necromancer : Unit, IDiseaseCaster, IWisard
    {
        private double Damage
        {
            get
            {
                return 9 * DamageMultiplier;
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
            unit.AddBuff(new DiseaseBuff(unit, this));
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, "Plague");
        }
    }
}
