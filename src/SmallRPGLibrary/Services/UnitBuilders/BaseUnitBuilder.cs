using System.Collections.Generic;
using System.Linq;
using SmallRPGLibrary.Consts;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitClasses;
using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Services.UnitBuilders
{
    public abstract class BaseUnitBuilder : IUnitBuilder
    {
        public bool AddWisardUnit(ICollection<Unit> list, int maxCount = DefaultValues.WISARD_UNIT_COUNT)
        {
            var wisardUnitCounter = list.Count(u => u is IWizard);
            if (wisardUnitCounter < maxCount)
            {
                list.Add(GetWisardUnit(wisardUnitCounter + 1));
                return true;
            }
            return false;
        }

        public bool AddRangeUnit(ICollection<Unit> list, int maxCount = DefaultValues.RANGE_UNIT_COUNT)
        {
            var rangeUnitCounter = list.Count(u => u is RangeUnit);
            if (rangeUnitCounter < maxCount)
            {
                list.Add(GetRangeUnit(rangeUnitCounter + 1));

                return true;
            }
            return false;
        }

        public bool AddMeleeUnit(ICollection<Unit> list, int maxCount = DefaultValues.MEELE_UNIT_COUNT)
        {
            var meeleUnitCounter = list.Count(u => u is MeleeUnit);
            if (meeleUnitCounter < maxCount)
            {
                list.Add(GetMeleeUnit(meeleUnitCounter + 1));
                return true;
            }
            return false;
        }

        protected abstract Unit GetMeleeUnit(int index);
        protected abstract Unit GetRangeUnit(int index);
        protected abstract Unit GetWisardUnit(int index);
    }
}