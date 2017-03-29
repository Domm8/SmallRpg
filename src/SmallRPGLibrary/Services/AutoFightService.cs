using System;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Exceptions;

namespace SmallRPGLibrary.Services
{
    public class AutoFightService
    {
        public void StartRandomFighting(UnitGroup firstGroup, UnitGroup secondGroup)
        {
            Start(firstGroup, secondGroup);
            while (firstGroup.IsSomeBodyAlive() && secondGroup.IsSomeBodyAlive())
            {
                FightOrHelp(firstGroup.GetRandomFighter(), secondGroup, firstGroup);
                FightOrHelp(secondGroup.GetRandomFighter(), firstGroup, secondGroup);
            }
            SelectWinner(firstGroup, secondGroup);
        }

        public void StartOrderedFighting(UnitGroup firstGroup, UnitGroup secondGroup)
        {
            Start(firstGroup, secondGroup);
            var i = 1;
            while (firstGroup.IsSomeBodyAlive() && secondGroup.IsSomeBodyAlive())
            {
                Console.Write(i++);
                FightOrHelp(firstGroup.GetNextFighter(), secondGroup, firstGroup);
                Console.Write(i++);
                FightOrHelp(secondGroup.GetNextFighter(), firstGroup, secondGroup);

                if (secondGroup.IsRoundComplete() && firstGroup.IsRoundComplete())
                {
                    i = 1;
                    Console.ReadKey();
                    firstGroup.ToNextRound();
                    secondGroup.ToNextRound();
                    GameLogger.Instance.Log("\nNext Round!\n");
                }
            }
            SelectWinner(firstGroup, secondGroup);
        }

        private void Start(UnitGroup firstGroup, UnitGroup secondGroup)
        {
            GameLogger.Instance.Log(string.Format("\nStart Fighting! {0} VS {1}!", firstGroup, secondGroup));
        }

        private void FightOrHelp(IFighter fighter, UnitGroup opositeGroup, UnitGroup currentGroup)
        {
            if (fighter == null) { return; }
            TryDoUnitAction(fighter, opositeGroup, currentGroup);
        }

        private void TryDoUnitAction(IFighter fighter, UnitGroup opositeGroup, UnitGroup currentGroup, bool firstTry = false)
        {
            try
            {
                if (!fighter.IsAlive)
                {
                    return;
                }
                if (UnitAction.Random && fighter.IsHelpfull())
                {
                    var target = currentGroup.GetNotImprovedTarget();
                    if (target != null)
                    {
                        fighter.DoRandomActionByType(target, UnitActionType.HelpBuff, firstTry);
                        return;
                    }
                }
                if (UnitAction.Random)
                {
                    UnitActionType type;
                    fighter.DoRandomActionByType(GetTarget(fighter, opositeGroup, out type), type, firstTry);
                }
                else if (fighter.IsHealer() && currentGroup.IsNeedHeal())
                {
                    fighter.DoRandomActionByType(currentGroup.GetTargetWithLowerHealth(), UnitActionType.Heal, firstTry);
                }
                else
                {
                    UnitActionType type;
                    fighter.DoRandomActionByType(GetTarget(fighter, opositeGroup, out type), type, firstTry);
                }
            }
            catch (EmptyUnitActionException)
            {
            }
            catch (UnitActionNotAllowedException)
            {
                TryDoUnitAction(fighter, opositeGroup, currentGroup, true);
            }
        }

        private IUnit GetTarget(IFighter attacker, UnitGroup opositeGroup, out UnitActionType type)
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
            return opositeGroup.GetRandomTarget();
        }

        private static void SelectWinner(UnitGroup firstGroup, UnitGroup secondGroup)
        {
            var winner = firstGroup.IsSomeBodyAlive() ? firstGroup : secondGroup;
            GameLogger.Instance.Log(string.Format("{0} is the winner!", winner));
            GameLogger.Instance.Log(winner.GetAliveFighters().PrintUnits());
        }
    }
}