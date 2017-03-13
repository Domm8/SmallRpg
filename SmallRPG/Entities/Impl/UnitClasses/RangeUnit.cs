using System;
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
                    case Race.Human:
                        damage = 3;
                        break;
                    case Race.Undead:
                    case Race.Orc:
                        damage = 2;
                        break;
                }
                return damage * DamageMultiplier;
            }
        }

        private double RangeDamage
        {
            get
            {
                var damage = 3;
                switch (UnitRace)
                {
                    case Race.Elf:
                        damage = 7;
                        break;
                    case Race.Human:
                        damage = 5;
                        break;
                    case Race.Orc:
                        damage = 3;
                        break;
                    case Race.Undead:
                        damage = 4;
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

        public override void Combat(IUnit unit)
        {
            var random = new Random();
            var next = random.Next(0, 100);
            if (next > 60)
            {
                MeleeAttack(unit);
            }
            else
            {
                RangeAttack(unit);
            }
        }

        private void MeleeAttack(IUnit target)
        {
            target.TakeDamage(Damage, this, "sword");
        }

        private void RangeAttack(IUnit target)
        {
            target.TakeDamage(RangeDamage, this, "bow");
        }
    }
}