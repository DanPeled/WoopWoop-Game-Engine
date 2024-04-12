# WoopWoop Game Engine
> A game engine built in C# using the Raylib library.

# Importing the library:
In order to import the library using nuget use the following command:
## [NuGet](#tab/nuget)
`dotnet add package WoopWoopEngine`.

## [Package Manager](#tab/package-manager)
`NuGet\Install-Package WoopWoopEngine -Version 1.0.1`

## [Package Refrence](#tab/package-reference)
`<PackageReference Include="WoopWoopEngine" Version="1.0.1" />`

# [Packet CLI](#tab/packet-cli)
`paket add WoopWoopEngine --version 1.0.1`

# Core features:
- Entity Components System (ECS) : used to handle entity events and actions
- In-code editor built with the engine itself to provide customizability to users willing to make changes
- Event-based actions in systems
- Built in support for in-game UI
- Textures support
- Built in box-collider using SAT collision checking
