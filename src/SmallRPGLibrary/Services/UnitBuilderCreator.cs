using System;
using SmallRPGLibrary.Enums;
using SmallRPGLibrary.Services.UnitBuilders;

namespace SmallRPGLibrary.Services
{
    public class UnitBuilderCreator
    {
        public static IUnitBuilder Create(Race race)
        {
            switch (race)
            {
                case Race.Elf:
                    return new ElfUnitBuilder();
                case Race.Human:
                    return new HumanUnitBuilder();
                case Race.Orc:
                    return new OrcUnitBuilder();
                case Race.Undead:
                    return new UndeadUnitBuilder();
            }
            throw new ArgumentException("Trying to create IUnitBuilder instance with unknown Race.");
        }
    }
}