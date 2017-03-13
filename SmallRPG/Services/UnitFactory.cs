using System;
using System.Collections.Generic;
using SmallRPG.Entities.Impl.UnitClasses;
using SmallRPG.Entities.Interface;
using SmallRPG.Enums;

namespace SmallRPG.Services
{
    public static class UnitFactory
    {
        private const int RANGE_UNIT_COUNT = 3;
        private const int MEELE_UNIT_COUNT = 3;

        public static IFighter GetWisardUnit(Race race)
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

        public static List<IFighter> GetRangeUnits(Race race)
        {
            var list = new List<IFighter>();
            for (var i = 0; i < RANGE_UNIT_COUNT; i++)
            {
                list.Add(new RangeUnit(race, i + 1));
            }
            return list;
        }

        public static List<IFighter> GetMeeleUnits(Race race)
        {
            var list = new List<IFighter>();
            for (var i = 0; i < MEELE_UNIT_COUNT; i++)
            {
                list.Add(new Warrior(race, i + 1));
            }
            return list;
        }

        public static List<IFighter> GetGroupUnits(Race race)
        {
            var units = GetRangeUnits(race);
            units.Add(GetWisardUnit(race));
            units.AddRange(GetMeeleUnits(race));
            return units;
        }
    }
}