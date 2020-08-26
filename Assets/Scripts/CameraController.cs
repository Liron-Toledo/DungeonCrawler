using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Singleton instance
    public static CameraController instance;
    public Room currentRoom;
    public float moveSpeedBetweenRooms;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if(currentRoom == null)
        {
            return;
        }

        // Gets the target position that we want our camera to move to
        Vector3 targetPos = GetCameraTargetPosition();
        // camera moves toward the target position at a specified speed
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedBetweenRooms);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
        {
            // Camera remains in its original position
            return Vector3.zero;
        }

        // Camera needs to move to the centre of the room the player is currently in
        Vector3 targetPos = currentRoom.GetRoomCentre();
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool isSwitchingScene()
    {
        // The camera is switching scenes if its position is not equal to its target position
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
