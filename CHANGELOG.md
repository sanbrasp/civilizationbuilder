# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

---

## [Unreleased]
### Planned
- Person prefab and view
- Building prefabs (house, farm, market)
- Tree and environment decoration
- Isometric camera setup
- Basic HUD (year, population count)

---

## [0.1.0] - 2025-05-07
### Added
- Project architecture and folder structure
- Data models: `Person`, `Gene`, `Genome`, `GenomeFactory`, `Settlement`, `Building`, `GameSaveData`
- Genetics system with dominant, recessive, and blended trait expression
- Inbreeding coefficient tracking across up to 4 generations
- `IGameService` interface and `GameServiceBase` abstract class
- `GeneticsService` — genome combination and inbreeding logic
- `PopulationService` — aging, breeding, and death
- `TimeService` — year progression and service orchestration
- `SaveService` — JSON save and load via Newtonsoft Json.NET
- `GameController` — Unity MonoBehaviour wiring all services together
- `GenomeFactory` — random genome generation for founding population
- CI/CD pipeline: CodeQL, Gitleaks, Semgrep, Dependabot
- MIT License