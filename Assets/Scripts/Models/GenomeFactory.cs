using UnityEngine;

namespace CivilizationBuilder.Models
{
    public static class GenomeFactory
    {
        public static Genome CreateRandom()
        {
            Genome genome = new Genome();
            
            genome.AddGenePair(CreateTrait("Height", 1.8f, 1.5f));
            genome.AddGenePair(CreateTrait("Strength", 1.0f, 0.5f));
            genome.AddGenePair(CreateTrait("Intelligence", 1.0f, 0.5f));
            genome.AddGenePair(CreateTrait("DiseaseResist", 1.0f, 0.3f));
            genome.AddGenePair(CreateTrait("Fertility", 1.0f, 0.4f));
            
            return genome;
        }

        private static GenePair CreateTrait(string name, float dominantValue, float recessiveValue)
        {
            Gene alleleA = new Gene(name, dominantValue, recessiveValue, RollExpression());
            Gene alleleB = new Gene(name, dominantValue, recessiveValue, RollExpression());
            
            return new GenePair(alleleA, alleleB);
        }

        private static GeneExpression RollExpression()
        {
            float roll = Random.value;
            return roll switch
            {
                < 0.5f => GeneExpression.Dominant,
                < 0.85f => GeneExpression.Recessive,
                _ => GeneExpression.Blended
            };
        }
    }
}