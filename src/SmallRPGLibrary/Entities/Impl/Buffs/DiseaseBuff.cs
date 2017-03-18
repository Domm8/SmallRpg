using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class DiseaseBuff : Buff
    {
        private readonly IUnit _diseaseCaster;

        public DiseaseBuff(IUnit target, IUnit caster)
            : base(target, 2, "Disease")
        {
            _diseaseCaster = caster;
        }

        public override double DamageMulplier
        {
            get
            {
                return 0.5;
            }
        }

        protected override void Action()
        {
            if (BuffedUnit.IsAlive && !BuffedUnit.IsBuffedBy<DiseaseBuff>())
            {
                GameLogger.Instance.Log(string.Format("{0} was diseased by {1}", BuffedUnit, _diseaseCaster), LogLevel.Improve);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be diseased by {1}, bacause he is already diseased or he is dead.", BuffedUnit,
                                  _diseaseCaster), LogLevel.Warn);
            }
        }

        protected override void IterationAction()
        {
            BuffedUnit.LooseHealth(1, "Disease Tic");
        }

        protected override void DeactivateAction()
        {
        }
    }
}