 Cube Crushers is a pong meets brick AR game where you move around outside in the real world, your goal is to hit a ball into cubes to destroy them. Destroy all the cubes to win the level before the time runs out!

Cube crusher supports solo play and online multiplayer using Lightships SharedAR Image Colocation functionality. The image used for colocalization is below(It is just a black and white version of the one provided with the sample project). The dimensions for the image are currently set to 0.13 meters(This can be changed on the RoomManager script attached to the NetworkManager gameobject in the Match scene). When using image Colocalization you should put the image on the ground in the middle of where you want the playing field to be. The game has been tested with 2 players, feel free to add more but your mileage may vary 🙂

The basic mechanics of the game are simple. There is a bouncy ball which you can hit by tapping on the screen when you are close to it. If the bouncy ball hits a cube it will destroy the cube. If the bouncy ball hits the wall behind a player then you lose points(Basically you don’t want to let the ball get past you).


To test the game, build it to your device and run it, then the steps will vary slightly based on whether you are playing by yourself or with someone else.
For Solo testing :
1. Click the “Create Room” button(Even though you are going to play by yourself you still need to make a room)
2. Once you are put in a room click the “Start Match” button
3. A ball will drop from the sky and a cube will spawn in the middle of the field. Try to destroy all the cubes before the time(displayed at the top of the screen)runs out.

For multiplayer as a host :
1. Click the “Create Room” button
2. Once you are put in a room look at the Colocalization image to start the Colocalization.
3. Wait until all the other players have joined and also Colocalized.
4. Click the “Start Match” button
5. A ball will drop from the sky and a cube will spawn in the middle of the field. Try to destroy all the cubes before the time(displayed at the top of the screen)runs out.

For multiplayer as a client:
1. After the host has created the room click the “Refresh List” button to refresh the list of available rooms.
2. Scroll through the list of rooms to find the one you want to join, the rooms are designated by name(Pro tip : you can click the “Delete Rooms” button to clear out all the rooms, this will also delete the room that the host just created though so be carful)
3. Click on the room you want to join
4. Once you are put in a room look at the Colocalization image to start the Colocalization.
5. Wait for the host to click the “Start Match” button
6. A ball will drop from the sky and a cube will spawn in the middle of the field. Try to destroy all the cubes before the time(displayed at the top of the screen)runs out.


Lightship features used : 
Shared AR
This is the big one. I used Lightship’s transport along with image Colocalization and the built in rooms API to enable multiplayer in my AR experience. For the netcode I used NGO(Netcode for Gameobjects).

Meshing 
I use meshing for the ground shadows in the game, I tried using it for the collisions also but it is not very fun when your ball bounces of in random directions so I decided to not use collision on the generated meshes.


Lightship thoughts : 
Overall lightship is really easy to add to a project and has some super cool functionality. Testing/debugging can be challenging with realtime multiplayer experiences but Playback is a great way to see how your AR features work in editor. The rooms were super easy to setup and use but I wasn’t exactly sure how I should go about cleaning them up after matches were complete. The multiplayer synchronization is very good and definitely one of the highlights when you can launch something towards your friend in AR! I did run into some issues with the shared AR experience desynchronizing during a match, this is definitely a pain, but it reminds me of how wii motes would sometimes get desynchronized. As long as the game dev accounts for this and builds in a pause functionality to allow the players to resync(maybe desynchronization could even be detected and the player could be prompted to resync) the impact can be minimized. That said I didn’t get around to trying to implement this myself ;).
Another thing that would be super cool is if the phones could communicate faster since they are right next to each other(maybe via bluetooth or something, I’m sure Niantic has put more thought into it then I have 🙂). Right now since all my messages are going through a relay server it seems that a hundred plus milliseconds of latency is pretty normal(which isn’t bad by any means, but one can always wish).
I also tried to implement Occlusion but it was always causing flickering, for example the in game cyber walls would flicker, or the ball would flicker near the ground. I tried to add semantic segmentation suppression but still ran into the same thing so I decided to take it out, I might have just not configured it right though.

NGO implementation :
A big part of any multiplayer experience is the way you implement the netcode. I found my demo to pose a special challenge since it was pretty real time/fast paced. I tried using network transforms but had multiple issues going down that route, so instead I had the balls motions simulated separately on each client and only synchronized during certain events like when a player hits the ball or when the balls position gets reset because it hit the back wall(Which only happens in multiplayer). Also in order to help mitigate the effects of latency I had the ball move slower for the client that just hit it. That way the ball would hopefully get across the field to the other player at about the same time on each client. 
This is purely an implementation detail, but the way I used NGO a lot of times was by splitting what normally would be one function into 4.
Starter function : This is the function that would be called on the client that wanted something to happen, it would immediately call the Action function so that the local client could see the effect right away and also call the ServerRpc to synchronize the activity.
ServerRpc : This would just call the ClientRpcs to synchronize the action
ClientRpc : This would call the Action function on all the client except the initiating client(since the action function was already called on that client)
Action function : this would actually do the thing that I wanted to have happen, this function would pretty much just be a normal Unity function.
