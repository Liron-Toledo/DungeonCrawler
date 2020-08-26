using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoomSpawner : MonoBehaviour
{

    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;
        public SpawnerData spawnerData;
    }

    public GridController grid;
    public RandomSpawner[] spawnerData;
    
    public void InitialiseObjectSpawning()
    {
        foreach (RandomSpawner rs in spawnerData)
        {
            SpawnObject(rs);
        }
    }
    
    void SpawnObject(RandomSpawner data)
    {
        int randomIteration = Random.Range(data.spawnerData.minSpawn, data.spawnerData.maxSpawn + 1);

        for(int i = 0; i < randomIteration; i++)
        {
            int randomPos = Random.Range(0, grid.availblePoints.Count - 1);
            GameObject go = Instantiate(data.spawnerData.itemToSpawn, grid.availblePoints[randomPos], Quaternion.identity, transform) as GameObject;
            grid.availblePoints.RemoveAt(randomPos);
            //Debug.Log("Spawned Object");
        }
    }
}
