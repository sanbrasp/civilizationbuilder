using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CivilizationBuilder.Models
{
    public enum SettlementTier
    {
        Hamlet,
        Village,
        Town,
        City,
        Metropolis
    }

    [Serializable]
    public class Settlement
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public SettlementTier Tier { get; private set; }
        public float PositionX { get; init; }
        public float PositionZ { get; init; }

        private List<string> _residentIds = new List<string>();
        private List<Building> _buildings = new List<Building>();

        public ReadOnlyCollection<string> ResidentIds => _residentIds.AsReadOnly();
        public ReadOnlyCollection<Building> Buildings => _buildings.AsReadOnly();
        public int PopulationCount => _residentIds.Count;

        public Settlement(string name, float positionX, float positionZ)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            Id = Guid.NewGuid().ToString();
            Name = name;
            Tier = SettlementTier.Hamlet;
            PositionX = positionX;
            PositionZ = positionZ;
        }

        public void AddResident(string personId)
        {
            if (string.IsNullOrEmpty(personId)) throw new ArgumentException("Person ID cannot be empty.", nameof(personId));
            if (!_residentIds.Contains(personId))
                _residentIds.Add(personId);
        }

        public void RemoveResident(string personId) => _residentIds.Remove(personId);

        public void AddBuilding(Building building)
        {
            if (building == null) throw new ArgumentNullException(nameof(building));
            _buildings.Add(building);
        }

        public void UpdateTier()
        {
            Tier = PopulationCount switch
            {
                >= 1000 => SettlementTier.Metropolis,
                >= 500  => SettlementTier.City,
                >= 100  => SettlementTier.Town,
                >= 21   => SettlementTier.Village,
                _       => SettlementTier.Hamlet
            };
        }
    }
}