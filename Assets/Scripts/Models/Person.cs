using System;
using Newtonsoft.Json;

namespace CivilizationBuilder.Models
{
    public enum Sex { Male, Female }
    public enum GenderExpression { FemininePresenting, MasculinePresenting, NonBinary }

    [Serializable]
    public class Person
    {
        [JsonProperty]public string Id { get; init; }
        [JsonProperty]public string Name { get; private set; }
        [JsonProperty]public int Age { get; private set; }
        [JsonProperty]public Sex Sex { get; init; }
        [JsonProperty]public GenderExpression Gender { get; init; }
        [JsonProperty]public Genome Genome { get; init; }
        [JsonProperty]public string ParentAId { get; init; }
        [JsonProperty]public string ParentBId { get; init; }
        [JsonProperty]public string SettlementId { get; private set; }

        public bool HasKnownParents => ParentAId != null && ParentBId != null;

        public Person(string name, Sex sex, GenderExpression gender, Genome genome,
            string parentAId = null, string parentBId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            Id = Guid.NewGuid().ToString();
            Name = name;
            Age = 0;
            Sex = sex;
            Gender = gender;
            Genome = genome ?? throw new ArgumentNullException(nameof(genome));
            ParentAId = parentAId;
            ParentBId = parentBId;
        }

        [JsonConstructor]
        public Person(string id, string name, int age, Sex sex, GenderExpression gender,
            Genome genome, string parentAId, string parentBId, string settlementId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            
            Id = id;
            Name = name;
            Age = age;
            Sex = sex;
            Gender = gender;
            Genome = genome ?? throw new ArgumentNullException(nameof(genome));
            ParentAId = parentAId;
            ParentBId = parentBId;
            SettlementId = settlementId;
        }

        public void AgeOneYear() => Age++;
        public void MoveToSettlement(string settlementId)
        {
            if (string.IsNullOrWhiteSpace(settlementId))
                throw new ArgumentException("Settlement ID cannot be empty.", nameof(settlementId));
            SettlementId = settlementId;
        }
        public void LeaveSettlement() => SettlementId = null;
    }
}