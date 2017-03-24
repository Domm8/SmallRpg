using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public abstract class Buff : IBuff
    {
        #region Private Fields

        private bool _isFinished;
        private int _lifeTime;
        private BuffCharacteristics _characteristics;

        #endregion

        #region Protected

        protected IUnit BuffedUnit;

        protected int LifeTime
        {
            get { return _lifeTime; }
            set { _lifeTime = value > -1 ? value : -1; }
        }

        #endregion

        #region Public Fields

        public bool IsActive
        {
            get { return LifeTime > 0 || LifeTime == -1; }
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

        public virtual BuffCharacteristics Characteristics
        {
            get { return _characteristics ?? (_characteristics = new BuffCharacteristics()); }
            protected set { _characteristics = value; }
        }

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
            LifeTime = 0;
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