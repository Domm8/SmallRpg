using System;
using SmallRPG.Enums;

namespace SmallRPG.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
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