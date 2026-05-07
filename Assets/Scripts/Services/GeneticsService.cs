using System;
using System.Collections.Generic;
using CivilizationBuilder.Models;
using UnityEngine;
using Random = System.Random;

namespace CivilizationBuilder.Services
{
    public class GeneticsService : GameServiceBase
    {
        private readonly Random _random = new Random();

        // How many generations back we check for shared ancestors
        private const int InBreedingGenerationDepth = 4;
        
        // Above this coefficient, recessive traits have a chance to force-express
        private const float InBreedingThreshold = 0.125f;
        
        
        // ============
        // PUBLIC API
        // ============

        public Genome CombineGenome(Person parentA, Person parentB, List<Person> allPeople)
        {
            if (parentA == null) throw new ArgumentNullException(nameof(parentA));
            if (parentB == null) throw new ArgumentNullException(nameof(parentB));
            if (allPeople == null) throw new ArgumentNullException(nameof(allPeople));

            float inbreedingCoefficient = CalculateInbreedingCoefficient(parentA, parentB, allPeople);
            Genome childGenome = new Genome();

            foreach (GenePair pairA in parentA.Genome.GenePairs)
            {
                GenePair pairB = parentB.Genome.GetTrait(pairA.AlleleA.TraitName);
                if (pairB == null) continue;

                Gene alleleFromA = PickAllele(pairA);
                Gene alleleFromB = PickAllele(pairB);
                Gene expressedA = ApplyInbreeding(alleleFromA, inbreedingCoefficient);
                Gene expressedB = ApplyInbreeding(alleleFromB, inbreedingCoefficient);
                
                childGenome.AddGenePair(new GenePair(expressedA, expressedB));
            }
            return childGenome;
        }

        public float CalculateInbreedingCoefficient(Person personA, Person personB, List<Person> allPeople)
        {
            if (personA == null) throw new ArgumentNullException(nameof(personA));
            if (personB == null) throw new ArgumentNullException(nameof(personB));
            if (allPeople == null) throw new ArgumentNullException(nameof(allPeople));

            HashSet<string> ancestorsA = GetAncestorIds(personA, allPeople, InBreedingGenerationDepth);
            HashSet<string> ancestorsB = GetAncestorIds(personB, allPeople, InBreedingGenerationDepth);
            
            ancestorsA.IntersectWith(ancestorsB);
            int sharedAncestors = ancestorsA.Count;
            
            // Each shared ancestor contributes 0.125 (one great-grandparent = 12.5%)
            // Clamped to 1.0 as an absolute maximum
            return Math.Min(sharedAncestors * 0.125f, 1.0f);
        }
        
        
        //====================
        // PRIVATE HELPERS
        //====================
        private Gene PickAllele(GenePair pair)
        {
            // 50/50 chance of passing down either allele
            return _random.NextDouble() < 0.5 ? pair.AlleleA : pair.AlleleB;
        }

        private Gene ApplyInbreeding(Gene gene, float coefficient)
        {
            // If coefficient is below threshold - no effect
            if (coefficient < InBreedingThreshold) return gene;
            
            // Above - chance to flip a dominant gene to recessive - higher coefficient, higher chance
            bool flipToRecessive = _random.NextDouble() < coefficient
                                   && gene.Expression == GeneExpression.Dominant;
            
            if (!flipToRecessive) return gene;

            return new Gene(
                gene.TraitName,
                gene.DominantValue,
                gene.RecessiveValue,
                GeneExpression.Recessive
            );
        }

        private HashSet<string> GetAncestorIds(Person person, List<Person> allPeople, int depth)
        {
            HashSet<string> ancestors = new HashSet<string>();
            if (depth == 0 || !person.HasKnownParents) return ancestors;
            
            Person parentA = allPeople.Find(p => p.Id == person.ParentAId);
            Person parentB = allPeople.Find(p => p.Id == person.ParentBId);

            if (parentA != null)
            {
                ancestors.Add(parentA.Id);
                ancestors.UnionWith(GetAncestorIds(parentA, allPeople, depth - 1));
            }

            if (parentB != null)
            {
                ancestors.Add(parentB.Id);
                ancestors.UnionWith(GetAncestorIds(parentB, allPeople, depth - 1));
            }
            return ancestors;
        }

        
        // ---------------------------------------------------------------
        // GameServiceBase overrides
        // ---------------------------------------------------------------
        public override void OnYearPassed(int currentYear)
        {
            
        }

        protected override void OnInitialize()
        {
            Debug.Log("GeneticsService initialized");
        }

        protected override void OnReset()
        {
            Debug.Log("GeneticsService reset");
        }
    }
}