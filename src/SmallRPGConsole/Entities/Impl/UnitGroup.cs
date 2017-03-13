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
        private readonly List<IFighter> _units;
        private Race Race { get; set; }

        public UnitGroup(Race race)
        {
            Race = race;
            _units = UnitFactory.GetGroupUnits(race);
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
        }

        public override string ToString()
        {
            return string.Format("Fighting group {0}", Race);
        }

        public bool IsSomeBodyAlive()
        {
            var isAlive = GetAliveUnits().Any();
            return isAlive;
        }
        
        public bool IsSomeBodyImproved()
        {
            var isImproved = GetAliveUnits().Any(u => u.IsImproved);
            return isImproved;
        }

        public IFighter GetTarget()
        {
            var aliveUnits = GetAliveUnits();
            var index = new Random().Next(0, aliveUnits.Count);
            return aliveUnits[index];
        }

        public IFighter GetTargetExeptCurrentFighter(IFighter attacker)
        {
            var aliveUnits = GetAliveUnits().ToList();
            if (aliveUnits.Count > 1)
            {
                aliveUnits = GetAliveUnits().Where(f => !f.Equals(attacker)).ToList();
            }
            var index = new Random().Next(0, aliveUnits.Count);
            return aliveUnits[index];
        }

        public IFighter GetImprovedTarget(IFighter attacker)
        {
            var aliveUnits = GetAliveUnits();
            var improvedUnits = aliveUnits.Where(u => u.IsImproved).ToList();
            if (improvedUnits.Count > 0)
            {
                var improvedIndex = new Random().Next(0, improvedUnits.Count);
                return improvedUnits[improvedIndex];
            }
            return null;
        }

        private static IFighter GetTarget(IFighter attacker, UnitGroup opositeGroup, UnitGroup currentGroup)
        {
            var isRandomlyImproved = new Random().Next(0, 10) > 4;

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
                return opositeGroup.GetImprovedTarget(attacker);
            }
            return opositeGroup.GetTarget();
        }

        private IFighter GetFighter()
        {
            var aliveUnits = GetAliveUnits();
            var improvedUnits = aliveUnits.Where(u => u.IsImproved).ToList();
            if (improvedUnits.Count > 0)
            {
                var improvedIndex = new Random().Next(0, improvedUnits.Count);
                return improvedUnits[improvedIndex];
            }
            return aliveUnits[new Random().Next(0, aliveUnits.Count)];
        }

        private List<IFighter> GetAliveUnits()
        {
            return _units.Where(u => u.IsAlive).ToList();
        }
    }
}