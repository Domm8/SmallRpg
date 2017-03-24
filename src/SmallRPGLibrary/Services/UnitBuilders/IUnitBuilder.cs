using System.Collections.Generic;
using SmallRPGLibrary.Consts;
using SmallRPGLibrary.Entities.Impl.Base;

namespace SmallRPGLibrary.Services.UnitBuilders
{
    public interface IUnitBuilder
    {
        bool AddMeleeUnit(ICollection<Unit> list, int maxCount = DefaultValues.MEELE_UNIT_COUNT);
        bool AddRangeUnit(ICollection<Unit> list, int maxCount = DefaultValues.RANGE_UNIT_COUNT);
        bool AddWisardUnit(ICollection<Unit> list, int maxCount = DefaultValues.WISARD_UNIT_COUNT);
    }
}