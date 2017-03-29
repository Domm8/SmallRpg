using SmallRPGLibrary.Entities.Impl.BuffSettings;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class ImprovementBuff : Buff
    {

        public ImprovementBuff(TargetBuffSettings settings)
            : base(settings.Target, 2, "Improve")
        {
            BuffCaster = settings.BuffCaster;
            if (BuffCaster.IsBuffedBy(GetType()))
            {
                LifeTime *= 2;
            }
        }

        public override BuffCharacteristics Characteristics
        {
            get
            {
                return base.Characteristics + new BuffCharacteristics { DamageMultiplier = 1.4, HealMultiplier = 1.5};
            }
        }


        protected override void Action()
        {
            if (BuffedUnit.IsAlive && !BuffedUnit.IsBuffedBy<ImprovementBuff>())
            {
                GameLogger.Instance.Log(string.Format("{0} was improved by {1}", BuffedUnit, BuffCaster), LogLevel.Improve);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be improved by {1}, bacause he is already improved or he is dead.", BuffedUnit,
                                  BuffCaster), LogLevel.Warn);
            }
        }

        protected override void IterationAction()
        {
        }

        protected override void DeactivateAction()
        {
        }
    }
}