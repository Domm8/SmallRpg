using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Interface
{
    public interface IUnit
    {
        double Health { get; }
        bool IsLeader { get; }
        Race UnitRace { get; }

        void TakeDamage(double damage, IUnit attacker, string attackName);
        void LooseHealth(double damage, string attackName);
        void Healing(double health, IUnitHealer healer, string healingName);
        void RestoreHealth(double health, string healingName);
        void BecomeImproved(IUnitImprover caster);
        void BecomeUnImproved();
        void BecomeUnDiseased();
        void BecomeCursed(ICurseCaster caster);
        void BecomeDiseased(IDiseaseCaster caster);
        bool IsFrendlyUnit(IUnit unit);
        void AddBuff(IBuff buff);
        bool IsBuffedBy<T>() where T : IBuff;
    }
}