using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public abstract class MeleeUnit : Unit
    {

        protected abstract AttackParameters MeleeAttackParams { get; }

        private double Damage
        {
            get { return CountUnitAttackDamage(MeleeAttackParams.Damage); }
        }

        protected override Characteristics Characteristics
        {
            get
            {
                return new Characteristics
                {
                    Stamina = 30,
                    Speed = 4,
                };
            }
        }

        protected MeleeUnit(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
        }

        [UnitAction]
        public void MeeleAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, MeleeAttackParams.Name);
        }
    }
}
