using System.Collections.Generic;
using SmallRPGLibrary.Entities.Impl.Base;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Services.Interfaces
{
    public interface IUnitFactory
    {
        List<Unit> GetGroupUnits(Race race);
    }
}