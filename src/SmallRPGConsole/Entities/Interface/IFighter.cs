namespace SmallRPG.Entities.Interface
{
    public interface IFighter
    {
        bool IsAlive { get; }
        bool IsHelpfull();
        void FightWith(IUnit unit);
        void HelpTo(IUnit unit);
    }
}