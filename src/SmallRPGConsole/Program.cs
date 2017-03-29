using System;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services;

namespace SmallRPGConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 35);
            var random = new Random();
            var firstRace = random.Next(0, 100) > 49 ? Race.Human : Race.Human;
            var secondRace = random.Next(0, 100) > 49 ? Race.Undead : Race.Undead;
            var firstGroup = new UnitGroup(firstRace);
            var secondGroup = new UnitGroup(secondRace);
            var autoFightService = new AutoFightService();

            var firstAttackFirst = random.Next(0, 100) > 49;
            if (firstAttackFirst)
            {
                autoFightService.StartOrderedFighting(firstGroup, secondGroup);
            }
            else
            {
                autoFightService.StartOrderedFighting(secondGroup, firstGroup);
            }

            Console.ReadKey();
        }
    }
}
