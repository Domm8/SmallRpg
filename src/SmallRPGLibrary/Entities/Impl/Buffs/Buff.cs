using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public abstract class Buff : IBuff
    {
        #region Private Fields

        private bool _isFinished;

        #endregion

        #region Protected

        protected IUnit BuffedUnit;
        protected int LifeTime { get; set; }

        #endregion

        #region Public Fields

        public bool IsActive
        {
            get { return LifeTime > 0; }
        }

        public virtual bool IsSingleAtUnit
        {
            get { return true; }
        }

        public virtual double DamageMulplier
        {
            get { return 1; }
        }

        public string Name { get; private set; }

        #endregion

        #region .ctor

        protected Buff(IUnit target, int lifetime, string name)
        {
            BuffedUnit = target;
            LifeTime = lifetime;
            Name = name;
        }

        #endregion

        #region Public Methods

        public void DoFirstBuffing()
        {
            if (IsActive)
            {
                Action();
            }
        }

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
        }

        public void FinishBuff()
        {
            if (_isFinished)
            {
                DeactivateAction();
                _isFinished = true;
            }

        }

        #endregion

        #region Not Implemented

        protected abstract void Action();
        protected abstract void IterationAction();
        protected abstract void DeactivateAction();

        #endregion

    }
}