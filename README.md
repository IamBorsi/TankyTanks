# TankyTanks
A multiplayer tank shooting game to test Unity NET Code.

### First approach with Unity's Net Code
In this little project, I am approaching for the first time the Unity NET Code while trying to explore Unity itself. I have been using Unity for no longer than a few months now and it has been quite a steep learning experience.
NET Code definitely adds quite some challenge to the mix since I have to design everything to work on a Network rather than a local instance. It is a nice experience and I'm learning quite a few new tricks.

### Input System
For this project, I am using the new Unity Input System so that the Input Manager can be device independent. The player movement is handled by default with the WASD keys registered as a Vector2 input that moves the entire body. The player will be able to rotate using the A-D keys and move back and forth using the W-S keys.
The turret movement/rotation is handled using the mouse input. Moving on the Y-axis will impact the pitch, while the X-axis will impact the Yaw.
