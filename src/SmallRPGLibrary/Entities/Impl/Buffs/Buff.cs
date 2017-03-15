using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public abstract class Buff
    {
        protected IUnit BuffedUnit;
        protected int LifeTime { get; set; }

        public bool IsActive
        {
            get { return LifeTime > 0; }
        }

        public string Name { get; private set; }

        protected Buff(IUnit target, int lifetime, string name)
        {
            BuffedUnit = target;
            LifeTime = lifetime;
            Name = name;
        }

        public void DoFirstBuffing()
        {
            if (IsActive)
            {
                Action();
            }
        }

        protected abstract void Action();
        protected abstract void IterationAction();
        protected abstract void DeactivateAction();

        public void NextRound()
        {
            if (IsActive)
            {
                IterationAction();
                LifeTime--;
            }
        }

        public void Deactivate()
        {
            LifeTime = -1;
            DeactivateAction();
        }
    }
}