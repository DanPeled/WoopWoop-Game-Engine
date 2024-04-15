# WoopWoop Game Engine
[![GitHub](https://img.shields.io/badge/GitHub-000000?style=flat&logo=github)](https://github.com/DanPeled/WoopWoop-Game-Engine)
[![Docs](https://img.shields.io/badge/docs-visit-blue)](https://danpeled.github.io/WoopWoop-Game-Engine/annotated.html)
[![Version](https://badgen.net/nuget/v/WoopWoopEngine)](https://www.nuget.org/packages/WoopWoopEngine/) ![](https://img.shields.io/badge/language-C%23-blue) ![](https://img.shields.io/badge/.NET-7.0-blue
) ![](https://img.shields.io/badge/-Raylib-orange)
> A game engine built in C# using the Raylib library.

# Importing the library:
In order to import the library using nuget use the following command: `dotnet add package WoopWoopEngine`.

# Core features:
- Entity Components System (ECS) : used to handle entity events and actions
- In-code editor built with the engine itself to provide customizability to users willing to make changes
- Event-based actions in systems
- Built in support for in-game UI
- Textures support
- Built in box-collider using SAT collision checking

# Change Logs
## 1.0.1.1
- Added a CollisionData class in order to collect data about collisions, which are accessed by the `BoxCollider::GetCollisionData(Entity)` method.
- Added audio support, used by the AudioClip class.
- Refactoring of collection of the `currentFrameEntities` array in the `Engine` in order to reduce function calls.
- Added an `OnEntityInstantiated` event in the `Entity` class.
- Added the [Fody](https://github.com/Fody/Fody) library for future features.

## 1.0.0 & 1.0.1 (same version, accidentaly uploaded twice)
- Entity Components System (ECS) : used to handle entity events and actions
- In-code editor built with the engine itself to provide customizability to users willing to make changes
- Event-based actions in systems
- Built in support for in-game UI
- Textures support
- Built in box-collider using SAT collision checking