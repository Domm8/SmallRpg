using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public class HarmfulBuff : Buff
    {
        private readonly double _buffStartDamage;
        private readonly double _iterationDamage;
        private readonly IUnit _attacker;

        public HarmfulBuff(IUnit target, int lifetime, string name, IUnit attacker, double buffStartDamage, double iterationDamage)
            : base(target, lifetime, name)
        {
            _attacker = attacker;
            _iterationDamage = attacker.CountUnitAttackDamage(iterationDamage);
            _buffStartDamage = attacker.CountUnitAttackDamage(buffStartDamage);
        }

        protected override void Action()
        {
            if (_buffStartDamage > 0)
            {
                BuffedUnit.TakeDamage(_buffStartDamage,_attacker, Name);
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