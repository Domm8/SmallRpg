using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.BuffSettings
{
    public class TargetBuffSettings
    {
        public IUnit Target { get; set; }
        public IUnit BuffCaster { get; set; }
    }
}