using System.Collections.Generic;
using System.Linq;
using SmallRPGLibrary.Consts;
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
            _units = UnitFactory.GetGroupUnits(race);
            _units.SelectLider();
        }

        public void StartFighting(UnitGroup unitGroup)
        {
            GameLogger.Instance.Log(string.Format("\nStart Fighting! {0} VS {1}!", this, unitGroup));
            while (IsSomeBodyAlive() && unitGroup.IsSomeBodyAlive())
            {
                FightOrHelp(GetFighter(), unitGroup, this);

                //Console.ReadLine();
                if (unitGroup.IsSomeBodyAlive())
                {
                    FightOrHelp(unitGroup.GetFighter(), this, unitGroup);
                    //Console.ReadLine();
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

        public bool IsNeedHeal()
        {
            return GetAliveUnits().Any(u => u.Health < DefaultValues.UNIT_NEED_HEAL_HEALTH);
        }

        public IUnit GetTarget()
        {
            return GetAliveUnits().GetRandomUnit();
        }

        public IUnit GetNotImprovedTarget(IFighter attacker)
        {
            var aliveUnits = GetAliveUnits().Where(f => !f.IsImproved).ToList();
            return aliveUnits.GetRandomUnit();
        }

        public IUnit GetTargetWithLowerHealth()
        {
           return GetAliveUnits().OrderBy(u => u.Health).FirstOrDefault();
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

        private static void FightOrHelp(IFighter fighter, UnitGroup opositeGroup, UnitGroup currentGroup)
        {
            if (UnitAction.Random && fighter.IsHelpfull())
            {
                var target = currentGroup.GetNotImprovedTarget(fighter);
                if (target != null)
                {
                    fighter.HelpTo(target);
                    return;
                }
            }
            if (UnitAction.Random)
            {
                UnitActionType type;
                fighter.FightWith(GetTarget(fighter, opositeGroup, out type), type);
            }
            else if (fighter.IsHealer() && currentGroup.IsNeedHeal())
            {
                fighter.HealUnit(currentGroup.GetTargetWithLowerHealth());
            }
            else
            {
                UnitActionType type;
                fighter.FightWith(GetTarget(fighter, opositeGroup, out type), type);
            }
        }

        private static IUnit GetTarget(IFighter attacker, UnitGroup opositeGroup, out UnitActionType type)
        {
            if (UnitAction.Random && attacker is ICurseCaster && opositeGroup.IsSomeBodyImproved())
            {
                var target = opositeGroup.GetImprovedTarget(attacker);
                if (target != null)
                {
                    type = UnitActionType.Curse;
                    return target;
                }
            }
            if (UnitAction.Random && attacker is IDiseaseCaster)
            {
                var target = opositeGroup.GetNotDiseasedTarget(attacker);
                if (target != null)
                {
                    type = UnitActionType.Disease;
                    return target;
                }
            }
            type = UnitActionType.Attack;
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