using SmallRPG.Enums;

namespace SmallRPG.Entities.Interface
{
    public interface IUnit
    {
        bool IsLeader { get; }
        bool IsImproved { get; }
        bool IsDiseased { get; }
        Race UnitRace { get; }

        void TakeDamage(double damage, IUnit attacker, string attackName);
        void Healing(double health, IUnitHealer healer, string healingName);
        void BecomeImproved(IUnitImprover caster);
        void BecomeCursed(ICurseCaster caster);
        void BecomeDiseased(IDiseaseCaster caster);
        bool IsFrendlyUnit(IUnit unit);
    }
}