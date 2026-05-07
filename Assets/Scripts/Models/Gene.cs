using System;
using Newtonsoft.Json;

namespace CivilizationBuilder.Models
{
    public enum GeneExpression
    {
        Dominant,
        Recessive,
        Blended
    }

    [Serializable]
    public class Gene
    {
        [JsonProperty]public string TraitName { get; init; }
        [JsonProperty]public float DominantValue { get; init; }
        [JsonProperty]public float RecessiveValue { get; init; }
        [JsonProperty]public GeneExpression Expression { get; init; }

        public Gene(string traitName, float dominantValue, float recessiveValue, GeneExpression expression)
        {
            if (string.IsNullOrWhiteSpace(traitName))
                throw new ArgumentException("Trait name cannot be empty.", nameof(traitName));

            TraitName = traitName;
            DominantValue = dominantValue;
            RecessiveValue = recessiveValue;
            Expression = expression;
        }
    }
}