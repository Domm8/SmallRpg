using System;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Interface
{
    public interface IUnit
    {
        double Health { get; }
        bool IsAlive { get; }
        bool IsLeader { get; }
        Race UnitRace { get; }

        void TakeDamage(double damage, IUnit attacker, string attackName);
        void TakeDamage(double damage, string attackName);
        void RestoreHealth(double health, IUnitHealer healer, string healingName);
        void RestoreHealth(double health, string healingName);
        bool DeactivateBuff(Type buffType);
        bool IsFrendlyUnit(IUnit unit);
        void AddBuff(IBuff buff);
        bool IsBuffedBy<T>() where T : IBuff;
        bool IsBuffedBy(Type buffType);
        double CountUnitAttackDamage(double damage);
    }
}