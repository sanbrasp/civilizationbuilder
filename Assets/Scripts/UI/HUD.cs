using CivilizationBuilder.Services;
using TMPro;
using UnityEngine;

namespace CivilizationBuilder.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _yearText;
        [SerializeField] private TextMeshProUGUI _populationText;
        
        private TimeService _timeService;
        private PopulationService _populationService;

        public void Initialize(TimeService timeService, PopulationService populationService)
        {
            _timeService = timeService ?? throw new System.ArgumentNullException(nameof(timeService));
            _populationService = populationService ?? throw new System.ArgumentNullException(nameof(populationService));
            
            // Event subscription so HUD updates automatically
            _timeService.OnYearAdvanced += OnYearAdvanced;
            _populationService.OnPersonBorn += _ => UpdatePopulation();
            _populationService.OnPersonDied += _ => UpdatePopulation();
            
            // Initial state
            UpdateYear(_timeService.CurrentYear);
            UpdatePopulation();
        }

        private void OnDestroy()
        {
            // Always unsubscribe when destroyed - avoid memory leaks!
            if (_timeService != null)
                _timeService.OnYearAdvanced -= OnYearAdvanced;
        }
        
        private void OnYearAdvanced(int year) => UpdateYear(year);
        
        private void UpdateYear(int year) => _yearText.text = $"Year: {year}";

        private void UpdatePopulation() =>
            _populationText.text = $"Population: {_populationService.AllPeople.Count}";

        public void RefreshAll()
        {
            UpdateYear(_timeService.CurrentYear);
            UpdatePopulation();
        }
    }
}