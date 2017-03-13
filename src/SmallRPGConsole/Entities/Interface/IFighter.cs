namespace SmallRPG.Entities.Interface
{
    public interface IFighter
    {
        bool IsAlive { get; }
        bool IsImproved { get; }
        void FightWith(IFighter unit);
    }
}