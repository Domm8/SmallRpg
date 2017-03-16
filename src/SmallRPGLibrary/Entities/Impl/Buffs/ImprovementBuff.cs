using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class ImprovementBuff : Buff
    {
        private readonly IUnitImprover _caster;

        public ImprovementBuff(IUnit unit, IUnitImprover caster)
            : base(unit, 2, "Improve")
        {
            _caster = caster;
            if (_caster is IUnit && ((IUnit)_caster).IsImproved)
            {
                LifeTime *= 2;
            }
        }

        protected override void Action()
        {
            BuffedUnit.BecomeImproved(_caster);
        }

        protected override void IterationAction()
        {
        }

        protected override void DeactivateAction()
        {
            BuffedUnit.BecomeUnImproved();
        }
    }
}