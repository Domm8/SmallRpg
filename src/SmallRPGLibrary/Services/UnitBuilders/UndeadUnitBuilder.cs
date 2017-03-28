using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Entities.Impl.UnitClasses;
using SmallRPGLibrary.Entities.Impl.UnitClasses.Range;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Services.UnitBuilders
{
    public class UndeadUnitBuilder : BaseUnitBuilder
    {
        protected override Unit GetMeleeUnit(int index)
        {
            return new DeathKnight(Race.Undead, index);
        }

        protected override Unit GetRangeUnit(int index)
        {
            return new SkeletBowman(Race.Undead, index);
        }

        protected override Unit GetWisardUnit(int index)
        {
            return new Necromancer(Race.Undead, index);
        }
    }
}