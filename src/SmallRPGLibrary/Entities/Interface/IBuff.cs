using SmallRPGLibrary.Entities.Impl.UnitFeatures;

namespace SmallRPGLibrary.Entities.Interface
{
    public interface IBuff
    {
        bool IsActive { get; }
        int MaxCountPerUnit { get; }
        string Name { get; }
        BuffCharacteristics Characteristics { get; }

        void DoFirstBuffing();
        void NextRound();
        void Deactivate();
        void FinishBuff();
    }
}