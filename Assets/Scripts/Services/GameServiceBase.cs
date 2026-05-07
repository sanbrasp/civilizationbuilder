using UnityEngine;

namespace CivilizationBuilder.Services
{
    public abstract class GameServiceBase : IGameService
    {
        protected bool IsInitialized { get; private set; }
        
        public void Initialize()
        {
            if (IsInitialized)
            {
                Debug.LogWarning($"{GetType().Name} is already initialized!");
                return;
            }

            OnInitialize();
            IsInitialized = true;
        }

        public void Reset()
        {
            OnReset();
            IsInitialized = false;
        }

        
        // Subclasses override these to provide specific behavior
        public abstract void OnYearPassed(int currentYear);
        protected abstract void OnInitialize();
        protected abstract void OnReset();
    }
}