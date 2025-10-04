# Unity Roulette Prototype

## Gameplay Demo

[![Gameplay Video](https://img.youtube.com/vi/a-o2dvJZP2M/maxresdefault.jpg)](https://www.youtube.com/watch?v=a-o2dvJZP2M)

## Controls and Gameplay Instructions

### Placing Bets
1.  **Select a Chip**: Click on one of the chip denominations at the bottom of the screen. The selected chip will be highlighted.
2.  **Place Chip on Table**: Click on any of the designated betting spots on the roulette table. This includes individual numbers, dozens, columns, red/black, etc.
3.  **Stacking Chips**: You can click multiple times on the same spot to stack chips and increase the bet amount.
4.  **Player Balance**: Your current balance is displayed in the bottom-right corner and will update automatically as you place bets.

### Spinning the Wheel
-   Once you are satisfied with your bets, press the **"Spin"** button on the right to start the roulette wheel.
-   Once the spin is initiated, no more bets can be placed.

### Deterministic Mode (For Testing)
The prototype includes a special mode for testing specific outcomes.
1.  **Enable Deterministic Mode**: Check the "Deterministic" toggle at the top of the screen.
2.  **Select Outcome**: An input field will become active. Enter the number (0-36) you want the ball to land on.
3.  **Spin**: Press the "Spin" button. The ball will now land on the number you selected.

## Design Patterns Utilized

This project leverages several common design patterns to create a structured, scalable, and maintainable codebase.

### 1. Service Locator
-   **Description**: The Service Locator pattern provides a central registry for services (dependencies), allowing different parts of the application to get references to services without needing a direct link.
-   **Implementation**: The `DI/ServiceLocator.cs` class is a static class that holds a dictionary of service interfaces and their concrete implementations. The `GameInitializer.cs` script is responsible for creating instances of all services (e.g., `BettingManager`, `AudioManager`) and registering them with the `ServiceLocator` at the start of the game. Other classes then call `ServiceLocator.Get<IMyService>()` to retrieve dependencies.

### 2. Observer
-   **Description**: The Observer pattern allows objects to subscribe to and receive notifications about events happening in other objects, without the objects having to be tightly coupled.
-   **Implementation**: C# `event` and `Action` delegates are used extensively. For example:
    -   `IBettingManager.OnBetsCleared`: UI elements subscribe to this event to know when to clear chip visuals from the board.
    -   `IWheelController.OnSpinComplete`: The `GameManager` subscribes to this to know when the wheel has finished spinning, triggering the payout sequence.
    -   `IStatisticService.SpinRecorded`: The `StatisticsUI` subscribes to this to update the displayed stats after each spin.

### 3. Object Pool
-   **Description**: This pattern is used to reuse objects that are expensive to create, such as GameObjects. Instead of destroying objects, they are returned to a "pool" to be reused later.
-   **Implementation**: The generic `PoolService<T>` class implements the core pooling logic. The `ChipManager` uses this service to manage pools for each type of chip visual. When a chip is placed, it's taken from the pool (`Get()`), and when bets are cleared, the chip visuals are returned to the pool (`Return()`) instead of being destroyed.

### 4. State (Implicit)
-   **Description**: The State pattern allows an object to alter its behavior when its internal state changes.
-   **Implementation**: The `GameManager` uses a `GameState` enum (`Betting`, `Spinning`, `Payout`). While not a formal implementation with separate classes for each state, the `ChangeState` method and the logic within `GameManager` that checks `_currentState` effectively mimic this pattern. For example, the `SpinButtonPressed()` method only works if the state is `GameState.Betting`.

### 5. Strategy (Implicit)
-   **Description**: The Strategy pattern defines a family of algorithms, encapsulates each one, and makes them interchangeable.
-   **Implementation**: The static `BetRules` class acts as a provider for different betting strategies or rules. The `GetPayout(BetType)` and `GetBetNumbers(BetType)` methods return different values or number sets based on the `BetType` enum provided. This decouples the payout and win-checking logic from the objects that need this information.

### 6. Data Container (ScriptableObject)
-   **Description**: A common Unity-specific pattern where `ScriptableObject`s are used to create data assets that exist outside of scenes, making it easy to manage and edit configuration data.
-   **Implementation**:
    -   `ChipDatabaseSO`: Holds a list of all available chip types, their values, sprites, and prefabs.
    -   `SoundDatabaseSO`: Contains a list of all audio clips, mapping them by a string key for easy access by the `AudioManager`.

## Known Issues & Future Improvements

### Architectural Refinements
-   **Service Locator vs. Dependency Injection**: The current Service Locator pattern, while effective, can hide dependencies and make unit testing more difficult. A future improvement would be to replace it with a true Dependency Injection framework like **VContainer** or **Zenject**, which makes dependencies explicit.
-   **`async void` Methods**: Some methods (e.g., in `GameManager`, `WheelController`) use `async void`. This should generally be avoided in favor of `async Task` to improve error handling and task composition. `async void` should primarily be reserved for top-level event handlers.
-   **Complex Logic in `BettingManager.RestoreState`**: The logic for restoring chip visuals on the board is complex and involves nested loops. This could be refactored for better performance and readability, perhaps by using a dictionary to map bet types and numbers to their corresponding UI spots.

### Future Features
-   **Back-Revert Button**: Add a button to allow players to revert their last bet action. I have plan to implement via command pattern.
-   **Expanded Bet Types**: Implement more complex roulette bets like double zero (American Roulette). I will have plan to add this but finding competible assets is a challenge.
-   **UI/UX Polish**: Add more animations, visual feedback, and refined graphics.
-   **Refactor `GameManager`**: The `GameManager` currently has many responsibilities. It could be broken down further, with game state logic potentially being handled by a hierarchical Finite State Machine.
