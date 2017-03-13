using System;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using SmallRPG.Services;

namespace SmallRPG.Entities.Impl
{
    public abstract class Unit : IUnit, IFighter
    {

        private double Health { get; set; }
        private bool IsDiseased { get; set; }
        private int UnitIndex { get; set; }

        protected virtual string ClassName
        {
            get
            {
                return GetType().Name;
            }
        }

        private bool IsImproved { get; set; }

        bool IFighter.IsImproved
        {
            get { return this.IsImproved; }
        }

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

        public void FightWith(IFighter unit)
        {
            if (!(unit is IUnit))
            {
                throw new ArgumentException("unit must implement IUnit interface!");
            }
            Combat((IUnit)unit);
            ClearBuffs();
        }

        public void TakeDamage(double damage, IUnit attacker, string attackName)
        {
            if (IsAlive)
            {
                Health -= damage;
                var hpLeftText = IsAlive ? string.Format("{0} HP left.", Health) : "Unit is dead.";
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
            if (IsAlive && !IsImproved)
            {
                GameLogger.Instance.Log(string.Format("{0} was improved by {1}", this, caster));
                IsImproved = true;
                DamageMultiplier = DamageMultiplier * 1.5;
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not be improved by {1}, bacause he is already improved or he is dead.", this, caster));
            }
        }

        public void BecomeCursed(ICurseCaster caster)
        {
            if (IsAlive && IsImproved)
            {
                IsImproved = false;
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
            if (IsAlive && !IsDiseased)
            {
                GameLogger.Instance.Log(string.Format("{0} was diseased by {1}", this, caster));
                IsDiseased = true;
                DamageMultiplier = DamageMultiplier * 0.5;
            }
            else
            {
                GameLogger.Instance.Log(string.Format("{0} can not be diseased by {1}, bacause he is already diseased or he is dead.", this, caster));
            }
        }

        public override string ToString()
        {
            var improvedText = IsImproved ? " (Improved)" : string.Empty;
            var diseasedText = IsDiseased ? " (Diseased)" : string.Empty;
            var indexText = UnitIndex != 0 ? "№" + UnitIndex : string.Empty;
            return string.Format("'{0} {1} {2}'{3}{4}", UnitRace, ClassName, indexText, improvedText, diseasedText);
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
            DamageMultiplier = 1;
            IsImproved = false;
            IsDiseased = false;
        }
    }
}