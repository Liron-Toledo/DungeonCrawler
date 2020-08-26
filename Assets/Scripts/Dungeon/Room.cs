using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Determines size of each room
    public int width;
    public int height;

    // Determines placement on the grid of each room
    public int x;
    public int y;

    private bool updatedDoors = false;

    public Room (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public List<Door> doors = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {
        // Ensures that we start in the right scene (only Dungeon main contains the room controller)
        if(RoomController.instance == null)
        {
            Debug.LogError("You pressed play in the wrong scene!");
            return;
        }

        Door[] drs = GetComponentsInChildren<Door>();
        foreach (Door d in drs)
        {
            doors.Add(d);

            switch (d.doorType)
            {
                case Door.DoorType.right: 
                    rightDoor = d;
                    break;

                case Door.DoorType.left: 
                    leftDoor = d;
                    break;

                case Door.DoorType.top: 
                    topDoor = d;
                    break;

                case Door.DoorType.bottom: 
                    bottomDoor = d;
                    break;

            }
        }

        // Adds newly created room to the game world 
        RoomController.instance.RegisterRoom(this);
    }

    private void Update()
    {
        if (name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach(Door door in doors)
        {
            switch(door.doorType)
            {
                case Door.DoorType.right:
                    if (GetRight() == null)
                        door.gameObject.SetActive(false);
                    break;

                case Door.DoorType.left:
                    if (GetLeft() == null)
                        door.gameObject.SetActive(false);
                    break;

                case Door.DoorType.top:
                    if (GetTop() == null)
                        door.gameObject.SetActive(false);
                    break;

                case Door.DoorType.bottom:
                    if (GetBottom() == null)
                        door.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public Room GetRight()
    {
        if(RoomController.instance.DoesRoomExist(x+1, y))
        {
            return RoomController.instance.FindRoom(x+1, y);
        }
       
        return null;
    }
    public Room GetLeft()
    {
        if (RoomController.instance.DoesRoomExist(x-1, y))
        {
            return RoomController.instance.FindRoom(x-1, y);
        }

        return null;
    }
    public Room GetTop()
    {
        if (RoomController.instance.DoesRoomExist(x, y+1))
        {
            return RoomController.instance.FindRoom(x, y+1);
        }

        return null;
    }
    public Room GetBottom()
    {
        if (RoomController.instance.DoesRoomExist(x, y-1))
        {
            return RoomController.instance.FindRoom(x, y-1);
        }

        return null;
    }

    // Used to visualise width and height of room in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3(x * width, y * height);
    }

    // If the player collides with the wall it means the player is entering a new room
    // TODO: need to make this based on doors rather than wall
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            RoomController.instance.onPlayerEnterRoom(this);
        }
    }
}
