namespace SmallRPGLibrary.Entities.Interface
{
    public interface IFighter
    {
        bool IsAlive { get; }
        bool IsHelpfull();
        bool IsHealer();
        void FightWith(IUnit unit);
        void HelpTo(IUnit unit);
        void HealUnit(IUnit unit);
    }
}