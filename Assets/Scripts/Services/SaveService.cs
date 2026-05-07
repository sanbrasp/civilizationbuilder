using System;
using System.Collections.Generic;
using System.IO;
using CivilizationBuilder.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace CivilizationBuilder.Services
{
    public class SaveService : GameServiceBase
    {
        private readonly PopulationService _populationService;
        private readonly TimeService _timeService;

        private const string SaveFileName = "savegame.json";
        private string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);


        public SaveService(PopulationService populationService, TimeService timeService)
        {
            _populationService = populationService ?? throw new ArgumentNullException(nameof(populationService));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }
        
        
        // ===========================
        // GAMESERVICEBASE OVERRIDES
        // ===========================
        
        public override void OnYearPassed(int currentYear)
        {
            
        }

        protected override void OnInitialize()
        {
            Debug.Log($"SaveService initialized. Save path: {SaveFilePath}");
        }

        protected override void OnReset()
        {
            Debug.Log($"SaveService reset.");
        }
        
        
        // =============
        // PUBLIC API
        // =============

        public void Save()
        {
            try
            {
                GameSaveData data = CollectSaveData();
                string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(SaveFilePath, json);
                Debug.Log($"Game saved to path: {SaveFilePath}.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save game: {e.Message}");
            }
        }

        public GameSaveData Load()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    Debug.LogWarning("No save file found.");
                    return null;
                }
                
                string json = File.ReadAllText(SaveFilePath);
                GameSaveData data = JsonConvert.DeserializeObject<GameSaveData>(json);
                Debug.Log($"Game loaded from {SaveFilePath}.");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load game: {e.Message}");
                return null;
            }
        }

        public bool SaveExists() => File.Exists(SaveFilePath);
        
        public void DeleteSave()
        {
            if (!SaveExists())
            {
                Debug.LogWarning("No save file found.");
                return;
            }
            File.Delete(SaveFilePath);
            Debug.Log($"Save file deleted.");
        }
        
        
        
        // ==================
        // PRIVATE HELPERS
        // ==================

        private GameSaveData CollectSaveData()
        {
            return new GameSaveData(
                _timeService.CurrentYear,
                new List<Person>(_populationService.AllPeople)
            );
        }
        
        
        
        // ==================
        // SAVE DATA MODEL
        // ==================

        [Serializable]
        public class GameSaveData
        {
            public int CurrentYear { get; init; }
            public List<Person> People { get; init; }


            public GameSaveData(int currentYear, List<Person> people)
            {
                CurrentYear = currentYear;
                People = people ?? throw new ArgumentNullException(nameof(people));
            }
        }
    }
}