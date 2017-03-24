﻿using System;
using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Entities.Impl.UnitClasses
{
    public class Warrior: MeleeUnit
    {
        public Warrior(Race unitRace, int unitIndex) : base(unitRace, unitIndex)
        {
            if (unitRace != Race.Elf)
            {
                throw new ArgumentException("Unit Race for class Warrior could be only Elf.");
            }
        }

        protected override AttackParameters MeleeAttackParams
        {
            get
            {
                return new AttackParameters("Mortal Strike", 16);
            }
        }
    }
}