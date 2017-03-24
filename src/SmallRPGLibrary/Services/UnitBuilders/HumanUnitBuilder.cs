using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitClasses;
using SmallRPGLibrary.Entities.Impl.UnitClasses.Range;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Services.UnitBuilders
{
    public class HumanUnitBuilder : BaseUnitBuilder 
    {
        protected override Unit GetMeleeUnit(int index)
        {
            return new Paladin(Race.Human, index);
        }

        protected override Unit GetRangeUnit(int index)
        {
            return new CrossBowMan(Race.Human, index);
        }

        protected override Unit GetWisardUnit(int index)
        {
            return new Mage(Race.Human, index);
        }
    }
}