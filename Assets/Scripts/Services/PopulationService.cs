using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CivilizationBuilder.Models;
using CivilizationBuilder.Services;
using UnityEngine;

namespace CivilizationBuilder.Services
{
    public class PopulationService : GameServiceBase
    {
        private readonly GeneticsService _geneticsService;
        private readonly List<Person> _allPeople = new List<Person>();

        private const int MinBreedingAge = 25;
        private const int MaxBreedingAge = 49;
        private const int MaxAge = 98;
        
        public IReadOnlyCollection<Person> AllPeople => _allPeople;

        public event Action<Person> OnPersonBorn;
        public event Action<Person> OnPersonDied;


        public PopulationService(GeneticsService geneticsService)
        {
            _geneticsService = geneticsService ?? throw new ArgumentNullException(nameof(geneticsService));
        }
        
        
        // ===========================
        // GAMESERVICEBASE OVERRIDES
        // ===========================
        protected override void OnInitialize()
        {
            Debug.Log("PopulationService initialized.");
        }

        protected override void OnReset()
        {
            _allPeople.Clear();
            Debug.Log("PopulationService reset.");
        }

        public override void OnYearPassed(int currentYear)
        {
            AgeEveryone();
            ProcessDeaths();
            ProcessBreeding(currentYear);
        }
        
        
        // =============
        // PUBLIC API
        // =============
        public void AddPerson(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (_allPeople.Exists(p => p.Id == person.Id))
            {
                Debug.LogWarning($"Person with id {person.Id} already exists in the current population.");
                return;
            }
            _allPeople.Add(person);
        }

        public Person FindById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            return _allPeople.Find(p => p.Id == id);
        }
        
        
        
        // ==================
        // PRIVATE HELPERS
        // ==================
        private void AgeEveryone()
        {
            foreach (Person person in _allPeople)
                person.AgeOneYear();
        }

        private void ProcessDeaths()
        {
            // Build the list of dead first - never modify while iterating!
            List<Person> dead = _allPeople.FindAll(p => p.Age >= MaxAge);

            foreach (Person person in dead)
            {
                _allPeople.Remove(person);
                OnPersonDied?.Invoke(person);
            }
        }

        private void ProcessBreeding(int currentYear)
        {
            List<Person> eligibleMales = _allPeople.FindAll(p => 
                p.Sex == Sex.Male && 
                p.Age >= MinBreedingAge && 
                p.Age <= MaxBreedingAge);
            
            List<Person> eligibleFemales = _allPeople.FindAll(p =>
                p.Sex == Sex.Female && 
                p.Age >= MinBreedingAge && 
                p.Age <= MaxBreedingAge);
            
            foreach (Person male in eligibleMales)
            {
                Person female = FindBreedingPartner(male, eligibleFemales);
                if (female == null) continue;
                
                Person child = CreateChild(male, female, currentYear);
                AddPerson(child);
                OnPersonBorn?.Invoke(child);
            }
        }

        private Person FindBreedingPartner(Person male, List<Person> eligibleFemales)
        {
            // Find females not directly related to the male
            List<Person> candidates = eligibleFemales.FindAll(f =>
                f.ParentAId != male.ParentAId ||
                !male.HasKnownParents ||
                !f.HasKnownParents);

            if (candidates.Count == 0) return null;
            
            // Pick random candidate
            return candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }

        private Person CreateChild(Person parentA, Person parentB, int currentYear)
        {
            Genome childGenome = _geneticsService.CombineGenome(parentA, parentB, _allPeople);
            Sex sexOfOffspring = UnityEngine.Random.value < 0.5f ? Sex.Male : Sex.Female;
            GenderExpression expressionOfOffspring = RollGenderExpression();
            string childName = GenerateName(sexOfOffspring);
            
            return new Person(childName, sexOfOffspring, expressionOfOffspring, childGenome, parentA.Id, parentB.Id);
        }

        private GenderExpression RollGenderExpression()
        {
            float roll = UnityEngine.Random.value;
            return roll switch
            {
                < 0.48f => GenderExpression.FemininePresenting,
                < 0.96f => GenderExpression.MasculinePresenting,
                _ => GenderExpression.NonBinary
            };
        }

        private string GenerateName(Sex sex)
        {
            // Placeholder for coming name service
            string[] femaleNames = { "Astrid", "Freya", "Ingrid", "Sigrid", "Hilde", "Bodil", "Hildur", "Tordis", "Emma", "Eva" };
            string[] maleNames = { "Erik",   "Bjorn", "Leif",  "Gunnar", "Ragnar", "Tore", "Geir", "Trond", "Bendik", "Mikael", "Kornelius" };
            
            return sex == Sex.Female
                ? femaleNames[UnityEngine.Random.Range(0, femaleNames.Length)]
                : maleNames[UnityEngine.Random.Range(0, maleNames.Length)];
        }
    }
}