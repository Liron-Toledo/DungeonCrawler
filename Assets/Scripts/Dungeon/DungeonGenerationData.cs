using UnityEngine;

/*
  Scriptable Objects are amazing data containers. 
  They don't need to be attached to a GameObject in a scene. 
  They can be saved as assets in our project. 
  Most often, they are used as assets which are only meant to store data.
 */


// Determines the name of our scritableObject in the unity editor
[CreateAssetMenu(fileName ="DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon Data")]
public class DungeonGenerationData : ScriptableObject
{
    public int numCrawlers;
    public int iterationMin;
    public int iterationMax;
}

/* 
   Algorithm will work as follows:

    A "Crawler" will chose a direction to travel from the room it is currently in. 
    Whatever direction it chooses will determine where a new room will be created.
    Every decison a "Crawler" makes counts as one iteration.
    A "Crawler" will make a number of decisions somewhere in between iterationMin and iterationMax. 
*/