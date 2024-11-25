# Unity Flocking Simulation
This project is a flocking simulation in Unity that models the behavior of fish-like entities in a 3D environment. The simulation implements flocking algorithms based on separation, alignment, and cohesion principles to create realistic group dynamics. Additionally, the simulation includes interactive user controls and environmental constraints, such as wall avoidance.

## Project Link
https://github.com/joyce-liu001/Boids-master.git

## Features
- **Flocking Behavior**: Fish follow classic flocking rules:
  - **Separation**: Maintain a safe distance from other fish to avoid crowding.
  - **Alignment**: Move in the same general direction as nearby fish.
  - **Cohesion**: Stay close to the group.
- **Wall Avoidance**: Fish detect and avoid walls and obstacles using raycasting.
- **User Interaction**: The fish follow a goal position that can be set by moving the mouse.
- **Natural Movement**: Randomness in individual movements adds realism.
- **Visuals**: Includes 100 fish (represented as small cones) navigating through a 3D tunnel environment.

## Unity Setup
To begin, download and extract 301588566.zip. Open the extracted folder as a Unity project in Unity Hub and select 2022.3.47f1 as the editor version. 
Later versions should also work, but if you run into issues, use the listed version. 
Once the project is opened, open the scene in Assets -> Scenes -> Obstacles.unity to begin the project.

## Project Script files
All the C# script files are contained in Assets/Scripts folder.
1) Flock.cs 
Governs individual fish behavior, applying the separation, alignment, cohesion rules, wall avoidance, and goal-seeking logic. 

2) GlobalFlock.cs 
Handles global parameters such as tank bounds, the number of fish, and the shared goal position.

## Demo on Youtube
https://youtu.be/Gn-lDyM6e7k
