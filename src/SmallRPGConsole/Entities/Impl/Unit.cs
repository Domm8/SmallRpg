using SmallRPG.Attributes;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using SmallRPG.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SmallRPG.Entities.Impl
{
    public abstract class Unit : IUnit, IFighter, IFormattable
    {
        private double _health;

        private double Health
        {
            get { return _health; }
            set
            {
                if (value <= 100 && value > 0)
                {
                    _health = value;
                }
                else if (value > 100)
                {
                    _health = 100;
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
            Health = 100;
        }

        protected Unit(Race unitRace, int unitIndex) : this(unitRace)
        {
            UnitIndex = unitIndex;
        }

        public void FightWith(IUnit unit)
        {
            InvokeUnitAction(UnitActionType.Attack, unit);
            ClearBuffs();
        }

        public void HelpTo(IUnit unit)
        {
            InvokeUnitAction(UnitActionType.Help | UnitActionType.Heal, unit);
            if (!unit.Equals(this))
            {
                ClearBuffs();
            }
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

        public void Healing(double health, IUnitHealer healer, string healingName)
        {
            if (IsAlive)
            {
                Health += health;

                GameLogger.Instance.Log(string.Format("{1} healed by {0} with {2}. Target unit restored {3} HP. Current unit HP {4}",
                    this, healer, healingName, health, Health));
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
                GameLogger.Instance.Log(string.Format("{0} was improved by {1}", this, caster));
                _isImproved = true;
                DamageMultiplier = DamageMultiplier * 1.5;
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not be improved by {1}, bacause he is already improved or he is dead.", this, caster));
            }
        }

        public void BecomeCursed(ICurseCaster caster)
        {
            if (IsAlive && _isImproved)
            {
                _isImproved = false;
                DamageMultiplier = DamageMultiplier / 1.5;
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

        public bool IsHelpfull()
        {
            var methodInfos = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return methodInfos.Any(m => m.GetCustomAttributes<UnitActionAttribute>()
                .Any(ua => ua.UnitActionType.Equals(UnitActionType.Help | UnitActionType.Heal)));
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
            var unitAttackMethods =
                unitActionMethods.Where(
                    m => m.GetCustomAttribute<UnitActionAttribute>().UnitActionType.Equals(actionType))
                                 .ToDictionary(m => m.Name);

            if (unitAttackMethods != null && unitAttackMethods.Count > 0)
            {
                var keys = unitAttackMethods.Keys.ToList();
                var keyIndex = new Random().Next(0, keys.Count);
                var method = unitAttackMethods[keys[keyIndex]];
                method.Invoke(this, new object[] { unit });
            }
        }

        private void ClearBuffs()
        {
            DamageMultiplier = IsLeader ? 1.5 : 1;
            _isImproved = false;
            _isDiseased = false;
        }
    }
}