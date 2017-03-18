using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.Buffs;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Attributes;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Wisard : Unit, IUnitImprover, IWisard
    {

        protected virtual double Damage
        {
            get
            {
                var damage = 4;
                switch (UnitRace)
                {
                    case Race.Elf:
                        damage = 10;
                        break;
                    case Race.Human:
                        damage = 6;
                        break;
                }
                return damage * DamageMultiplier;
            }
        }

        protected virtual string MagicAttackName
        {
            get { return "Magic"; }
        }

        public Wisard(Race unitRace, int unitIndex)
            : base(unitRace, unitIndex)
        {
        }

        [UnitAction(UnitActionType.HelpBuff)]
        public void ImproveUnit(IUnit unit)
        {
            unit.AddBuff(new ImprovementBuff(unit, this));
        }

        [UnitAction(UnitActionType.Attack)]
        public void MagicAttack(IUnit unit)
        {
            unit.TakeDamage(Damage, this, MagicAttackName);
        }
    }
}
