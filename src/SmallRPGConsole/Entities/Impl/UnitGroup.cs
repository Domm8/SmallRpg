using System;
using System.Collections.Generic;
using System.Linq;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using SmallRPG.Services;

namespace SmallRPG.Entities.Impl
{
    public class UnitGroup
    {
        private readonly List<Unit> _units;
        private Race Race { get; set; }

        public UnitGroup(Race race)
        {
            Race = race;
            _units = UnitFactory.GetGroupUnits(race);
            _units.SelectLider();
        }

        public void StartFighting(UnitGroup unitGroup)
        {
            GameLogger.Instance.Log(string.Format("\nStart Fighting! {0} VS {1}!", this, unitGroup));
            while (IsSomeBodyAlive() && unitGroup.IsSomeBodyAlive())
            {
                var allyFighter = GetFighter();
                allyFighter.FightWith(GetTarget(allyFighter, unitGroup, this));

                if (unitGroup.IsSomeBodyAlive())
                {
                    var opositeFighter = unitGroup.GetFighter();
                    opositeFighter.FightWith(GetTarget(opositeFighter, this, unitGroup));
                }
            }
            var winner = IsSomeBodyAlive() ? this : unitGroup;
            GameLogger.Instance.Log(string.Format("{0} is the winner!", winner));
            GameLogger.Instance.Log(winner.GetAliveUnits().PrintUnits());
        }

        public override string ToString()
        {
            return string.Format("Fighting group {0}", Race);
        }

        public bool IsSomeBodyAlive()
        {
           return GetAliveUnits().Any();
        }
        
        public bool IsSomeBodyImproved()
        {
            return GetAliveUnits().Any(u => u.IsImproved);
        }

        public IUnit GetTarget()
        {
            return GetAliveUnits().GetRandomUnit();
        }

        public IUnit GetTargetExeptCurrentFighter(IFighter attacker)
        {
            var aliveUnits = GetAliveUnits();
            if (aliveUnits.Count > 1)
            {
                aliveUnits = aliveUnits.Where(f => !f.Equals(attacker)).ToList();
            }
            return aliveUnits.GetRandomUnit();
        }

        public IUnit GetImprovedTarget(IFighter attacker)
        {
            var improvedUnits = GetAliveUnits().Where(u => u.IsImproved).ToList();
            if (improvedUnits.Count > 0)
            {
                return improvedUnits.GetRandomUnit();
            }
            return null;
        }

        public IUnit GetNotDiseasedTarget(IFighter attacker)
        {
            var notDiseasedUnits = GetAliveUnits().Where(u => !u.IsDiseased).ToList();
            if (notDiseasedUnits.Count > 0)
            {
                return notDiseasedUnits.GetRandomUnit();
            }
            return null;
        }

        private static IUnit GetTarget(IFighter attacker, UnitGroup opositeGroup, UnitGroup currentGroup)
        {
            var isRandomlyImproved = new Random().Next(0, 100) > 49;

            if (attacker is IUnitImprover && isRandomlyImproved)
            {
                return currentGroup.GetTargetExeptCurrentFighter(attacker);
            }
            var isSomebodyImproved = opositeGroup.IsSomeBodyImproved();
            if (attacker is ICurseCaster && !isSomebodyImproved && attacker is IUnitImprover)
            {
                return currentGroup.GetTargetExeptCurrentFighter(attacker);
            }
            if (attacker is ICurseCaster && isSomebodyImproved)
            {
                var target = opositeGroup.GetImprovedTarget(attacker);
                if (target != null) { return target; }
            }
            if (attacker is IDiseaseCaster)
            {
                var target = opositeGroup.GetNotDiseasedTarget(attacker);
                if (target != null) { return target; }
            }
            return opositeGroup.GetTarget();
        }

        private IFighter GetFighter()
        {
            var aliveUnits = GetAliveUnits();
            var improvedUnits = aliveUnits.Where(u => u.IsImproved).ToList();
            if (improvedUnits.Count > 0)
            {
                return improvedUnits.GetRandomUnit();
            }
            return aliveUnits.GetRandomUnit();
        }

        private List<Unit> GetAliveUnits()
        {
            return _units.Where(u => u.IsAlive).ToList();
        }
    }
}