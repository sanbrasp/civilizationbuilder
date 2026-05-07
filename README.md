# CivilizationBuilder 🔨

A civilization-building game built in Unity (URP) and C#.

(_This is a personal practice project - not intended to be a work of perfection._)

---

# CivilizationBuilder

> ⚠️ **Work in Progress** — This project is in active early development. 
> Expect incomplete features, breaking changes, and general chaos. 🏗️

A civilization-building game built in Unity (URP) and C#.

---

## Concept
An RTS-style game viewed from an isometric angle where the player advances through time,
building settlements from humble hamlets to sprawling metropolises.
Features an intelligent genetics and breeding system where traits are inherited from parents,
and inbreeding causes recessive traits to surface.

---

## Features
- Settlement progression: Hamlet → Village → Town → City → Metropolis
- Genetics system with dominant, recessive, and blended trait expression
- Inbreeding coefficient tracking across up to 4 generations
- JSON save/load system
- Time progression system

---

## Tech Stack

| | |
|---|---|
| Frameworks | .NET, Unity 6 (URP) |
| Languages | C# |
| Architecture | Models / Services / Controllers |
| Serialization | Newtonsoft Json.NET |

<p align="center">
  <img src="https://img.shields.io/badge/Unity-6-black?logo=unity&logoColor=white" alt="Unity 6"/>
  <img src="https://img.shields.io/badge/C%23-.NET-purple?logo=csharp&logoColor=white" alt="C#"/>
  <img src="https://img.shields.io/badge/Render%20Pipeline-URP-blue?logo=unity&logoColor=white" alt="URP"/>
  <img src="https://img.shields.io/badge/Serialization-Newtonsoft%20Json.NET-green" alt="Newtonsoft"/>
  <img src="https://img.shields.io/badge/Status-WIP-red" alt="Status"/>
</p>

---

## Project Plan

### ✅ Phase 1 — Core Architecture
- [x] Project structure and folder layout
- [x] Data models (Person, Gene, Genome, Settlement, Building)
- [x] Genetics and inbreeding system
- [x] Population service (aging, breeding, death)
- [x] Time service
- [x] Save/load system (JSON)
- [x] Game controller

### 🔧 Phase 2 — Visuals (Current)
- [ ] Person prefab and view
- [ ] Building prefabs (house, farm, market)
- [ ] Tree and environment decoration
- [ ] Isometric camera setup
- [ ] Basic HUD (year, population count)

### 📋 Phase 3 — Gameplay
- [ ] Settlement founding and placement
- [ ] Building construction system
- [ ] Settlement tier upgrades
- [ ] Resource system
- [ ] Player input and selection

### 🎨 Phase 4 — Polish
- [ ] Name service (JSON-driven name lists)
- [ ] Visual trait expression (height, color variation)
- [ ] Sound effects and music
- [ ] Save/load UI
- [ ] Main menu

### 🌍 Phase 5 — Expansion
- [ ] Multiple settlements
- [ ] Trade between settlements
- [ ] War and conflict
- [ ] World map view

---

## License
This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.