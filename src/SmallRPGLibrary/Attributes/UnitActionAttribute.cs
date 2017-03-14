using System;
using SmallRPGLibrary.Enums;

namespace SmallRPGLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UnitActionAttribute : Attribute
    {
        private readonly UnitActionType _actionType;

        public UnitActionAttribute(UnitActionType type)
        {
            _actionType = type;
        }

        public UnitActionAttribute()
        {
            _actionType = UnitActionType.Attack;
        }

        public UnitActionType UnitActionType { get { return _actionType; } }
    }
}