# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

EasyGame is a 2D platformer game built with Unity (version 6000.2.6f2) and C#. The game is inspired by the "I Wanna" series with challenging obstacles, traps, and boss fights. It is published on Steam.

## Build Commands

This project uses the standard Unity build pipeline:
1. Open the project folder in Unity 6000.2.6f2
2. Build via File → Build Settings
3. Build scenes are configured in order: Main.unity (menu), Stage1-6.unity (gameplay stages)

No custom build scripts or CLI commands are available.

## Project Structure

```
Assets/
├── 00_Scenes/          # Main.unity + Stage1-6.unity
├── 01_Scripts/         # All game code (97 C# files)
│   ├── Player/         # Player controller, input, physics
│   ├── Platform/       # Moving platforms, special platforms
│   ├── Traps/          # 20+ trap types
│   ├── Enemy/          # 30+ enemy types with AI
│   ├── Boss/           # 5 boss implementations (BlueDragon, DemonSkull, Evil_Angel, Minotaur, Skeleton_King)
│   ├── UI/             # Camera, sound, UI management
│   └── ...
├── 02_Prefabs/         # Game object prefabs
├── 03_Sprite/          # Visual assets
├── 07_Sound/           # Audio files
└── Plugins/            # Third-party: MasterAudio, DoTween, EasySave3, Steamworks.NET
```

## Core Architecture

### Raycast-Based Physics System
The game uses custom raycast collision instead of Unity's Rigidbody2D physics to prevent fast-moving object penetration issues:

- **RaycastController.cs** - Base class managing ray origins and spacing (`skinWidth = 0.015f`)
- **Controller2D.cs** - Inherits from RaycastController; handles collision detection, slope climbing/descending, platform fall-through
- **Player.cs** - Main player controller with jump mechanics, wall jump/slide, javelin attacks

Ray counts are calculated dynamically based on collider bounds. Velocity is adjusted based on raycast distance to maintain precision.

### Layer System
- Layer 10: Player grounded
- Layer 11: Player jumping (in air)
- Layer 12: Player falling through platforms
- Layer 15: Dead objects

### Tag-Based Platform Behaviors
- `"Through"` - One-way platforms (fall-through enabled)
- `"CompulsionJump"` - Spring platforms with forced boost
- `"Slide"` - Slippery surfaces
- `"SlideWall"` - Wall-slideable surfaces

### Platform Passenger System
`PlatformController.cs` uses Dictionary caching to track and move objects riding on moving platforms. Waypoint-based movement with easing interpolation.

### Input System
`KeyManager.cs` manages configurable key bindings via `KeyAction` enum:
```csharp
public enum KeyAction { UP, DOWN, LEFT, RIGTH, ATTACK, JUMP, RESET, KEYCOUNT }
```
Note: `RIGTH` is intentionally misspelled in the codebase (legacy).

### Data Persistence
Uses Easy Save 3 (ES3) for saving:
- Player position: `"PlayerX"`, `"PlayerY"`
- Progress: `"StageIndex"`, `"DeathCount"`, `"ResetCount"`
- Key bindings: Stored by KeyAction enum names

## Third-Party Dependencies

| Package | Purpose |
|---------|---------|
| Master Audio (DarkTonic) | Complete audio management, playlists, SFX |
| DoTween (Demigiant) | Animation tweening, fade effects |
| Easy Save 3 | Data persistence |
| Steamworks.NET | Steam API, achievements |
| TextMesh Pro | UI text rendering |

## Key Patterns

- **Object Pooling**: Javelin attack uses 10 pooled objects
- **Waypoint Movement**: Platforms follow predefined waypoint sequences with configurable easing
- **Checkpoint System**: Integrated with GameManager for respawn handling
- **Boss Structure**: Each boss has its own subdirectory under `Boss/` with attack pattern scripts
