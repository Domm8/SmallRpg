using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.Base
{
    public abstract class Wizard : Unit, IUnitImprover, IWizard
    {
        protected abstract AttackParameters MagicAttackParams { get; }

        protected override Characteristics Characteristics
        {
            get
            {
                return new Characteristics
                {
                    Stamina = 1,
                    Speed = 7,
                };
            }
        }

        protected Wizard(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
        }

        [UnitAction(UnitActionType.HelpBuff)]
        public void ImproveUnit(IUnit unit)
        {
            unit.AddBuff(new ImprovementBuff(unit, this));
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(CountUnitAttackDamage(MagicAttackParams.Damage), this, MagicAttackParams.Name);
        }
    }
}
