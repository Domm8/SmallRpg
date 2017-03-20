using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmallRPGLibrary.Consts;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitClasses;
using SmallRPGLibrary.Entities.Interface;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Services
{
    public static class ExtensionHelper
    {
        public static string PrintUnits(this IEnumerable<IUnit> list)
        {
            var textBuilder = new StringBuilder();
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                textBuilder.AppendFormat("\t{0:HP}\n", enumerator.Current);
            }
            return textBuilder.ToString();
        }

        public static void SelectLider(this List<Unit> list)
        {
            if (!list.Any(u => u.IsLeader))
            {
                list.ToList().GetRandomUnit().BecomeLeader();
            }
        }


        public static Unit GetWisardUnit(Race race, int unitIndex)
        {
            switch (race)
            {
                case Race.Elf:
                    return new Druid(race, unitIndex);
                case Race.Human:
                    return new Wisard(race, unitIndex);
                case Race.Orc:
                    return new Shaman(race, unitIndex);
                case Race.Undead:
                    return new Necromancer(race, unitIndex);
            }
            throw new ArgumentException("Trying to create Wisard unit with unknown Race.");
        }

        public static bool AddWisardUnit(this ICollection<Unit> list, Race race)
        {
            var wisardUnitCounter = list.Count(u => u is IWisard);
            if (wisardUnitCounter < DefaultValues.WISARD_UNIT_COUNT)
            {
                list.Add(GetWisardUnit(race, wisardUnitCounter + 1));
                return true;
            }
            return false;
        }

        public static bool AddRangeUnit(this ICollection<Unit> list, Race race)
        {
            var rangeUnitCounter = list.Count(u => u is RangeUnit);
            if (rangeUnitCounter < DefaultValues.RANGE_UNIT_COUNT)
            {
                list.Add(race == Race.Human ?
                    new CrossBowMan(race, rangeUnitCounter + 1) :
                    new RangeUnit(race, rangeUnitCounter + 1));

                return true;
            }
            return false;
        }

        public static bool AddMeeleUnit(this ICollection<Unit> list, Race race)
        {
            var meeleUnitCounter = list.Count(u => u is Warrior);
            if (meeleUnitCounter < DefaultValues.MEELE_UNIT_COUNT)
            {
                list.Add(new Warrior(race, meeleUnitCounter + 1));
                return true;
            }
            return false;
        }

        public static Unit GetRandomUnit(this List<Unit> list)
        {
            if (list.Count <= 0)
            {
                return null;
            }
            var random = new Random();
            var bigIndex1 = random.Next(0, list.Count * 25);
            var bigIndex2 = random.Next(0, list.Count * 25) * random.Next(0, 2);
            var bigIndex3 = random.Next(0, list.Count * 25) * random.Next(0, 2);
            var bigIndex4 = random.Next(0, list.Count * 25) * random.Next(0, 2);
            var index = (bigIndex1 + bigIndex2 + bigIndex3 + bigIndex4) / 100;
            return list[index];
        }

        public static bool HasAnyFlag(this Enum value, Enum flags)
        {
            return value != null && ((Convert.ToInt32(value) & Convert.ToInt32(flags)) != 0);
        }

        public static bool HasFlag(this Enum value, Enum flags)
        {
            var f = Convert.ToInt32(flags);
            return value != null && ((Convert.ToInt32(value) & f) == f);
        }
    }
}