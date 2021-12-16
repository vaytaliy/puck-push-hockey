# puck-push-hockey
Monogame project for an aero hockey-like online multiplayer game

Purpose of the project:
1. Practice c# and OOP 
2. Learn basics about multithreading
3. Low level of web app interfacing (WebSockets on UDP protocol)
4. Peer-to-peer communication

Features:
1. Hand implemented 2D physics engine which includes collision detection, ice sliding physics, deflection, collision, acceleration of collided object based off speed at which it got hit
2. Play 1v1 offline with 2 players on the same computer
3. Play 1v1 on separate computers (LAN or Internet)
4. UI is quite a lot of work to implement in MonoGame, so console is used in parallel to allow inputing server address if you want to play online
