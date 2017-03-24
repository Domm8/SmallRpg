using System;
using SmallRPGLibrary.Attributes;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class DeathKnight : MeleeUnit
    {
        public DeathKnight(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (unitRace != Race.Undead)
            {
                throw new ArgumentException("Unit Race for class DeathKnight could be only Undead.");
            }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get
            {
                return new AttackParameters("Death Strike", 16);
            }
        }

        protected override string ClassName
        {
            get { return "Death Knight"; }
        }

        [UnitAction]
        public void Infection(IUnit target)
        {
            target.AddBuff(new HarmfulBuff(target, 3, "Infection", this, 9, 4));
        }
    }
}