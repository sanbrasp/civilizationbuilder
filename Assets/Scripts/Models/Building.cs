using System;

namespace CivilizationBuilder.Models
{
    public enum BuildingType
    {
        House,
        Farm,
        Market,
        Barracks,
        Temple,
        Well
    }

    [Serializable]
    public class Building
    {
        public string Id { get; init; }
        public BuildingType Type { get; init; }
        public float PositionX { get; init; }
        public float PositionZ { get; init; }
        public int ConstructedOnYear { get; init; }

        public Building(BuildingType type, float positionX, float positionZ, int currentYear)
        {
            Id = Guid.NewGuid().ToString();
            Type = type;
            PositionX = positionX;
            PositionZ = positionZ;
            ConstructedOnYear = currentYear;
        }
    }
}