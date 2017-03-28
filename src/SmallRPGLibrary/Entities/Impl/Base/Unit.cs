﻿using System.Collections.Generic;
using SmallRPGLibrary.Consts;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Exceptions;

namespace SmallRPGLibrary.Entities.Impl.Base
{
    public abstract class Unit : IUnit, IFighter, IFormattable
    {
        #region Fields And Priperties

        #region Private

        private double _health;

        private int MaxHealth
        {
            get { return (int)(DefaultValues.UNIT_MAX_HEALTH + FullCharacteristics.Stamina / 6); }
        }

        private readonly List<IBuff> _buffList = new List<IBuff>();

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

        protected abstract Characteristics Characteristics { get; }

        public Characteristics FullCharacteristics
        {
            get
            {
                var buffCharacteristics = Characteristics ?? new Characteristics();
                _buffList.ForEach(b => buffCharacteristics = buffCharacteristics + b.Characteristics);
                return buffCharacteristics;
            }
        }

        protected bool IsDarkRace
        {
            get { return UnitRace == Race.Orc || UnitRace == Race.Undead; }
        }

        protected double DamageMultiplier
        {
            get
            {
                var multiplier = _multiplier;
                _buffList.ForEach(b => multiplier = b.Characteristics.DamageMultiplier * multiplier);
                return multiplier;
            }
        }

        protected double HealMultiplier
        {
            get
            {
                var multiplier = _multiplier;
                _buffList.ForEach(b => multiplier = b.Characteristics.HealMultiplier * multiplier);
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

        public void DoRandomActionByType(IUnit unit, UnitActionType actionType, bool sameActionTry = false)
        {
            if (!sameActionTry)
            {
                RemoveUnActiveBuffs();
            }
            InvokeRandomUnitAction(actionType, unit);
            CanMove = false;
        }

        public double CountUnitAttackDamage(double damage)
        {
            return damage*DamageMultiplier;
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
                string message;
                if (attacker != null)
                {
                    message = string.Format("{1} attacked {0} with {2}. Target unit loose {3} HP. {4}", this, attacker,
                        attackName, damage, hpLeftText);
                }
                else
                {
                    message = string.Format("-- {0} unit loose {2} HP because of {1}. {3}", this, attackName, damage,
                        hpLeftText);
                }
                GameLogger.Instance.Log(message);
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("-- {0} can not be attacked {1}, bacause he is already dead.", this,
                        attacker != null ? "by " + attacker : ""), LogLevel.Warn);
            }

        }

        public void TakeDamage(double damage, string attackName)
        {
            TakeDamage(damage, null, attackName);
        }

        public void RestoreHealth(double health, IUnitHealer healer, string healingName)
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
                GameLogger.Instance.Log(string.Format("-- {0} unit restore {2} HP because of {1}. {3} HP left.",
                                                      this, healingName, health, Health), LogLevel.Heal);
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
                throw new UnitActionNotAllowedException("IBuff property IsSingleAtUnit is true! And Unit already hav such buff. BuffType: " + buff.GetType()); 
                // added for not buffing unit with the same buff twice
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

        #region Invoke Unit Actions

        private void InvokeRandomUnitAction(UnitActionType actionType, IUnit unit)
        {
            var unitMethods = GetUnitActionDictionary(actionType);
            if (unitMethods.Count == 0)
            {
                GameLogger.Instance.Log("Error occured unitMethods is empty!", LogLevel.Error);
                throw new UnitActionNotAllowedException("unitMethods is empty");
            }
            var keys = unitMethods.Keys.ToList();
            var keyIndex = new Random().Next(0, keys.Count);
            var method = unitMethods[keys[keyIndex]];
            InvokeAction(method, unit);
        }

        private Dictionary<string, MethodInfo> GetUnitActionDictionary(UnitActionType actionType)
        {
            var methodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var unitActionMethods = methodInfos.Where(m => m.GetCustomAttributes<UnitActionAttribute>().Any());
            var unitMethods =
                unitActionMethods.Where(
                    m => m.GetCustomAttribute<UnitActionAttribute>().UnitActionType.HasAnyFlag(actionType))
                    .ToDictionary(m => m.Name);
            return unitMethods;
        }

        private void InvokeAction(MethodInfo method, params object[] methodParameters)
        {
            try
            {
                method.Invoke(this, methodParameters);
            }
            catch (TargetInvocationException exc)
            {
                if (exc.InnerException != null && exc.InnerException is UnitActionNotAllowedException)
                {
                    GameLogger.Instance.Log("UnitActionNotAllowedException exception was thrown!", LogLevel.Warn);
                    throw exc.InnerException;
                }
            }
            catch (Exception)
            {
                GameLogger.Instance.Log("Error occured while invocing one of unitMethods!", LogLevel.Error);
            }
        }

        #endregion

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
            var buffNames =
                _buffList.GroupBy(b => b.Name)
                    .Select(g => string.Format("({0}{1})", g.Key, g.Count() > 1 ? g.Count().ToString() : ""))
                    .ToArray();
            var leaderText = _isLeader ? " (Leader)" : string.Empty;
            var indexText = UnitIndex != 0 ? " №" + UnitIndex : string.Empty;
            return string.Format("'{0} {1}{2}'{3}{4}", UnitRace, ClassName, indexText, string.Join(" ", buffNames),
                                 leaderText);
        }
		
        private void RemoveUnActiveBuffs()
        {
            _buffList.ForEach(b =>
                {
                    if (!b.IsActive)
                    {
                        b.FinishBuff();
                    }
                    else
                    {
                        b.NextRound();
                    }
                });
            _buffList.RemoveAll(b => !b.IsActive);
        }

        #endregion

    }
}