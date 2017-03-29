using SmallRPGLibrary.Entities.Impl.BuffSettings;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class DiseaseBuff : Buff
    {
        
        public DiseaseBuff(TargetBuffSettings settings)
            : base(settings.Target, 2, "Disease")
        {
            BuffCaster = settings.BuffCaster;
        }

        public override BuffCharacteristics Characteristics
        {
            get
            {
                return base.Characteristics + new BuffCharacteristics { DamageMultiplier = 0.5 };
            }
        }

        protected override void Action()
        {
            if (BuffedUnit.IsAlive && !BuffedUnit.IsBuffedBy<DiseaseBuff>())
            {
                GameLogger.Instance.Log(string.Format("{0} was diseased by {1}", BuffedUnit, BuffCaster), LogLevel.Improve);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be diseased by {1}, bacause he is already diseased or he is dead.", BuffedUnit,
                                  BuffCaster), LogLevel.Warn);
            }
        }

        protected override void IterationAction()
        {
            BuffedUnit.TakeDamage(BuffCaster.CountUnitAttackDamage(1), "Disease Tic");
        }

        protected override void DeactivateAction()
        {
        }
    }
}