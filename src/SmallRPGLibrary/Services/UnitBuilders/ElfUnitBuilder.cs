using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitClasses;
using SmallRPGLibrary.Entities.Impl.UnitClasses.Range;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Services.UnitBuilders
{
    public class ElfUnitBuilder : BaseUnitBuilder
    {
        protected override Unit GetMeleeUnit(int index)
        {
            return new Warrior(Race.Elf, index);
        }

        protected override Unit GetRangeUnit(int index)
        {
            return new Hunter(Race.Elf, index);
        }

        protected override Unit GetWisardUnit(int index)
        {
            return new Druid(Race.Elf, index);
        }
    }
}