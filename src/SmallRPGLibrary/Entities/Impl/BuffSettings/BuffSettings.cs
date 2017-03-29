namespace SmallRPGLibrary.Entities.Impl.BuffSettings
{
    public class BuffSettings : TargetBuffSettings
    {
        public int LifeTime { get; set; }
        public string Name { get; set; }
        public int MaxCountPerUnit { get; set; }
    }
}