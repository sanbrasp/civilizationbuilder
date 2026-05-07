namespace CivilizationBuilder.Services
{
    public interface IGameService
    {
        void Initialize();
        void OnYearPassed(int currentYear);
        void Reset();
    }
}