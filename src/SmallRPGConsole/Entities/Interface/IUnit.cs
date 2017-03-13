using SmallRPG.Enums;

namespace SmallRPG.Entities.Interface
{
    public interface IUnit
    {
        Race UnitRace { get; }

        void TakeDamage(double damage, IUnit attacker, string attackName);
        void BecomeImproved(IUnitImprover caster);
        void BecomeCursed(ICurseCaster caster);
        void BecomeDiseased(IDiseaseCaster caster);
        bool IsFrendlyUnit(IUnit unit);
    }
}