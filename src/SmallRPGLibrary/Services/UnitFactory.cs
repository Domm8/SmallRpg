using System;
using System.Collections.Generic;
using SmallRPGLibrary.Consts;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services.Interfaces;

namespace SmallRPGLibrary.Services
{
    public class UnitFactory : IUnitFactory
    {
        public List<Unit> GetGroupUnits(Race race)
        {
            var unitList = new List<Unit>();
            var rangeSuccess = true;
            var wisardSuccess = true;
            var meeleSuccess = true;
            while (unitList.Count < DefaultValues.UNIT_LIST_COUNT)
            {
                var random = new Random().Next(1, 4);
                if (rangeSuccess && (random == 1 || (!wisardSuccess && !meeleSuccess)))
                {
                    rangeSuccess = unitList.AddRangeUnit(race);
                }
                if (meeleSuccess && (random == 2 || (!rangeSuccess && !wisardSuccess)))
                {
                    meeleSuccess = unitList.AddMeeleUnit(race);
                }
                if (wisardSuccess && (random == 3 || (!rangeSuccess && !meeleSuccess)))
                {
                    wisardSuccess = unitList.AddWisardUnit(race);
                }
            }

            return unitList;
        }
    }
}