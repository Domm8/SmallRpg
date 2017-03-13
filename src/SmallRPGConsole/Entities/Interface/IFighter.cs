namespace SmallRPG.Entities.Interface
{
    public interface IFighter
    {
        bool IsAlive { get; }
        bool IsImproved { get; }
        bool IsDiseased { get; }
        void FightWith(IUnit unit);
    }
}