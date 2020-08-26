using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

// RoomInfo specifies the name and coordinates of a room
public class RoomInfo
{
    public string name;
    // x and y values are in relation to the scene [ e.g. if start room is (0, 0) its neighbor on the right will be (1, 0) ]
    public int x;
    public int y;
}

public class RoomController : MonoBehaviour
{
    // A single instance of the RoomController (class is a Singleton i.e will only ever be one instance of RoomController)
    public static RoomController instance;

    // Name of the current floor
    string currentFloorName = "Basement";

    // RoomInfo of the current room the player is in
    RoomInfo currentLoadRoomData;

    // Current room the player is in
    Room currentRoom;

    // Queue of rooms that need to be loaded into the map
    // Queues are FIFO, so we can use it to load our room (scene) in the desired order
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    // List of rooms that have been succesfully loaded
    public List<Room> loadedRooms = new List<Room>();

    // Determines wether a room is currently being loaded or not
    bool isLoadingRoom = false;

    bool spawnedBossRoom = false;

    bool updatedRooms = false;

    private void Awake()
    {
        // Creates an instance of RoomController on awake
        instance = this;
    }

    private void Update()
    {
        // Loads rooms from the queue
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        // If a room is currently being loaded 
        if(isLoadingRoom)
        {
            return;
        }

        // If there are no more rooms to load
        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if(spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }

                UpdateRooms();
                updatedRooms = true;
            }

            return;
        }

        // Assign the data for the next room we will load in
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        // Starts a Corroutine that loads the next room based on the data we give it
        StartCoroutine(loadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator loadRoomRoutine(RoomInfo info)
    {
        string roomName = currentFloorName + info.name;

        // Unity waits every frame for all of its threads (i.e coroutine) to finish before advancing to the next frame
        // Asynchronous Operation allows you to stop (or pause) execution of code for the current frame and then pick up where it left off on the next one
        // This prevents us from having to wait every time a new room is loaded and lets new rooms be loaded as we play
        // [ LoadSceneMode.Additive allows for scenes to overlap (hence why we can load multiple rooms into one scene) ]
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        // if the loadRoom operation is not complete (i.e the scene/room is not yet loaded) we yield (pause execution of coroutine)
        while (!loadRoom.isDone)
        {
            yield return null;
        }
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.x, bossRoom.y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.x == tempRoom.x && r.y == tempRoom.y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.x, tempRoom.y);
        }
    }
    
    // Adds a room with a specified name and position to the loadRoomQueue
    public void LoadRoom (string name, int x, int y)
    {
        // if the room already exists we stop
        if(DoesRoomExist(x, y))
        {
            return;
        }


        RoomInfo newRoomData = new RoomInfo();

        newRoomData.name = name;
        newRoomData.x = x;
        newRoomData.y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    // Adds room to the game world
    public void RegisterRoom(Room room)
    {
        // Only load room if there is no existing room at the position
        if (!DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y))
        {
            room.transform.position = new Vector3(
            currentLoadRoomData.x * room.width,
            currentLoadRoomData.y * room.height,
            0);

            room.x = currentLoadRoomData.x;
            room.y = currentLoadRoomData.y;
            room.name = currentFloorName + "-" + currentLoadRoomData.name + " " + room.x + " " + room.y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            // if no rooms have been loaded yet
            if (loadedRooms.Count == 0)
            {
                // We pass the room that is currently being loaded to the camera
                CameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);
           
        }
        else
        {
            // Destroy the room that is trying to be loaded on top of another
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }

        

    }

    // Checks if a room existss at a specified location
   public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    }

    // Returns room at specified coordinates
    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[] {
            "Empty",
            "Basic"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    // Upon entering a new room 
    public void onPlayerEnterRoom(Room room)
    {
        // Camera is notified what room the player is currently in
        CameraController.instance.currentRoom = room;
        currentRoom = room;
        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(2f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (room!=currentRoom)
            {
                EnemyController[] enemies = room.GetComponents<EnemyController>();
                if (enemies != null)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                        Debug.Log("Not in room");
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
            }
            else
            {

                EnemyController[] enemies = room.GetComponents<EnemyController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                        Debug.Log("In room");
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
            }
                
            
        }
        
    }  
    
}
