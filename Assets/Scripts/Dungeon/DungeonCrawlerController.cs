using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
};

public class DungeonCrawlerController : MonoBehaviour
{
    /*
    Vector2Int are 2D vectors that can only be integers, they are commonly used to represent grid like structures
    
    Vector2Int.down	    Shorthand for writing Vector2Int (0, -1)
    Vector2Int.left	    Shorthand for writing Vector2Int (-1, 0)
    Vector2Int.right	Shorthand for writing Vector2Int (1, 0)
    Vector2Int.up	    Shorthand for writing Vector2Int (0, 1)
    Vector2Int.zero     Shorthand for writing Vector2Int (0, 0)
    */

    // List of the coordinates that a crawler has visited
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
    
    // Maps are Direction enums to the appropriate Vector2Int translation
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        { Direction.up, Vector2Int.up },
        { Direction.left, Vector2Int.left },
        { Direction.down, Vector2Int.down },
        { Direction.right, Vector2Int.right },
    };

    // Iterates through each crawler and fetches which positions each crawlers has visited.
    // Rooms can then be spawned at those positions
    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();
        for (int i = 0; i < dungeonData.numCrawlers; i++)
        {
            // Creates crawler at (0, 0) [starting room]
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        int iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for (int i = 0; i < iterations; i++)
        {
            foreach (DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPos = dungeonCrawler.Move(directionMovementMap);
                positionsVisited.Add(newPos);
            }
        }

        return positionsVisited;
    }

}
