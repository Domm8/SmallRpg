using System;
using System.Globalization;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using SmallRPG.Services;

namespace SmallRPG.Entities.Impl
{
    public abstract class Unit : IUnit, IFighter, IFormattable
    {

        private double Health { get; set; }
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

        public abstract void Combat(IUnit unit);

        public void FightWith(IUnit unit)
        {
            Combat(unit);
            ClearBuffs();
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

        private string UnitToString()
        {
            var improvedText = _isImproved ? " (Improved)" : string.Empty;
            var diseasedText = _isDiseased ? " (Diseased)" : string.Empty;
            var leaderText = _isLeader ? " (Leader)" : string.Empty;
            var indexText = UnitIndex != 0 ? " №" + UnitIndex : string.Empty;
            return string.Format("'{0} {1}{2}'{5}{3}{4}", UnitRace, ClassName, indexText, improvedText, diseasedText, leaderText);
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

        private void ClearBuffs()
        {
            DamageMultiplier = IsLeader ? 1.5 : 1;
            _isImproved = false;
            _isDiseased = false;
        }
    }
}