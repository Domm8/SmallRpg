using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.Base
{
    public abstract class RangeUnit : Unit
    {
        protected abstract AttackParameters MeleeAttackParams { get; }

        protected abstract AttackParameters RangeAttackParams { get; }

        protected override Characteristics Characteristics
        {
            get
            {
                return new Characteristics
                {
                    Stamina = 1,
                    Speed = 10,
                };
            }
        }

        protected RangeUnit(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
        }

        [UnitAction(UnitActionType.Attack)]
        protected void MeleeAttack(IUnit target)
        {
            target.TakeDamage(CountUnitAttackDamage(MeleeAttackParams.Damage), this, MeleeAttackParams.Name);
        }

        [UnitAction(UnitActionType.Attack)]
        protected virtual void RangeAttack(IUnit target)
        {
            target.TakeDamage(CountUnitAttackDamage(RangeAttackParams.Damage), this, RangeAttackParams.Name);
        }
    }
}