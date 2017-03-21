using System.Collections.Generic;
using SmallRPGLibrary.Consts;
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

        #region Private

        private double _health;

        private int MaxHealth
        {
            get { return (int)(DefaultValues.UNIT_MAX_HEALTH + Characteristics.Stamina/6); }
        }

        private readonly List<IBuff> _buffList;

        private bool _isLeader;

        private double _multiplier = 1;

        private int UnitIndex { get; set; }

        #endregion

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
                if (value <= MaxHealth && value > 0)
                {
                    _health = value;
                }
                else if (value > MaxHealth)
                {
                    _health = MaxHealth;
                }
                else
                {
                    _health = 0;
                }
            }
        }

        public Race UnitRace { get; private set; }

        public bool CanMove { get; private set; }

        public abstract Characteristics Characteristics { get; }

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
            Health = MaxHealth;
            _multiplier = 1;
            CanMove = true;
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

        public bool IsBuffedBy(Type buffType) 
        {
            return _buffList.Exists(buffType.IsInstanceOfType);
        }

        public void DoRandomActionByType(IUnit unit, UnitActionType actionType)
        {
            RemoveUnActiveBuffs();
            InvokeUnitAction(actionType, unit);
            CanMove = false;
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

        public bool DeactivateBuff(Type buffType)
        {
            if (IsAlive && IsBuffedBy(buffType))
            {
                _buffList.Where(buffType.IsInstanceOfType).ToList().ForEach(b => b.Deactivate());
                _buffList.RemoveAll(buffType.IsInstanceOfType);
                return true;
            }
            return false;
        }

        public void BecomeLeader()
        {
            if (IsAlive && !_isLeader)
            {
                GameLogger.Instance.Log(string.Format("{0} become a Leader", this));
                _isLeader = true;
                _multiplier = 1.15;
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

        public void ToNextRound()
        {
            CanMove = true;
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

        #region ToString

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

        #endregion

        #region Private Methods

        private string UnitToString()
        {
            var buffNames = _buffList.Select(b => string.Format("({0})", b.Name)).ToArray();
            var leaderText = _isLeader ? " (Leader)" : string.Empty;
            var indexText = UnitIndex != 0 ? " №" + UnitIndex : string.Empty;
            return string.Format("'{0} {1}{2}'{3}{4}", UnitRace, ClassName, indexText, string.Join(" ", buffNames),
                                 leaderText);
        }

        private void IterateBuffs()
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
            IterateBuffs();
        }

        #endregion

    }
}