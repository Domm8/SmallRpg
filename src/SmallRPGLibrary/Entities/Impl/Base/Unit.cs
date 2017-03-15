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
        private double _health;
        private readonly List<Buff> _buffList;

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

        private bool _isImproved;
        private bool _isDiseased;
        private bool _isLeader;

        private int UnitIndex { get; set; }

        protected virtual string ClassName
        {
            get
            {
                return GetType().Name;
            }
        }

        public bool IsImproved { get { return _isImproved; } }
        public bool IsLeader { get { return _isLeader; } }
        public bool IsDiseased { get { return _isDiseased; } }

        public bool IsAlive
        {
            get { return Health > 0; }
        }

        public Race UnitRace { get; private set; }

        protected bool IsDarkRace
        {
            get
            {
                return UnitRace == Race.Orc || UnitRace == Race.Undead;
            }
        }

        protected double DamageMultiplier { get; set; }

        protected Unit(Race unitRace)
        {
            UnitRace = unitRace;
            DamageMultiplier = 1;
            Health = DefaultValues.UNIT_MAX_HEALTH;
            _buffList = new List<Buff>();
        }

        protected Unit(Race unitRace, int unitIndex) : this(unitRace)
        {
            UnitIndex = unitIndex;
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

        public void TakeDamage(double damage, IUnit attacker, string attackName)
        {
            if (IsAlive)
            {
                Health -= damage;
                var hpLeftText = IsAlive ? string.Format("{0} HP left.", Health) : string.Format("Unit {0} is dead.", this);
                GameLogger.Instance.Log(string.Format("{1} attacked {0} with {2}. Target unit loose {3} HP. {4}",
                    this, attacker, attackName, damage, hpLeftText));
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not be attacked by {1}, bacause he is already dead.", this, attacker));
            }
           
        }

        public void LooseHealth(double damage, string attackName)
        {
            if (IsAlive)
            {
                Health -= damage;
                var hpLeftText = IsAlive ? string.Format("{0} HP left.", Health) : string.Format("Unit {0} is dead.", this);
                GameLogger.Instance.Log(string.Format("{0} unit loose {2} HP because of {1}. {3}",
                    this, attackName, damage, hpLeftText));
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} is already dead.", this));
            }
           
        }

        public void Healing(double health, IUnitHealer healer, string healingName)
        {
            if (IsAlive)
            {
                Health += health;

                GameLogger.Instance.Log(string.Format("{0} healed by {1} with {2}. Target unit restored {3} HP. Current unit HP {4}",
                    this, healer, healingName, health, Health), LogLevel.Heal);
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not be healed by {1}, bacause he is already dead.", this, healer));
            }
        }

        public void BecomeImproved(IUnitImprover caster)
        {
            if (IsAlive && !_isImproved)
            {
                GameLogger.Instance.Log(string.Format("{0} was improved by {1}", this, caster), LogLevel.Improve);
                _isImproved = true;
                DamageMultiplier = DamageMultiplier * 1.5;
            }
            else
            {
                GameLogger.Instance.Log(
                    string.Format("{0} can not be improved by {1}, bacause he is already improved or he is dead.", this,
                                  caster), LogLevel.Improve);
            }
        }

        public void BecomeUnImproved()
        {
            if (IsAlive && _isImproved)
            {
                _isImproved = false;
                DamageMultiplier = DamageMultiplier / 1.5;
            }
        }

        public void BecomeUnDiseased()
        {
            if (IsAlive && _isDiseased)
            {
                _isDiseased = false;
                DamageMultiplier = DamageMultiplier / 0.5;
            }
        }

        public void BecomeCursed(ICurseCaster caster)
        {
            if (IsAlive && _isImproved)
            {
                BecomeUnImproved();
                GameLogger.Instance.Log(string.Format("{0} was cursed by {1}", this, caster));
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not be cursed by {1}, bacause he is not improved or he is dead.", this, caster));
            }
        }

        public void BecomeDiseased(IDiseaseCaster caster)
        {
            if (IsAlive && !_isDiseased)
            {
                GameLogger.Instance.Log(string.Format("{0} was diseased by {1}", this, caster));
                _isDiseased = true;
                DamageMultiplier = DamageMultiplier * 0.5;
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not be diseased by {1}, bacause he is already diseased or he is dead.", this, caster));
            }
        }

        public void BecomeLeader()
        {
            if (IsAlive && !_isLeader)
            {
                GameLogger.Instance.Log(string.Format("{0} become a Leader", this));
                _isLeader = true;
                DamageMultiplier = DamageMultiplier * 1.5;
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not become a leader!", this));
            }
        }

        public void Inspiration(IUnit unit)
        {
            if (IsAlive && !_isLeader)
            {
                GameLogger.Instance.Log(string.Format("{1} was inspired by a Leader {0}", this, unit));
                _isLeader = true;
                DamageMultiplier = DamageMultiplier * 1.5;
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not become a leader!", this));
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

        public void AddBuff(Buff buff)
        {
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

        private string UnitToString()
        {
            var improvedText = _isImproved ? " (Improved)" : string.Empty;
            var diseasedText = _isDiseased ? " (Diseased)" : string.Empty;
            var leaderText = _isLeader ? " (Leader)" : string.Empty;
            var indexText = UnitIndex != 0 ? " №" + UnitIndex : string.Empty;
            return string.Format("'{0} {1}{2}'{5}{3}{4}", UnitRace, ClassName, indexText, improvedText, diseasedText, leaderText);
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

        private void ClearBuffs()
        {
            DamageMultiplier = IsLeader ? 1.5 : 1;
            _isImproved = false;
            _isDiseased = false;
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
                        b.Deactivate();
                    }
                });
            _buffList.RemoveAll(b => !b.IsActive);
            ToNextRound();
        }
    }
}