# Battleship Game

## Overview
This project is a Battleship game implementation consisting of a frontend built with Angular and a backend developed in .NET. The game allows players to place ships, make moves, and compete against an AI opponent.

## Project Structure

### Frontend
- **Framework**: Angular
- **Location**: `battleship-frontend/`
- **Key Files**:
  - `src/app/app.component.html`: Main HTML structure for the game.
  - `src/app/battleship.service.ts`: Service handling game logic and API communication.

### Backend
- **Framework**: .NET 9.0
- **Location**: `BattleshipBackend/`
- **Key Files**:
  - `BattleshipGameService.cs`: Core game logic.
  - `Controllers/BattleshipController.cs`: API endpoints for the game.

### Tests
- **Backend Tests**: Located in `BattleshipBackend.Tests/`.

## How to Run

### Prerequisites
- **Node.js**: Required for the Angular frontend.
- **.NET SDK**: Required for the backend.

### Steps
1. **Frontend**:
   - Navigate to `battleship-frontend/`.
   - Run `npm install` to install dependencies.
   - Run `ng serve` to start the development server.
   - Access the game at `http://localhost:4200`.

2. **Backend**:
   - Navigate to `BattleshipBackend/`.
   - Run `dotnet build` to build the project.
   - Run `dotnet run` to start the backend server.
   - The API will be available at `http://localhost:5000`.

## Features
- Drag-and-drop ship placement.
- Turn-based gameplay against an AI.
- Real-time updates for ship placement and moves.

## Contributing
Feel free to fork this repository and submit pull requests. Ensure your code follows the project's coding standards.

## License
This project is licensed under the MIT License.
