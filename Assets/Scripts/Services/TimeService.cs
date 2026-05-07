using System;
using System.Collections.Generic;
using UnityEngine;

namespace CivilizationBuilder.Services
{
    public class TimeService : GameServiceBase
    {
        private readonly List<IGameService> _gameServices = new List<IGameService>();
        private int _currentYear;
        
        public int CurrentYear =>  _currentYear;
        public event Action<int> OnYearAdvanced;

        private const int StartingYear = 1;
        
        
        // ===========================
        // GAMESERVICEBASE OVERRIDES
        // ===========================
        
        public override void OnYearPassed(int currentYear)
        {
            
        }

        protected override void OnInitialize()
        {
            _currentYear = StartingYear;
            Debug.Log($"TimeService initialized. Starting year: {_currentYear}");
        }

        protected override void OnReset()
        {
            _currentYear = StartingYear;
            _gameServices.Clear();
            Debug.Log($"TimeService reset.");
        }
        
        
        // =============
        // PUBLIC API
        // =============

        public void RegisterService(IGameService gameService)
        {
            if (gameService == null) throw new ArgumentNullException(nameof(gameService));
            if (_gameServices.Contains(gameService))
            {
                Debug.LogWarning($"TimeService already registered. Ignoring {gameService.GetType().Name}");
                return;
            }
            _gameServices.Add(gameService);
        }

        public void AdvanceYear()
        {
            _currentYear++;
            Debug.Log($"Year {_currentYear} begins.");
            
            foreach (IGameService service in _gameServices)
                service.OnYearPassed(_currentYear);
            
            OnYearAdvanced?.Invoke(_currentYear);
        }

        public void SetYear(int year)
        {
            if (year < 1) throw new ArgumentException("Year must be greater than or equal to 1.",  nameof(year));
            
            _currentYear = year;
        }
    }
}