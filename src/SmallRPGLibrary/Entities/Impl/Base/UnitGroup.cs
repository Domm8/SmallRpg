using System.Collections.Generic;
using System.Linq;
using SmallRPGLibrary.Consts;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;

namespace SmallRPGLibrary.Entities.Impl.Base
{
    public class UnitGroup
    {
        private readonly List<Unit> _units;
        private Race Race { get; set; }

        public UnitGroup(Race race)
        {
            Race = race;
            _units = UnitFactoryCreator.Create().GetGroupUnits(race);
            _units.SelectLider();
        }

        public override string ToString()
        {
            return string.Format("Fighting group {0}", Race);
        }

        public List<IFighter> GetAliveFighters()
        {
            return new List<IFighter>(_units.Where(u => u.IsAlive));
        }

        public bool IsSomeBodyAlive()
        {
           return GetAliveUnits().Any();
        }

        public bool IsRoundComplete()
        {
           return !GetAliveUnits().Any(u => u.CanMove);
        }
        
        public bool IsSomeBodyImproved()
        {
            return GetAliveUnits().Any(u => u.IsBuffedBy<ImprovementBuff>());
        }

        public bool IsNeedHeal()
        {
            return GetAliveUnits().Any(u => u.Health < DefaultValues.UNIT_NEED_HEAL_HEALTH);
        }

        public IUnit GetRandomTarget()
        {
            return GetAliveUnits().GetRandomUnit();
        }

        public void ToNextRound()
        {
            GetAliveUnits().ForEach(u => u.ToNextRound());
        }

        public IUnit GetNotImprovedTarget()
        {
            var aliveUnits = GetAliveUnits().Where(f => !f.IsBuffedBy<ImprovementBuff>()).ToList();
            return aliveUnits.GetRandomUnit();
        }

        public IUnit GetTargetWithLowerHealth()
        {
           return GetAliveUnits().OrderBy(u => u.Health).FirstOrDefault();
        }

        public IUnit GetImprovedTarget()
        {
            var improvedUnits = GetAliveUnits().Where(u => u.IsBuffedBy<ImprovementBuff>()).ToList();
            if (improvedUnits.Count > 0)
            {
                return improvedUnits.GetRandomUnit();
            }
            return null;
        }

        public IUnit GetNotDiseasedTarget()
        {
            var notDiseasedUnits = GetAliveUnits().Where(u => !u.IsBuffedBy<DiseaseBuff>()).ToList();
            if (notDiseasedUnits.Count > 0)
            {
                return notDiseasedUnits.GetRandomUnit();
            }
            return null;
        }

        public IFighter GetRandomFighter()
        {
            var aliveUnits = GetAliveUnits();
            var improvedUnits = aliveUnits.Where(u => u.IsBuffedBy<ImprovementBuff>()).ToList();
            if (improvedUnits.Count > 0)
            {
                return improvedUnits.GetRandomUnit();
            }
            return aliveUnits.GetRandomUnit();
        }

        public IFighter GetNextFighter()
        {
            var aliveUnits = GetAliveUnits().Where(u => u.CanMove);
            var improvedUnits = aliveUnits.Where(u => u.IsBuffedBy<ImprovementBuff>()).ToList();
            if (improvedUnits.Count > 0)
            {
                return improvedUnits.OrderByDescending(u => u.FullCharacteristics.Speed).FirstOrDefault();
            }
            return aliveUnits.OrderByDescending(u => u.FullCharacteristics.Speed).FirstOrDefault();
        }

        private List<Unit> GetAliveUnits()
        {
            return _units.Where(u => u.IsAlive).ToList();
        }
    }
}