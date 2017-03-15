using System;

namespace SmallRPGLibrary.Enums
{
    [Flags]
    public enum UnitActionType
    {
        Attack = 1,
        HelpBuff = 2,
        Heal = 4,
        SelfBuff = 8,
        Curse = 16,
        Disease = 32
    }
}