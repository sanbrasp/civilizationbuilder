using UnityEngine;
using CivilizationBuilder.Models;

namespace CivilizationBuilder.Controllers
{
    public class PersonView : MonoBehaviour
    {
        private Person _person;

        public void Initialize(Person person)
        {
            _person = person ?? throw new System.ArgumentNullException(nameof(person));
            name = $"Person_{person.Name}";
        }

        public string PersonId => _person?.Id;
    }
}