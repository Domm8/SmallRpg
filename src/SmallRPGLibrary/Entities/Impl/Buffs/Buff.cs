using SmallRPGLibrary.Entities.Impl.UnitFeatures;
using SmallRPGLibrary.Entities.Interface;

namespace SmallRPGLibrary.Entities.Impl.Buffs
{
    public abstract class Buff : IBuff
    {
        #region Private Fields

        private bool _isFinished;
        private int _lifeTime;
        private int _maxCountPerUnit = 1;
        private BuffCharacteristics _characteristics;

        #endregion

        #region Protected

        protected IUnit BuffedUnit { get; set; }
        protected IUnit BuffCaster { get; set; }

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

        public int MaxCountPerUnit
        {
            get { return _maxCountPerUnit; }
            protected set { _maxCountPerUnit = value <= 0 ? 1 : value; }
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

        protected Buff(BuffSettings.BuffSettings settings)
        {
            BuffedUnit = settings.Target;
            LifeTime = settings.LifeTime;
            Name = settings.Name;
            MaxCountPerUnit = settings.MaxCountPerUnit;
            BuffCaster = settings.BuffCaster;
        }

        protected Buff(IUnit target, int lifeTime, string name)
        {
            BuffedUnit = target;
            LifeTime = lifeTime;
            Name = name;
        }

        protected Buff(IUnit target, int lifeTime, string name, int maxCountPerUnit) 
            : this(target, lifeTime, name)
        {
            MaxCountPerUnit = maxCountPerUnit;
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