using SmallRPG.Entities.Interface;
using SmallRPG.Enums;
using System;

namespace SmallRPG.Entities.Impl.UnitClasses
{
    public class Shaman : Unit, IUnitImprover, ICurseCaster
    {
        public Shaman(Race unitRace) : base(unitRace)
        {
            if (!IsDarkRace && UnitRace == Race.Undead)
            {
                throw new ArgumentException("Unit Race for class Shaman could be only Orc.");
            }
        }

        public override void Combat(IUnit unit)
        {
            if (unit.IsFrendlyUnit(this))
            {
                ImproveUnit(unit);
            }
            else
            {
                CastCurse(unit);
            }
        }

        public void ImproveUnit(IUnit unit)
        {
            unit.BecomeImproved(this);
        }

        public void CastCurse(IUnit unit)
        {
            unit.BecomeCursed(this);
        }
    }
}
