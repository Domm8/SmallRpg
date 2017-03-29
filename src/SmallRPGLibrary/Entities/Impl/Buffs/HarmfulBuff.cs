using SmallRPGLibrary.Entities.Impl.BuffSettings;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class HarmfulBuff : Buff
    {
        private readonly double _buffStartDamage;
        private readonly double _iterationDamage;

        public HarmfulBuff(HarmfulBuffSettings settings)
            : base(settings)
        {
            _iterationDamage = BuffCaster.CountUnitAttackDamage(settings.IterationDamage);
            _buffStartDamage = BuffCaster.CountUnitAttackDamage(settings.BuffStartDamage);
        }

        protected override void Action()
        {
            if (_buffStartDamage > 0)
            {
                BuffedUnit.TakeDamage(_buffStartDamage, BuffCaster, Name);
            }
        }

        protected override void IterationAction()
        {
            if (_iterationDamage > 0)
            {
                BuffedUnit.TakeDamage(_iterationDamage, Name);
            }
        }

        protected override void DeactivateAction()
        {
        }
    }
}