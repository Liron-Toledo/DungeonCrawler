using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;
    private bool itemRoomSpawned = false;
    public int itemRoomSpawnChance = 10;

    private void Start()
    {
        // Contains all the rooms (positions) that the crawlers have visited
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    // Spawns rooms into the game world
    // Boss room is spawned elsewhere (refer to RoomController)
    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);
        foreach (Vector2Int roomLocation in rooms)
        {
            if (itemRoomSpawned == false && Random.Range(0,100) < itemRoomSpawnChance)
            {
                RoomController.instance.LoadRoom("Item", roomLocation.x, roomLocation.y);
                itemRoomSpawned = true;
            }
            else
            {
                RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
                itemRoomSpawnChance += 10;
            }

            
        }
    }
}
