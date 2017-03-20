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
                return 8 * DamageMultiplier;
            }
        }

        protected virtual string MagicAttackName
        {
            get { return "Magic"; }
        }

        public override Characteristics Characteristics
        {
            get
            {
                return new Characteristics
                {
                    Stamina = 1,
                    Speed = 5,
                };
            }
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
