# puck-push-hockey
Monogame project for an aero hockey-like online multiplayer game
It was created in less than 3 days so a lot of things may not look nice and aren't handled gracefully (menus, connecting to server etc)

You can download it from here: https://github.com/vaytaliy/puck-push-hockey/tree/main/PuckPush/Hockey/bin/Debug/netcoreapp3.1/publish

## Features:
1. Hand implemented 2D physics engine which includes collision detection, ice sliding physics, deflection, collision, acceleration
2. Play 1v1 offline with 2 players on the same computer
3. Play 1v1 on separate computers (LAN or Internet)
4. UI is quite a lot of work to implement in MonoGame, so console is used in parallel to allow inputing server address if you want to play online

## Note about online:

if you play online you may need to port-forward in settings by allowing UDP on port 8005, don't need to do this for LAN. Check your public IP address and share it to the client (friend who will connect to play)

Once server is created click on the local game button wait for 2nd player to connect and they have to press the same button

