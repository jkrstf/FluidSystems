\# Fluidic System Control Suite



This project is a .NET 9 and WPF–based framework for modeling, simulating, and controlling complex fluidic systems.



\## Key Features



\* \*\*Dynamic Topology Management\*\*: Complete fluidic networks (components and connections) are defined via external \*\*JSON files\*\*.

\* \*\*Graph-Based Simulation\*\*: Features real-time flow propagation and material tracking using a \*\*Breadth-First Search (BFS)\*\* algorithm.

\* \*\*Simplified Physical Model\*\*: The system operates on a "quasi-infinite" pressure and velocity model, focusing on logical routing and material integrity.

\* \*\*Generalized Control\*\*: A robust, industrial interface for component manipulation, equipped with built-in safety interlocks and validation.



\## Tech Stack



\* \*\*Framework\*\*: .NET 9

\* \*\*UI\*\*: WPF

\* \*\*Data\*\*: JSON-based serialization for system definitions

\* \*\*Algorithms\*\*: Graph Theory / BFS for material distribution tracking



\## Architecture



The project consists of two primary layers:

1\.  \*\*Core Library\*\*: The engine responsible for the graph model, simulation algorithms, and state management.

2\.  \*\*WPF Application\*\*: The presentation layer featuring an interactive P\&ID (Piping and Instrumentation Diagram), control panel, diagnostics and logs view.



\## Functional Capabilities



\### Control Modes

\* \*\*Manual Control\*\*: A generalized interface that allows for the safe toggling of any component within the network. It features safety-first logic to prevent invalid states.

\* \*\*Automated Sequences\*\*: Includes high-level functions for:

&nbsp;   \* \*\*Chamber Fill\*\*: Automated routing from a source to target chamber.

&nbsp;   \* \*\*Chamber Empty\*\*: Automated routing from a chamber to target sink.

&nbsp;   \* \*\*Manifold Purge\*\*: Clearing intermediate pipes with air.





&nbsp;\*\*Current Status \& Extensibility\*\*: The project is in its early stages. Currently, a default JSON system definition and layout are automatically loaded at startup. However, the internal services are fully architected to read from JSON, meaning the fluidic system and the UI layout are technically swappable by providing different configuration files, although the automated sequences only supports the built-in system at the moment.



\## Getting Started



1\.  \*\*System Initialization\*\*: Upon startup, the system services automatically parse the integrated JSON configuration to build the internal graph model.

2\.  \*\*Dynamic Generation\*\*: The UI validates the graph integrity and generates the interactive layout dynamically based on the loaded definitions.

3\.  \*\*Simulate\*\*: Interact with the diagram and control panel; the BFS engine will instantly recalculate material flow based on valve positions.

