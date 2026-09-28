# Heist Sorting Game

A solo-built Unity 2D endless arcade game — sort stolen bills into the correct bags before the clock catches up with you. **In active development.**

## Concept

You're sorting cash mid-heist. Bills fly in one at a time, each needing a specific swipe direction to land in its matching bag. No levels — endless survival, chasing your own high score under constant timer pressure. Set in a vault environment, cartoony/vibrant art direction inspired by *Royal Kingdom*.

## Core Mechanics

- **Sorting** — each bill has a value (displayed) and a separate, decoupled score value, so score reflects skill rather than which bill happened to spawn.
- **Elastic timer** — a fixed passive drain, a fixed gain on a correct sort, a larger fixed penalty on a wrong sort. Only the timer's *cap* shrinks as difficulty rises — never the drain rate or the reward/penalty amounts — so a mistake costs a growing share of your buffer without any single variable spiraling out of tuning range.
- **Difficulty driver** — shrinks the cap based on total in-run actions (bills, obstacles, power-ups — correct or wrong), not score. This was a deliberate redesign: an earlier score-driven version would have let any future points multiplier accidentally accelerate difficulty. Calibrated against real playtest data, not guessed numbers.
- **Multiplier** — builds on sorting streaks, drops on mistakes, decays if idle, rewards fast consecutive sorts.
- **Obstacles & power-ups** — fake bills mixed in periodically; three power-ups (Time Freeze, Double Multiplier, Auto Sort) spawn through a run.
- **Persistent high score** — saved locally, survives app restarts.

## Technical Highlights

A few things worth calling out for anyone reading this as more than a feature list:

- **Iterative difficulty-system design.** Went through two full architectural revisions — time-based → score-based → action-count-based — each driven by a concrete flaw found through actual playtesting or a spotted exploit vector, not aesthetic preference. Final tuning was verified against real telemetry (playtest pace data), not assumed.
- **Centralized pause-state ownership.** Settings and Pause both needed to freeze the game, from multiple entry points. An early version let each system independently touch `Time.timeScale`, causing state conflicts when one closed while the other was still open. Refactored to a single owner (`PauseManager`) — a direct application of the Single Responsibility Principle to a real bug, not just a textbook example.
- **Event-driven architecture throughout.** Game state transitions (`Start`, `Game Over`, `Quit to Menu`) broadcast via UnityEvents; independent systems (scoring, difficulty, UI, power-ups) subscribe and react without direct coupling to each other.
- **Root-cause debugging on several real issues**, including a UI raycast-ordering bug (Settings panel losing click priority depending on which menu opened it, fixed via explicit sibling-order control) and a per-run state leak (obstacle-spawn timing silently carrying over between runs instead of resetting).

## Tech Stack

- Unity 6 (2D), C#
- Unity's New Input System (touch on device, mouse in-editor)
- ScriptableObjects for bill / obstacle / power-up data
- Git/GitHub for version control
- Target platforms: iOS & Android, portrait (1080×1920)

## Status

Core loop, menus, and persistence are built and tested. Not yet released.

- ✅ Core sorting loop, scoring, timer, multiplier
- ✅ Difficulty scaling (action-count driven, tuned against real playtest data)
- ✅ Full menu system — Main Menu, Pause, Settings, enhanced Game Over screen with persisted high score
- ✅ Obstacles, power-ups, mobile lifecycle handling (auto-pause on app backgrounding), Android back button
- 🔧 Audio — plumbing built, placeholder/free clips being sourced
- ⏳ Custom art pass — deliberately held back until core loop was validated; that gate has now been cleared
- ⏳ Store submission — accounts, device builds, review

## Running the Project

1. Clone the repo
2. Open in Unity Hub (Unity 6.x)
3. Open the main scene, press Play

## Project Structure

```
Assets/
  Scripts/
    BillData.cs, ObstacleData.cs, PowerUpData.cs   — ScriptableObject data definitions
    BillDisplay.cs                                  — visual setup for whatever's currently on screen
    SwipeDetector.cs, TapDetector.cs                — input handling
    BillSpawner.cs, SortingManager.cs               — core spawn/sort loop
    ObstacleManager.cs, PowerUpManager.cs           — fake bills & power-up logic
    ScoreSystem.cs, TimerSystem.cs, DifficultyManager.cs  — scoring, elastic timer, difficulty curve
    GameStateManager.cs                             — state machine (Idle / Playing / GameOver), event dispatch
    UIManager.cs, MainMenuManager.cs, PauseManager.cs, SettingsManager.cs — UI & pause-state ownership
    AudioManager.cs, BackButtonHandler.cs           — audio plumbing, Android back-button routing
    FeedbackManager.cs                              — correct/wrong visual feedback
```
