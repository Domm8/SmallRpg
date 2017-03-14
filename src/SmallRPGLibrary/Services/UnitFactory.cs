using System;
using System.Collections.Generic;
using System.Linq;
using SmallRPGLibrary.Entities.Impl;
using SmallRPGLibrary.Entities.Impl.UnitClasses;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Services
{
    public static class UnitFactory
    {
        private const int WISARD_UNIT_COUNT = 1;
        private const int RANGE_UNIT_COUNT = 3;
        private const int MEELE_UNIT_COUNT = 4;
        private const int LIST_COUNT = WISARD_UNIT_COUNT + RANGE_UNIT_COUNT + MEELE_UNIT_COUNT;

        private static Unit GetWisardUnit(Race race)
        {
            switch (race)
            {
                case Race.Elf:
                case Race.Human:
                    return new Wisard(race);
                case Race.Orc:
                    return new Shaman(race);
                case Race.Undead:
                    return new Necromancer(race);
            }
            throw new ArgumentException("Trying to create Wisard unit with unknown Race.");
        }

        private static bool AddWisardUnit(this ICollection<Unit> list, Race race)
        {
            var wisardUnitCounter = list.Count(u => u is IWisard);
            if (wisardUnitCounter < WISARD_UNIT_COUNT)
            {
                list.Add(GetWisardUnit(race));
                return true;
            }
            return false;
        }
        
        private static bool AddRangeUnit(this ICollection<Unit> list, Race race)
        {
            var rangeUnitCounter = list.Count(u => u is RangeUnit);
            if (rangeUnitCounter < RANGE_UNIT_COUNT)
            {
                list.Add(race == Race.Human ? 
                    new CrossBowMan(race, rangeUnitCounter + 1) : 
                    new RangeUnit(race, rangeUnitCounter + 1));

                return true;
            }
            return false;
        }

        private static bool AddMeeleUnit(this ICollection<Unit> list, Race race)
        {
            var meeleUnitCounter = list.Count(u => u is Warrior);
            if (meeleUnitCounter < MEELE_UNIT_COUNT)
            {
                list.Add(new Warrior(race, meeleUnitCounter + 1));
                return true;
            }
            return false;
        }

        public static List<Unit> GetGroupUnits(Race race)
        {
            var unitList = new List<Unit>();
            var rangeSuccess = true;
            var wisardSuccess = true;
            var meeleSuccess = true;
            while (unitList.Count < LIST_COUNT)
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