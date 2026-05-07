using CivilizationBuilder.Models;
using CivilizationBuilder.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CivilizationBuilder.Controllers
{
    public class GameController : MonoBehaviour
    {
        // ==================
        // SERVICES
        // ==================
        private GeneticsService _geneticsService;
        private PopulationService _populationService;
        private TimeService _timeService;
        private SaveService _saveService;
        
        // ==================
        // UNITY LIFECYCLE
        // ==================
        private void Awake()
        {
            InitializeServices();
        }

        private void Start()
        {
            SeedStartingPopulation();
        }
        
        
        // ==================
        // PUBLIC API
        // ==================
        public void AdvanceYear()
        {
            _timeService.AdvanceYear();
        }

        public void SaveGame()
        {
            _saveService.Save();
        }

        public void LoadGame()
        {
            SaveService.GameSaveData data = _saveService.Load();
            if (data == null) return;
            
            _populationService.Reset();
            _populationService.Initialize();
            
            foreach (Person person in data.People)
                _populationService.AddPerson(person);

            _timeService.SetYear(data.CurrentYear);
            
            Debug.Log($"Game loaded. Year: {data.CurrentYear}, Population: {data.People.Count}.");
        }
        
        
        // ==========
        // CONTROLS
        // ==========
        private void Update()
        {
            // TODO: Expensive method invocation
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                AdvanceYear();
            // TODO: Expensive method invocation
            if (Keyboard.current.sKey.wasPressedThisFrame)
                SaveGame();
            // TODO: Expensive method invocation
            if (Keyboard.current.lKey.wasPressedThisFrame)
                LoadGame();
        }
        
        
        // ==================
        // PRIVATE HELPERS
        // ==================
        private void InitializeServices()
        {
            // Build services:
            _geneticsService = new GeneticsService();
            _populationService = new PopulationService(_geneticsService);
            _timeService = new TimeService();
            _saveService = new SaveService(_populationService, _timeService);
            
            _timeService.RegisterService(_populationService);
            
            // Register services that react to time:
            _populationService.OnPersonBorn += OnPersonBorn;
            _populationService.OnPersonDied += OnPersonDied;
            
            // Initialize all services:
            _geneticsService.Initialize();
            _populationService.Initialize();
            _timeService.Initialize();
            _saveService.Initialize();
        }

        private void SeedStartingPopulation()
        {
            // Create a few founding people with randomized genomes
            for (int i = 0; i < 10; i++)
            {
                Sex sex = i % 2 == 0 ? Sex.Male : Sex.Female;
                GenderExpression gender = GenderExpression.FemininePresenting;
                Genome genome = GenomeFactory.CreateRandom();
                string name = sex == Sex.Male ? "Founder" : "Foundress";

                Person person = new Person($"{name} {i + 1}", sex, gender, genome);
                _populationService.AddPerson(person);
            }
            Debug.Log($"Starting population seeded with {_populationService.AllPeople.Count} people.");
        }
        
        
        
        // ==================
        // EVENT HANDLERS
        // ==================
        private void OnPersonBorn(Person person)
        {
            Debug.Log($"[Event] {person.Name} was born.");
        }

        private void OnPersonDied(Person person)
        {
            Debug.Log($"[Event] {person.Name} has died at age {person.Age}.");
        }
    }
}