# Battleship Game Documentation

## Overview
The Battleship Game project is a full-stack application that allows players to engage in a classic Battleship game against an AI opponent. The project is divided into a frontend built with Angular and a backend developed using .NET 9.0.

## Project Structure

### Frontend
- **Framework**: Angular
- **Directory**: `battleship-frontend/`
- **Key Features**:
  - Drag-and-drop ship placement.
  - Real-time updates for moves and ship placement.
- **Key Files**:
  - `src/app/app.component.html`: Main HTML structure for the game.
  - `src/app/battleship.service.ts`: Service handling game logic and API communication.

### Backend
- **Framework**: .NET 9.0
- **Directory**: `BattleshipBackend/`
- **Key Features**:
  - API endpoints for game logic.
  - Core game logic implementation.
- **Key Files**:
  - `BattleshipGameService.cs`: Contains the core game logic.
  - `Controllers/BattleshipController.cs`: Defines API endpoints for the game.

### Tests
- **Backend Tests**:
  - Directory: `BattleshipBackend.Tests/`
  - Includes unit tests for backend logic and API endpoints.

## Prerequisites
- **Node.js**: Required for running the Angular frontend.
- **.NET SDK**: Required for building and running the backend.

## How to Run

### Frontend
1. Navigate to the `battleship-frontend/` directory.
2. Run `npm install` to install dependencies.
3. Run `ng serve` to start the development server.
4. Access the game at `http://localhost:4200`.

### Backend
1. Navigate to the `BattleshipBackend/` directory.
2. Run `dotnet build` to build the project.
3. Run `dotnet run` to start the backend server.
4. The API will be available at `http://localhost:5000`.

## Features
- **Frontend**:
  - Interactive UI for ship placement and gameplay.
  - Real-time updates for moves and game state.
- **Backend**:
  - Turn-based gameplay logic.
  - AI opponent logic.
  - RESTful API for frontend communication.

## Contributing
1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Submit a pull request with a clear description of your changes.

## License
This project is licensed under the MIT License. See the LICENSE file for details.
