# OptiTrackBasedXRDroneInterface

This is a Unity project designed to integrate drones with XR (Extended Reality) interfaces using the Quest3 and OptiTrack motion capture system. This repository correspond to the Indoor OptiTrack-Based XR-Drone Interface developed for the master thesis **Robust In-and Outdoor Drone Control and Path Visualization via XR Interface**. The outdoor interface source code can also be found here: https://github.com/jguillenpastor/ArUcoBasedXRDroneInterface.git.

In addition, the source code for a developed Virtual Drone Simulation can be also found in both repositories.

## 🎥 Demo Video

[![Watch the demo](https://img.youtube.com/vi/IehHCEsYN-M/0.jpg)](https://www.youtube.com/watch?v=IehHCEsYN-M)

## 📦 Project Structure

- `Assets/`: Main game assets and scripts.
- `ProjectSettings/`: Unity project configuration files.
- `Packages/`: External packages and dependencies.

## 🚀 Requirements

- Unity 2022.3.5f1 or higher (LTS recommended)
- Configured OptiTrack motion capture system
- Required SDKs for XR and drone control

## 🛠️ Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/jguillenpastor/OptiTrackBasedXRDroneInterface.git

## 🧩 Scripts OptiTrack-Based XR-Drone Interface Descriptions

In Assets>Scripts>ROS

- **DroneMovement.cs** — Updates the virtual drone's position in Unity by transforming the Optitrack position of the real drone to Unity reference frame using the headset initial position.

- **DronePositionLogger.cs** — Logs the drone and headset positions in both Unity and ROS coordinate systems into CSV files for further analysis.

- **VRPNPoseSubscriber.cs** — Subscribes to ROS topics to retrieve and convert drone and headset pose and velocity data into Unity coordinates, storing the initial headset position for alignment.

- **WaypointSubscriber.cs** — Subscribes to ROS waypoint messages, transforms them into Unity coordinates, and instantiates waypoint markers in the scene relative to the user's initial headset position.

In Assets>Scripts>InfoDisplay

- **FloatingDroneLabel.cs** — Displays a floating UI label above the drone showing its real-time velocity and altitude, with dynamic font size based on the camera distance.

- **DroneHighlightRectangle.cs** — Renders a rectangular outline around the drone using a `LineRenderer`, always facing the user to improve visibility and tracking.

In Assets>Scripts>DroneScripts

- **Drone_Line_render.cs** — Draws a trail line showing the drone's movement path over time using a `LineRenderer`, adding new points only when the drone has moved a minimum distance.

## 🧩 Scripts Virtual Drone Simulation Descriptions

In Assets>Scripts>DroneScripts

- **IP_Drone_Inputs.cs** — Captures user input from keyboard and mouse to generate velocity and angular velocity commands for the drone controller.

- **IP_Dronecontroller.cs** — Core drone physics and input handling logic. Applies vertical and horizontal forces and manages drone rotation based on velocity commands.

- **PID_Controller.cs** — Implements a generic PID control algorithm to calculate corrections based on error input; used for waypoint navigation.

- **ScaleCylinder.cs** — Dynamically scales the Y-height of a cylinder in response to Meta Quest thumbstick input, simulating growth or shrinkage.

- **TargetHandler.cs** — Allows spawning and deleting target objects in the scene using Oculus Touch buttons, maintaining a list of dynamically added targets.

- **Drone_Line_render.cs** — Draws a trail line showing the drone's movement path over time using a `LineRenderer`, adding new points only when the drone has moved a minimum distance.

