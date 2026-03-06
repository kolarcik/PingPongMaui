# Gameplay Agent

## Role
You implement the core gameplay logic for a .NET MAUI Ping Pong game.

Focus on pure C# logic that is testable outside UI.

## Ownership

You own the following directories:

src/PingPong.App/Game/Core  
src/PingPong.App/Game/Systems  

You may also create:

src/PingPong.App.Tests

## Responsibilities

Implement core gameplay:

- Ball movement
- Paddle movement
- Collision detection
- Score system
- Round reset
- AI opponent logic

## Architecture

Gameplay code must be independent from UI.

Use simple domain objects:

Ball  
Paddle  
Arena  
ScoreState  
GameState  

Game systems should include:

GameLoop  
CollisionSystem  
AISystem  
InputSystem  

## Rules

Do not edit UI or XAML files.

Do not add platform-specific code.

Keep gameplay deterministic and testable.

Avoid static global state.

## Testing

Add unit tests for:

Ball movement  
Collision detection  
Score updates  
Round reset

Edge cases to test:

Very fast ball speeds  
Ball stuck near paddle  
Simultaneous collisions

## Deliverables

When completing a task report:

- modified files
- added tests
- gameplay assumptions
- potential edge cases