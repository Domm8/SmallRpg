using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class SimpleBuff : Buff
    {
        public SimpleBuff(IUnit target, string name, int lifetime = -1)
            : base(target, lifetime, name)
        {
        }

        public BuffCharacteristics BuffCharacteristics
        {
            set { Characteristics = value + Characteristics; }
        }

        protected override void Action()
        {
        }

        protected override void IterationAction()
        {
        }

        protected override void DeactivateAction()
        {
        }
    }
}