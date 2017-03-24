using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class ImprovementBuff : Buff
    {
        private readonly IUnit _caster;

        public ImprovementBuff(IUnit unit, IUnit caster)
            : base(unit, 2, "Improve")
        {
            _caster = caster;
            if (_caster.IsBuffedBy(GetType()))
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
                GameLogger.Instance.Log(string.Format("{0} was improved by {1}", BuffedUnit, _caster), LogLevel.Improve);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be improved by {1}, bacause he is already improved or he is dead.", BuffedUnit,
                                  _caster), LogLevel.Warn);
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