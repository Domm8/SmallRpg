using System;
using SmallRPG.Enums;

namespace SmallRPG.Entities.Impl.UnitClasses
{
    public class CrossBowMan : RangeUnit
    {
        public CrossBowMan(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (unitRace != Race.Human)
            {
                throw new ArgumentException("Unit Race for class CrossBowMan could be only Human.");
            }
        }

        protected override string ClassName
        {
            get { return "Crossbowman"; }
        }
    }
}