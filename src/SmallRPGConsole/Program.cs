using System;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Enums;

namespace SmallRPGConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 35);
            var random = new Random();
            var firstRace = random.Next(0, 100) > 49 ? Race.Human : Race.Human;
            var secondRace = random.Next(0, 100) > 49 ? Race.Orc : Race.Orc;
            var firstGroup = new UnitGroup(firstRace);
            var secondGroup = new UnitGroup(secondRace);

            var firstAttackFirst = random.Next(0, 100) > 49;
            if (firstAttackFirst)
            {
                firstGroup.StartOrderedFighting(secondGroup);
            }
            else
            {
                secondGroup.StartOrderedFighting(firstGroup);
            }

            Console.ReadKey();
        }
    }
}
