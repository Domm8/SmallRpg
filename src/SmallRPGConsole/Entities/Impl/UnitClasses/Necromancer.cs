using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using System;

namespace SmallRPG.Entities.Impl.UnitClasses
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

        public override void Combat(IUnit unit)
        {
            var random = new Random();
            var next = random.Next(0, 10);
            if (next > 4)
            {
                MagicAttack(unit);
            }
            else
            {
                CastDisease(unit);
            }
        }

        public void CastDisease(IUnit unit)
        {
            unit.BecomeDiseased(this);
        }

        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, "magic");
        }
    }
}
