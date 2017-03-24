using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitClasses;
using SmallRPGLibrary.Entities.Impl.UnitClasses.Range;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Services.UnitBuilders
{
    public class OrcUnitBuilder : BaseUnitBuilder
    {
        protected override Unit GetMeleeUnit(int index)
        {
            return new Berserk(Race.Orc, index);
        }

        protected override Unit GetRangeUnit(int index)
        {
            return new AxeThrower(Race.Orc, index);
        }

        protected override Unit GetWisardUnit(int index)
        {
            return new Shaman(Race.Orc, index);
        }
    }
}