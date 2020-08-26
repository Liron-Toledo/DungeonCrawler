using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]
    public struct Grid
    {
        public float columns, rows;
        public float verticalOffSet, horizontalOffset;
    }

    public Grid grid;
    public GameObject gridTile;
    public List<Vector2> availblePoints = new List<Vector2>();
    //Need to adjust the grid sprites pixels per unit when changing this value
    public float IterationIncrement = 0.5f;

    private void Awake()
    {
        room = GetComponentInParent<Room>();
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid.verticalOffSet += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;

        for (float y = 0; y < grid.rows; y+= IterationIncrement)
        {
            for (float x = 0; x < grid.columns; x+= IterationIncrement)
            {
                GameObject go = Instantiate(gridTile, transform);
                go.GetComponent<Transform>().position = new Vector2(x - (grid.columns - grid.horizontalOffset), y - (grid.rows - grid.verticalOffSet));
                go.name = "X: " + x + "Y: " + y;
                availblePoints.Add(go.transform.position);
            }
        }

        GetComponentInParent<ObjectRoomSpawner>().InitialiseObjectSpawning();
    }
}
