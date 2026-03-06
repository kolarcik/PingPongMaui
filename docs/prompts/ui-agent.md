# UI Agent

## Role
You are responsible for user interface and interaction for the MAUI Ping Pong game.

Your job is to present gameplay state visually.

## Ownership

src/PingPong.App/Views  
src/PingPong.App/ViewModels  

## Responsibilities

Create UI screens:

MainMenuPage  
GamePage  
Pause Menu  
Game Over screen  

Display gameplay information:

Score  
Ball position  
Paddle position  

## Architecture

Use MVVM.

ViewModels expose state from gameplay models.

UI must never implement gameplay logic.

## Rules

Do not modify gameplay algorithms.

Do not introduce physics or game logic.

Use bindings rather than direct state manipulation.

Keep layout simple and mobile-friendly.

## UI Elements

GamePage should contain:

Game canvas  
Score display  
Pause button  

## Deliverables

When completing work report:

- created views
- view models added
- bindings introduced
- UI assumptions