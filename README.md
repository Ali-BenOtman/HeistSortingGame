# Heist Sorting Game

A solo-built Unity 2D endless arcade game — sort stolen bills into the correct bags before the clock catches up with you.

## Concept

You're sorting cash mid-heist. Bills of different denominations fly in one at a time, each needing a specific swipe direction to land in its matching bag. No levels, no end — it's an endless survival format, chasing your own high score under constant timer pressure. Set in a vault environment, with a cartoony, vibrant art direction inspired by *Royal Kingdom*.

## Core Mechanics

- **Sorting** — each bill has a value and a correct swipe direction. Sort correctly to score and gain time; sort wrong and lose a chunk of time plus multiplier progress.
- **Timer** — an elastic countdown: passive drain always ticking, a small time gain per correct sort, a larger penalty per wrong sort. The timer's *cap* shrinks as your score climbs, so a run tightens the longer you survive — correct play still feels safe, but the margin for error shrinks over time.
- **Multiplier** — builds on sorting streaks, drops a level on mistakes, decays if you sit idle too long, and rewards fast consecutive sorts with a speed bonus.
- **Obstacles** — fake bills mixed in roughly every 20–30 bills. Tap one correctly for a bonus; swipe it by mistake and your multiplier resets.
- **Power-ups** — Time Freeze, Double Multiplier, and Auto Sort spawn periodically through a run.

## Tech Stack

- Unity 6 (2D)
- C#
- Unity's New Input System (touch on device, mouse in-editor)
- ScriptableObjects for bill / obstacle / power-up data
- Target platforms: iOS & Android, portrait (1080×1920)

## Status

Actively in development, core-loop-first.

- ✅ Phase 1 — Core loop (spawning, swipe sorting, scoring, timer, UI)
- ✅ Phase 2 — Advanced mechanics (multiplier system, difficulty scaling, fake-bill obstacles)
- 🔧 Phase 3 — Polish & power-ups (three power-ups built; narrative polish and audio still pending)
- 🔧 Currently reworking difficulty scaling to be driven by score rather than elapsed time
- ⏳ Phase 4 — Retention & monetization systems (planned)
- ⏳ Art & animation — not yet started; current build uses placeholder assets, held off intentionally until the core loop is fully tuned and feels right

## Running the Project

1. Clone the repo
2. Open it in Unity Hub (Unity 6.x)
3. Open the main scene
4. Press Play

## Project Structure

```
Assets/
  Scripts/
    BillData.cs, ObstacleData.cs, PowerUpData.cs   — ScriptableObject data definitions
    BillDisplay.cs                                  — visual setup for whatever's currently on screen
    SwipeDetector.cs, TapDetector.cs                — input handling
    BillSpawner.cs, SortingManager.cs               — core spawn/sort loop
    ObstacleManager.cs, PowerUpManager.cs           — fake bills & power-up logic
    ScoreSystem.cs, TimerSystem.cs, DifficultyManager.cs  — scoring, timer, difficulty curve
    FeedbackManager.cs, GameStateManager.cs, UIManager.cs — feedback, state, HUD
```
