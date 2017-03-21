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
            _units = UnitFactoryBuilder.Create().GetGroupUnits(race);
            _units.SelectLider();
        }

        public void StartRandomFighting(UnitGroup unitGroup)
        {
            GameLogger.Instance.Log(string.Format("\nStart Fighting! {0} VS {1}!", this, unitGroup));

            while (IsSomeBodyAlive() && unitGroup.IsSomeBodyAlive())
            {
                FightOrHelp(GetRandomFighter(), unitGroup, this);
                FightOrHelp(unitGroup.GetRandomFighter(), this, unitGroup);
            }
            SelectWinner(this, unitGroup);
        }

        public void StartOrderedFighting(UnitGroup unitGroup)
        {
            GameLogger.Instance.Log(string.Format("\nStart Fighting! {0} VS {1}!", this, unitGroup));

            while (IsSomeBodyAlive() && unitGroup.IsSomeBodyAlive())
            {
                FightOrHelp(GetNextFighter(), unitGroup, this);
                FightOrHelp(unitGroup.GetNextFighter(), this, unitGroup);

                if (unitGroup.IsRoundComplete() && IsRoundComplete())
                {
                    ToNextRound();
                    unitGroup.ToNextRound();
                    GameLogger.Instance.Log("Next Round!");
                }
            }

            SelectWinner(this, unitGroup);
        }

        public override string ToString()
        {
            return string.Format("Fighting group {0}", Race);
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

        public IUnit GetTarget()
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

        private static void FightOrHelp(IFighter fighter, UnitGroup opositeGroup, UnitGroup currentGroup)
        {
            if (fighter == null)
            {
                return;
            }
            if (UnitAction.Random && fighter.IsHelpfull())
            {
                var target = currentGroup.GetNotImprovedTarget();
                if (target != null)
                {
                    fighter.DoRandomActionByType(target, UnitActionType.HelpBuff);
                    return;
                }
            }
            if (UnitAction.Random)
            {
                UnitActionType type;
                fighter.DoRandomActionByType(GetTarget(fighter, opositeGroup, out type), type);
            }
            else if (fighter.IsHealer() && currentGroup.IsNeedHeal())
            {
                fighter.DoRandomActionByType(currentGroup.GetTargetWithLowerHealth(), UnitActionType.Heal);
            }
            else
            {
                UnitActionType type;
                fighter.DoRandomActionByType(GetTarget(fighter, opositeGroup, out type), type);
            }
        }

        private static IUnit GetTarget(IFighter attacker, UnitGroup opositeGroup, out UnitActionType type)
        {
            if (UnitAction.Random && attacker is ICurseCaster && opositeGroup.IsSomeBodyImproved())
            {
                var target = opositeGroup.GetImprovedTarget();
                if (target != null)
                {
                    type = UnitActionType.Curse;
                    return target;
                }
            }
            if (UnitAction.Random && attacker is IDiseaseCaster)
            {
                var target = opositeGroup.GetNotDiseasedTarget();
                if (target != null)
                {
                    type = UnitActionType.Disease;
                    return target;
                }
            }
            type = UnitActionType.Attack;
            return opositeGroup.GetTarget();
        }

        private IFighter GetRandomFighter()
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
                return improvedUnits.OrderByDescending(u => u.Characteristics.Speed).FirstOrDefault();
            }
            return aliveUnits.OrderByDescending(u => u.Characteristics.Speed).FirstOrDefault();
        }

        private static void SelectWinner(UnitGroup firstGroup, UnitGroup secondGroup)
        {
            var winner = firstGroup.IsSomeBodyAlive() ? firstGroup : secondGroup;
            GameLogger.Instance.Log(string.Format("{0} is the winner!", winner));
            GameLogger.Instance.Log(winner.GetAliveUnits().PrintUnits());
        }

        private List<Unit> GetAliveUnits()
        {
            return _units.Where(u => u.IsAlive).ToList();
        }
    }
}