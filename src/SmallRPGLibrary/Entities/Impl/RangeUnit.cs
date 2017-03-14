using System;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;

namespace SmallRPG.Entities.Impl
{
    public class RangeUnit : Unit
    {
        protected override double Damage
        {
            get
            {
                switch (UnitRace)
                {
                    case Race.Elf:
                        return 12;
                    case Race.Human:
                        return 14;                    
                    case Race.Undead:
                        return 11;                    
                    case Race.Orc:
                        return 13;
                }
                return 0;
            }
        }

        public RangeUnit(Race unitRace) : base(unitRace)
        {
        }

        public override void Combat(IUnit unit)
        {
            if (!unit.GetType().IsAssignableFrom(typeof(IFighter)))
            {
                throw new ArgumentException("Argument unit must implement IFighter interface!");
            }
            var random = new Random();
            var next = random.Next(0, 10);
            if (next > 5)
            {
                MeleeAttack((IFighter) unit);
            }
            else
            {
                RangeAttack((IFighter) unit);
            }
        }

        private void MeleeAttack(IFighter target)
        {
            target.TakeDamage(Damage, this, "bow");
        }

        private void RangeAttack(IFighter target)
        {
            target.TakeDamage(Damage, this, "bow");
        }
    }
}