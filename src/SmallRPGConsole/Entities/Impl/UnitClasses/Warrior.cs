using SmallRPG.Attributes;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using SmallRPG.Services;

namespace SmallRPG.Entities.Impl.UnitClasses
{
    public class Warrior : Unit
    {

        private double Damage
        {
            get
            {
                var damage = 15;
                switch (UnitRace)
                {
                    case Race.Elf:
                        damage = 15;
                        break;
                    case Race.Human:
                        damage = 18;
                        break;
                    case Race.Orc:
                        damage = 19;
                        break;
                    case Race.Undead:
                        damage = 18;
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
                    case Race.Orc:
                        return "Goblin";
                    case Race.Undead:
                        return "Zomby";
                }
                return base.ClassName;
            }
        }

        public Warrior(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
        }

        [UnitAction]
        public void MeeleAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, "meele");
        }
    }
}
