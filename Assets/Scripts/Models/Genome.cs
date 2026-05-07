using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace CivilizationBuilder.Models
{
    [Serializable]
    public class GenePair
    {
        [JsonProperty]public Gene AlleleA { get; init; }
        [JsonProperty]public Gene AlleleB { get; init; }

        public GenePair(Gene alleleA, Gene alleleB)
        {
            AlleleA = alleleA ?? throw new ArgumentNullException(nameof(alleleA));
            AlleleB = alleleB ?? throw new ArgumentNullException(nameof(alleleB));
        }

        public float GetExpressedValue()
        {
            if (AlleleA.Expression == GeneExpression.Blended)
                return (AlleleA.DominantValue + AlleleB.DominantValue) / 2f;

            bool aIsDominant = AlleleA.Expression == GeneExpression.Dominant;
            bool bIsDominant = AlleleB.Expression == GeneExpression.Dominant;

            if (aIsDominant || bIsDominant)
                return aIsDominant ? AlleleA.DominantValue : AlleleB.DominantValue;

            return AlleleA.RecessiveValue;
        }
    }

    [Serializable]
    public class Genome
    {
        [JsonProperty("GenePairs")]
        private List<GenePair> _genePairs = new List<GenePair>();

        [JsonIgnore]
        public ReadOnlyCollection<GenePair> GenePairs => _genePairs.AsReadOnly();

        public void AddGenePair(GenePair pair)
        {
            if (pair == null) throw new ArgumentNullException(nameof(pair));
            _genePairs.Add(pair);
        }

        public GenePair GetTrait(string traitName)
        {
            if (string.IsNullOrWhiteSpace(traitName))
                throw new ArgumentException("Trait name cannot be empty.", nameof(traitName));

            return _genePairs.Find(p => p.AlleleA.TraitName == traitName);
        }
    }
}