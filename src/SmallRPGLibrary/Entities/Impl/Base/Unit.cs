using System.Collections.Generic;
using SmallRPGLibrary.Consts;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Impl.Base
{
    public abstract class Unit : IUnit, IFighter, IFormattable
    {
        #region Fields And Priperties

        private double _health;
        private readonly List<IBuff> _buffList;
        private bool _isLeader;
        private double _multiplier = 1;

        private int UnitIndex { get; set; }

        public bool IsLeader
        {
            get { return _isLeader; }
        }

        public bool IsAlive
        {
            get { return Health > 0; }
        }

        public double Health
        {
            get { return _health; }
            private set
            {
                if (value <= DefaultValues.UNIT_MAX_HEALTH && value > 0)
                {
                    _health = value;
                }
                else if (value > DefaultValues.UNIT_MAX_HEALTH)
                {
                    _health = DefaultValues.UNIT_MAX_HEALTH;
                }
                else
                {
                    _health = 0;
                }
            }
        }

        public Race UnitRace { get; private set; }

        protected bool IsDarkRace
        {
            get { return UnitRace == Race.Orc || UnitRace == Race.Undead; }
        }

        protected double DamageMultiplier
        {
            get
            {
                var multiplier = _multiplier;
                _buffList.ForEach(b => multiplier = b.DamageMulplier * multiplier);
                return multiplier;
            }
        }

        protected virtual string ClassName
        {
            get { return GetType().Name; }
        }

        #endregion

        #region .ctor

        protected Unit(Race unitRace)
        {
            UnitRace = unitRace;
            Health = DefaultValues.UNIT_MAX_HEALTH;
            _multiplier = 1;
            _buffList = new List<IBuff>();
        }

        protected Unit(Race unitRace, int unitIndex) : this(unitRace)
        {
            UnitIndex = unitIndex;
        }

        #endregion

        public bool IsBuffedBy<T>() where T : IBuff
        {
            return _buffList.Exists(b => b is T);
        }

        public void FightWith(IUnit unit, UnitActionType actionType)
        {
            RemoveUnActiveBuffs();
            InvokeUnitAction(actionType, unit);
        }

        public void HelpTo(IUnit unit)
        {
            RemoveUnActiveBuffs();
            InvokeUnitAction(UnitActionType.HelpBuff, unit);
        }

        public void HealUnit(IUnit unit)
        {
            RemoveUnActiveBuffs();
            InvokeUnitAction(UnitActionType.Heal, unit);
        }

        #region Public State change Methods

        public void TakeDamage(double damage, IUnit attacker, string attackName)
        {
            if (IsAlive)
            {
                Health -= damage;
                var hpLeftText = IsAlive
                                     ? string.Format("{0} HP left.", Health)
                                     : string.Format("Unit {0} is dead.", this);
                GameLogger.Instance.Log(string.Format("{1} attacked {0} with {2}. Target unit loose {3} HP. {4}",
                                                      this, attacker, attackName, damage, hpLeftText));
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be attacked by {1}, bacause he is already dead.", this, attacker),
                    LogLevel.Warn);
            }

        }

        public void LooseHealth(double damage, string attackName)
        {
            if (IsAlive)
            {
                Health -= damage;
                var hpLeftText = IsAlive
                                     ? string.Format("{0} HP left.", Health)
                                     : string.Format("Unit {0} is dead.", this);
                GameLogger.Instance.Log(string.Format("{0} unit loose {2} HP because of {1}. {3}",
                                                      this, attackName, damage, hpLeftText));
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} is already dead.", this), LogLevel.Warn);
            }
        }

        public void Healing(double health, IUnitHealer healer, string healingName)
        {
            if (IsAlive)
            {
                Health += health;

                GameLogger.Instance.Log(
                    string.Format("{0} healed by {1} with {2}. Target unit restored {3} HP. Current unit HP {4}",
                                  this, healer, healingName, health, Health), LogLevel.Heal);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be healed by {1}, bacause he is already dead.", this, healer),
                    LogLevel.Warn);
            }
        }

        public void RestoreHealth(double health, string healingName)
        {
            if (IsAlive)
            {
                Health += health;
                GameLogger.Instance.Log(string.Format("{0} unit loose {2} HP because of {1}. {3} HP left.",
                                                      this, healingName, health, Health));
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} is already dead.", this), LogLevel.Warn);
            }
        }

        public void BecomeCursed(ICurseCaster caster)
        {
            if (IsAlive && IsBuffedBy<ImprovementBuff>())
            {
                _buffList.OfType<ImprovementBuff>().ToList().ForEach(b => b.Deactivate());
                GameLogger.Instance.Log(string.Format("{0} was cursed by {1}", this, caster), LogLevel.Improve);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be cursed by {1}, bacause he is not improved or he is dead.", this,
                                  caster), LogLevel.Warn);
            }
        }

        public void BecomeLeader()
        {
            if (IsAlive && !_isLeader)
            {
                GameLogger.Instance.Log(string.Format("{0} become a Leader", this));
                _isLeader = true;
                _multiplier = 1.5;
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not become a leader!", this));
            }
        }

        #endregion

        public bool IsFrendlyUnit(IUnit unit)
        {
            if (IsDarkRace)
            {
                return unit.UnitRace == Race.Orc || unit.UnitRace == Race.Undead;
            }
            else
            {
                return unit.UnitRace == Race.Elf || unit.UnitRace == Race.Human;
            }
        }

        public void AddBuff(IBuff buff)
        {
            if (buff.IsSingleAtUnit && _buffList.Any(b => b.GetType() == buff.GetType()))
            {
                return; // added for not buffing unit with the same buff twice
            }
            buff.DoFirstBuffing();
            _buffList.Add(buff);
        }

        public bool IsHelpfull()
        {
            var methodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return methodInfos.Any(m => m.GetCustomAttributes<UnitActionAttribute>()
                .Any(ua => ua.UnitActionType.HasAnyFlag(UnitActionType.HelpBuff)));
        }

        public bool IsHealer()
        {
            return GetType()
                    .GetCustomAttributes<UnitActionAttribute>()
                    .Any(ua => ua.UnitActionType.HasAnyFlag(UnitActionType.Heal));
        }

        private void InvokeUnitAction(UnitActionType actionType, IUnit unit)
        {
            var methodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var unitActionMethods = methodInfos.Where(m => m.GetCustomAttributes<UnitActionAttribute>().Any());
            var unitMethods =
                unitActionMethods.Where(
                    m => m.GetCustomAttribute<UnitActionAttribute>().UnitActionType.HasAnyFlag(actionType))
                                 .ToDictionary(m => m.Name);
            try
            {
                if (unitMethods != null && unitMethods.Count > 0)
                {
                    var keys = unitMethods.Keys.ToList();
                    var keyIndex = new Random().Next(0, keys.Count);
                    var method = unitMethods[keys[keyIndex]];
                    method.Invoke(this, new object[] { unit });
                }
                else
                {
                    GameLogger.Instance.Log(string.Format("Error occured unitMethods is empty!"), LogLevel.Error);
                }
            }
            catch (Exception)
            {
                GameLogger.Instance.Log(string.Format("Error occured while invocing one of unitMethods!"), LogLevel.Error);
            }

        }

        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }
            if (format.ToUpperInvariant() == "HP")
            {
                return string.Format("{0} {1} HP Left", UnitToString(), Health.ToString("##.####", formatProvider));
            }
            return UnitToString();
        }

        #region Private Methods

        private string UnitToString()
        {
            var buffNames = _buffList.Select(b => string.Format("({0})", b.Name)).ToArray();
            var leaderText = _isLeader ? " (Leader)" : string.Empty;
            var indexText = UnitIndex != 0 ? " №" + UnitIndex : string.Empty;
            return string.Format("'{0} {1}{2}'{3}{4}", UnitRace, ClassName, indexText, string.Join(" ", buffNames),
                                 leaderText);
        }

        private void ToNextRound()
        {
            _buffList.ForEach(b =>
                {
                    if (b.IsActive)
                    {
                        b.NextRound();
                    }
                });
        }

        private void RemoveUnActiveBuffs()
        {
            _buffList.ForEach(b =>
                {
                    if (!b.IsActive)
                    {
                        b.FinishBuff();
                    }
                });
            _buffList.RemoveAll(b => !b.IsActive);
            ToNextRound();
        }

        #endregion

    }
}