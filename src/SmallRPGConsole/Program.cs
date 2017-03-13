using System;
using SmallRPG.Entities.Impl;
using SmallRPG.Enums;

namespace SmallRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 35);
            var random = new Random();
            var firstRace = random.Next(0, 100) > 49 ? Race.Elf : Race.Human;
            var secondRace = random.Next(0, 100) > 49 ? Race.Orc : Race.Undead;
            var firstGroup = new UnitGroup(firstRace);
            var secondGroup = new UnitGroup(secondRace);

            var firstAttackFirst = random.Next(0, 100) > 49;
            if (firstAttackFirst)
            {
                firstGroup.StartFighting(secondGroup);
            }
            else
            {
                secondGroup.StartFighting(firstGroup);
            }

            Console.ReadKey();
        }
    }
}
