using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class DiseaseBuff : Buff
    {
        private readonly IDiseaseCaster _diseaseCaster;

        public DiseaseBuff(IUnit target, IDiseaseCaster caster)
            : base(target, 2, "Disease")
        {
            _diseaseCaster = caster;
        }

        protected override void Action()
        {
            BuffedUnit.BecomeDiseased(_diseaseCaster);
        }

        protected override void IterationAction()
        {
            BuffedUnit.LooseHealth(1, "Disease Tic");
        }

        protected override void DeactivateAction()
        {
            BuffedUnit.BecomeUnDiseased();
        }
    }
}