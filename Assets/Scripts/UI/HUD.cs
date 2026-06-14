using CivilizationBuilder.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CivilizationBuilder.UI
{
    public class HUD : MonoBehaviour
    {
        [FormerlySerializedAs("_yearText")] [SerializeField] private TextMeshProUGUI _yearValue;
        [FormerlySerializedAs("_populationText")] [SerializeField] private TextMeshProUGUI _populationValue;
        [SerializeField] private TextMeshProUGUI _settlementValue;
        [SerializeField] private TextMeshProUGUI _birthsValue;
        [SerializeField] private TextMeshProUGUI _deathsValue;
        
        private TimeService _timeService;
        private PopulationService _populationService;
        private int _birthsThisYear;
        private int _deathsThisYear;

        
        public void Initialize(TimeService timeService, PopulationService populationService)
        {
            _timeService = timeService ?? throw new System.ArgumentNullException(nameof(timeService));
            _populationService = populationService ?? throw new System.ArgumentNullException(nameof(populationService));
            
            // Event subscription so HUD updates automatically
            _timeService.OnYearAdvanced += OnYearAdvanced;
            _populationService.OnPersonBorn += _ => OnBirth();
            _populationService.OnPersonDied += _ => OnDeath();
            
            RefreshAll();
        }

        private void OnDestroy()
        {
            // Always unsubscribe when destroyed - avoid memory leaks!
            if (_timeService != null)
                _timeService.OnYearAdvanced -= OnYearAdvanced;
        }

        private void OnYearAdvanced(int year)
        {
            _birthsThisYear = 0;
            _deathsThisYear = 0;
            UpdateYear(year);
            UpdatePopulation();
            UpdateSettlement();
            UpdateBirthsDeaths();
        }

        private void OnBirth()
        {
            _birthsThisYear++;
            UpdatePopulation();
            UpdateBirthsDeaths();
        }

        private void OnDeath()
        {
            _deathsThisYear++;
            UpdatePopulation();
            UpdateBirthsDeaths();
        }
        
        private void UpdateYear(int year) => _yearValue.text = year.ToString();

        private void UpdatePopulation() =>
            _populationValue.text = _populationService.AllPeople.Count.ToString();

        private void UpdateSettlement() =>
            _settlementValue.text = GetSettlementTier();

        private void UpdateBirthsDeaths()
        {
            _birthsValue.text = $"+{_birthsThisYear}";
            _deathsValue.text = _deathsThisYear.ToString();
        }

        private string GetSettlementTier()
        {
            int pop = _populationService.AllPeople.Count;
            return pop switch
            {
                >= 1000 => "Metropolis",
                >= 500 => "City",
                >= 100 => "Town",
                >= 21 => "Village",
                _ => "Hamlet"
            };
        }

        public void RefreshAll()
        {
            UpdateYear(_timeService.CurrentYear);
            UpdatePopulation();
            UpdateSettlement();
            _birthsThisYear = 0;
            _deathsThisYear = 0;
            UpdateBirthsDeaths();
        }
    }
}