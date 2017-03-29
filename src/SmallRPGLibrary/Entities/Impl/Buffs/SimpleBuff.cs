using SmallRPGLibrary.Entities.Impl.BuffSettings;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class SimpleBuff : Buff
    {
        public SimpleBuff(SimpleBuffSettings settings)
            : base(settings)
        {
            BuffCharacteristics = settings.BuffCharacteristics;
        }

        public BuffCharacteristics BuffCharacteristics
        {
            set
            {
                if (value != null)
                {
                    Characteristics = value + Characteristics;
                }
            }
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