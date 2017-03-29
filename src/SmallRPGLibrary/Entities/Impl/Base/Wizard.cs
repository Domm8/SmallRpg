using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Impl.BuffSettings;
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
            var buffSettings = new TargetBuffSettings
            {
                BuffCaster = this,
                Target = unit,
            };
            unit.AddBuff(new ImprovementBuff(buffSettings));
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(CountUnitAttackDamage(MagicAttackParams.Damage), this, MagicAttackParams.Name);
        }
    }
}
