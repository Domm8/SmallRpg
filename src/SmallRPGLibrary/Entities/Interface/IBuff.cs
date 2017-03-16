namespace SmallRPGLibrary.Entities.Interface
{
    public interface IBuff
    {
        bool IsActive { get; }
        bool IsSingleAtUnit { get; }
        string Name { get; }

        void DoFirstBuffing();
        void NextRound();
        void Deactivate();
        void FinishBuff();
    }
}